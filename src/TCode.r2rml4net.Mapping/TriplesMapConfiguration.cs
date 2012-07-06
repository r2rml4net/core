using System;
using System.Collections.Generic;
using System.Linq;
using TCode.r2rml4net.RDF;
using VDS.RDF;
using System.Text.RegularExpressions;
using System.Text;
using VDS.RDF.Query;

namespace TCode.r2rml4net.Mapping
{
    /// <summary>
    /// Implementation of fluent configuration interface for <a href="http://www.w3.org/TR/r2rml/#triples-map">Triples Maps</a>
    /// </summary>
    internal class TriplesMapConfiguration : BaseConfiguration, ITriplesMapConfiguration, ITriplesMapFromR2RMLViewConfiguration, ITriplesMap
    {
        private static readonly Regex TableNameRegex = new Regex(@"([\p{L}0-9 _]+)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        private string _triplesMapUri;
        private SubjectMapConfiguration _subjectMapConfiguration;
        private readonly IList<IPredicateObjectMapConfiguration> _propertyObjectMaps = new List<IPredicateObjectMapConfiguration>();

        internal TriplesMapConfiguration(IGraph r2RMLMappings)
            : base(r2RMLMappings)
        {
        }

        #region Implementation of ITriplesMapConfiguration

        /// <summary>
        /// <see cref="ITriplesMapConfiguration.TableName"/>
        /// </summary>
        public string TableName
        {
            get
            {
                if (_triplesMapUri != null)
                {
                    var result = (SparqlResultSet)R2RMLMappings.ExecuteQuery(string.Format(@"
                                    PREFIX rr: <http://www.w3.org/ns/r2rml#>

                                    SELECT ?tableName
                                    WHERE 
                                    {{
                                      <{0}> rr:logicalTable ?lt .
                                      ?lt rr:tableName ?tableName
                                    }}", _triplesMapUri));

                    if (result.Count > 1)
                        throw new InvalidTriplesMapException("Triples map contains multiple table names", Uri);

                    if (result.Count == 1)
                        return result[0].Value("tableName").ToString();
                }
                return null;
            }
            internal set
            {
                if (this.TableName != null)
                    throw new InvalidTriplesMapException("Table name already set", Uri);
                if (this.SqlQuery != null)
                    throw new InvalidTriplesMapException("Cannot set both table name and SQL query", Uri);

                if (value == null)
                    throw new ArgumentNullException("value");
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentOutOfRangeException("value");

                string tablename = TrimTableName(value);
                if (tablename == string.Empty)
                    throw new ArgumentOutOfRangeException("value", "The table name seems invalid");

                AssertTableNameTriples(tablename);
            }
        }

        private static string TrimTableName(string tablename)
        {
            var regexMatch = TableNameRegex.Match(tablename);

            StringBuilder stringBuilder = new StringBuilder(tablename.Length);

            if (regexMatch.Success)
            {
                stringBuilder.Append(regexMatch.Value);
                regexMatch = regexMatch.NextMatch();

                while (regexMatch.Success)
                {
                    stringBuilder.AppendFormat(".{0}", regexMatch.Value);
                    regexMatch = regexMatch.NextMatch();
                }
            }

            return stringBuilder.ToString();
        }

        private void AssertTableNameTriples(string tablename)
        {
            // TODO: refactor with new version of dotNetRDF
            _triplesMapUri = string.Format("{0}{1}TriplesMap", R2RMLMappings.BaseUri, tablename);

            IBlankNode tableDefinition;
            AssertTriplesMapsTriples(out tableDefinition);

            var tableName = R2RMLMappings.CreateUriNode(R2RMLUris.RrTableNameProperty);
            var tableNameLiteral = R2RMLMappings.CreateLiteralNode(tablename);

            R2RMLMappings.Assert(tableDefinition, tableName, tableNameLiteral);
        }

        private void AssertSqlQueryTriples(string sqlQuery)
        {
            // TODO: refactor for something else than GUID
            _triplesMapUri = string.Format("{0}{1}TriplesMap", R2RMLMappings.BaseUri, Guid.NewGuid());

            IBlankNode tableDefinition;
            AssertTriplesMapsTriples(out tableDefinition);

            var sqlQueryLiteral = R2RMLMappings.CreateLiteralNode(sqlQuery);
            var sqlQueryProperty = R2RMLMappings.CreateUriNode(R2RMLUris.RrSqlQueryProperty);

            R2RMLMappings.Assert(tableDefinition, sqlQueryProperty, sqlQueryLiteral);
        }

        /// <summary>
        /// <see cref="ITriplesMapConfiguration.SqlQuery"/>
        /// </summary>
        public string SqlQuery
        {
            get
            {
                if (_triplesMapUri != null)
                {
                    var result = (SparqlResultSet)R2RMLMappings.ExecuteQuery(string.Format(@"
                                    PREFIX rr: <http://www.w3.org/ns/r2rml#>

                                    SELECT ?sqlQuery
                                    WHERE 
                                    {{
                                      <{0}> rr:logicalTable ?lt .
                                      ?lt rr:sqlQuery ?sqlQuery
                                    }}", _triplesMapUri));

                    if (result.Count > 1)
                        throw new InvalidTriplesMapException("Triples map contains multiple SQL queries", Uri);

                    if (result.Count == 1)
                        return result[0].Value("sqlQuery").ToString();
                }
                return null;
            }
            internal set
            {
                if (this.SqlQuery != null)
                    throw new InvalidTriplesMapException("SQL query already set", Uri);
                if (this.TableName != null)
                    throw new InvalidTriplesMapException("Cannot set both table name and SQL query", Uri);

                if (value == null)
                    throw new ArgumentNullException("value");
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentOutOfRangeException("value");

                AssertSqlQueryTriples(value);
            }
        }

        /// <summary>
        /// <see cref="ITriplesMapConfiguration.Uri"/>
        /// </summary>
        public Uri Uri
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_triplesMapUri))
                    return null;
                
                return new Uri(Uri.EscapeUriString(_triplesMapUri));
            }
        }

        private void AssertTriplesMapsTriples(out IBlankNode tableDefinition)
        {
            var tripleMap = R2RMLMappings.CreateUriNode(Uri);
            var tripleMapClass = R2RMLMappings.CreateUriNode(R2RMLUris.RrTriplesMapClass);
            var type = R2RMLMappings.CreateUriNode(R2RMLUris.RdfType);
            var logicalTable = R2RMLMappings.CreateUriNode(R2RMLUris.RrLogicalTableProperty);
            tableDefinition = R2RMLMappings.CreateBlankNode();

            R2RMLMappings.Assert(tripleMap, type, tripleMapClass);
            R2RMLMappings.Assert(tripleMap, logicalTable, tableDefinition);
        }

        /// <summary>
        /// <see cref="ITriplesMapConfiguration.SubjectMap"/>
        /// </summary>
        public ISubjectMapConfiguration SubjectMap
        {
            get
            {
                AssertTriplesMapInitialized();

                if (_subjectMapConfiguration == null)
                    _subjectMapConfiguration= new SubjectMapConfiguration(R2RMLMappings.GetUriNode(Uri), R2RMLMappings);

                return _subjectMapConfiguration;
            }
        }

        /// <summary>
        /// <see cref="ITriplesMapConfiguration.CreatePropertyObjectMap"/>
        /// </summary>
        public IPredicateObjectMapConfiguration CreatePropertyObjectMap()
        {
            AssertTriplesMapInitialized();

            var propertyObjectMap = new PredicateObjectMapConfiguration(R2RMLMappings.GetUriNode(Uri), R2RMLMappings);
            _propertyObjectMaps.Add(propertyObjectMap);
            return propertyObjectMap;
        }

        #endregion

        #region Implementation of ITriplesMapFromR2RMLViewConfiguration

        /// <summary>
        /// Asserts this Triples Map's SQL query as a query of type defined by <paramref name="uri"/> parmeter
        /// </summary>
        /// <param name="uri">Usually on of the URIs listed on http://www.w3.org/2001/sw/wiki/RDB2RDF/SQL_Version_IRIs </param>
        public ITriplesMapFromR2RMLViewConfiguration SetSqlVersion(Uri uri)
        {
            if (TableName != null)
                throw new InvalidTriplesMapException("Cannot set SQL version to a table-based logical table", Uri);

            R2RMLMappings.Assert(LogicalTableNode, R2RMLMappings.CreateUriNode(R2RMLUris.RrSqlVersionProperty), R2RMLMappings.CreateUriNode(uri));

            return this;
        }

        /// <summary>
        /// <see cref="SetSqlVersion(System.Uri)"/>
        /// </summary>
        public ITriplesMapFromR2RMLViewConfiguration SetSqlVersion(string uri)
        {
            return this.SetSqlVersion(new Uri(uri));
        }

        /// <summary>
        /// <see cref="ITriplesMapFromR2RMLViewConfiguration.SqlVersions"/>
        /// </summary>
        public Uri[] SqlVersions
        {
            get
            {
                IBlankNode logicalTableNode = LogicalTableNode;

                if (logicalTableNode == null)
                    return new Uri[0];

                var triples = R2RMLMappings.GetTriplesWithSubjectPredicate(logicalTableNode, R2RMLMappings.CreateUriNode(R2RMLUris.RrSqlVersionProperty));
                return triples.Select(triple => ((IUriNode)triple.Object).Uri).ToArray();
            }
        }

        #endregion

        IBlankNode LogicalTableNode
        {
            get
            {
                if (Uri == null)
                    throw new Exception("No TriplesMap URI!");

                var logicalTables = R2RMLMappings.GetTriplesWithSubjectPredicate(
                    R2RMLMappings.CreateUriNode(Uri),
                    R2RMLMappings.CreateUriNode(R2RMLUris.RrLogicalTableProperty)
                    ).ToArray();

                if (logicalTables.Count() > 1)
                    throw new InvalidTriplesMapException("Triples Map contains multiple logical tables!", Uri);

                return logicalTables.First().Object as IBlankNode;
            }
        }

        void AssertTriplesMapInitialized()
        {
            if (Uri == null)
                throw new InvalidOperationException("Triples map hasn't been initialized yet. Please set the TableName or SqlQuery property");
        }

        #region Overrides of BaseConfiguration

        protected internal override void RecursiveInitializeSubMapsFromCurrentGraph(INode currentNode = null)
        {
            var uriNode = currentNode as IUriNode;
            if (uriNode != null)
                _triplesMapUri = (uriNode).Uri.ToString();

            base.RecursiveInitializeSubMapsFromCurrentGraph(currentNode);
        }

        protected override void InitializeSubMapsFromCurrentGraph()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}