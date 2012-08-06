﻿using System;
using System.Collections.Generic;
using System.Globalization;
using VDS.RDF;

namespace TCode.r2rml4net.Mapping.Tests.Mocks
{
    internal class MockConfiguration :
        ITriplesMapFromR2RMLViewConfiguration,
        ITriplesMapConfiguration, 
        ISubjectMapConfiguration, 
        IPredicateObjectMapConfiguration, 
        IGraphMap, 
        ITermTypeConfiguration, 
        ITermType, 
        IObjectMapConfiguration,
        ILiteralTermMapConfiguration,
        ITermMapConfiguration
    {
        public MockConfiguration(Uri baseURI, IR2RMLConfiguration r2RMLConfiguration)
        {
            R2RMLConfiguration = r2RMLConfiguration;
            BaseURI = baseURI;
        }

        #region Implementation of IMapBase

        /// <summary>
        /// The node representing this <see cref="IMapBase"/>
        /// </summary>
        public INode Node
        {
            get { return null; }
        }

        public Uri BaseURI { get; private set; }

        /// <summary>
        /// Creates an attached <see cref="IGraphMap"/>
        /// </summary>
        public IGraphMap CreateGraphMap()
        {
            return this;
        }

        #endregion

        #region Implementation of IQueryMap

        /// <summary>
        /// Gets the effective sql query of a <see cref="ITriplesMap"/> or a <see cref="IRefObjectMap"/>
        /// </summary>
        public string EffectiveSqlQuery
        {
            get { return string.Empty; }
        }

        #endregion

        #region Implementation of ITriplesMap

        /// <summary>
        /// Gets predicate-object maps associated with this <see cref="ITriplesMap"/>
        /// </summary>
        public IEnumerable<IPredicateObjectMap> PredicateObjectMaps
        {
            get { return this.AsEnumerable(); }
        }

        /// <summary>
        /// Adds a subject map subgraph to the mapping graph or returns existing if already created. Subject maps are used to construct subjects
        /// for triples procduced once mapping is applied to relational data
        /// <remarks>Triples map must contain exactly one subject map</remarks>
        /// </summary>
        public ISubjectMapConfiguration SubjectMap
        {
            get { return this; }
        }

        /// <summary>
        /// Adds a property-object map, which is used together with subject map to create complete triples\r\n
        /// from logical rows as described on http://www.w3.org/TR/r2rml/#predicate-object-map
        /// <remarks>Triples map can contain many property-object maps</remarks>
        /// </summary>
        public IPredicateObjectMapConfiguration CreatePropertyObjectMap()
        {
            return this;
        }

        /// <summary>
        /// The <see cref="IR2RMLConfiguration"/> containing this <see cref="ITriplesMapConfiguration"/>
        /// </summary>
        public IR2RMLConfiguration R2RMLConfiguration { get; private set; }

        /// <summary>
        /// <see cref="ITriplesMapConfiguration.Uri"/> of the triples map represented by this instance
        /// </summary>
        public Uri Uri
        {
            get { return null; }
        }

        /// <summary>
        /// Gets the subject map associated with this <see cref="ITriplesMap"/>
        /// </summary>
        ISubjectMap ITriplesMap.SubjectMap
        {
            get { return SubjectMap; }
        }

        /// <summary>
        /// Name of the table view which is source for triples
        /// </summary>
        /// <remarks>See http://www.w3.org/TR/r2rml/#physical-tables</remarks>
        public string TableName
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Query, which will be used as source for triples
        /// </summary>
        /// <remarks>See http://www.w3.org/TR/r2rml/#r2rml-views</remarks>
        public string SqlQuery
        {
            get { return string.Empty; }
        }

        #endregion

        #region Implementation of ITermMap

        /// <summary>
        /// Gets template or null if absent
        /// </summary>
        /// <exception cref="InvalidTriplesMapException"></exception>
        public string Template
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Returns term type set with configuration
        /// or a default value
        /// </summary>
        /// <remarks>Default value is described on http://www.w3.org/TR/r2rml/#dfn-term-type</remarks>
        public Uri TermTypeURI
        {
            get { return null; }
        }

        /// <summary>
        /// Gets column or null if not set
        /// </summary>
        /// <exception cref="InvalidTriplesMapException"></exception>
        public string ColumnName
        {
            get { return null; }
        }

        /// <summary>
        /// Gets the inverse expression associated with this <see cref="ITermMap"/> or null if not set
        /// </summary>
        /// <exception cref="InvalidTriplesMapException"></exception>
        /// <remarks>See http://www.w3.org/TR/r2rml/#inverse</remarks>
        public string InverseExpression
        {
            get { return string.Empty; }
        }

        public bool IsConstantValued
        {
            get { return true; }
        }

        public bool IsColumnValued
        {
            get { return true; }
        }

        public bool IsTemplateValued
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Allows setting the term type of the current term map
        /// </summary>
        /// <remarks>Please see <see cref="ITermTypeConfiguration"/> members for info on valid values for different term maps</remarks>
        ITermTypeConfiguration ITermMapConfiguration.TermType
        {
            get { return this; }
        }

        /// <summary>
        /// Sets the term map as constant-valued
        /// </summary>
        /// <param name="uri">Constant value URI</param>
        /// <remarks>
        /// Asserted using the rr:constant property described on http://www.w3.org/TR/r2rml/#constant
        /// </remarks>
        ITermTypeConfiguration ITermMapConfiguration.IsConstantValued(Uri uri)
        {
            return this;
        }

        /// <summary>
        /// Sets the term map as template-values
        /// </summary>
        /// <param name="template">A valid template</param>
        /// <remarks>
        /// For details on templates read more on http://www.w3.org/TR/r2rml/#from-template
        /// </remarks>
        ITermTypeConfiguration ITermMapConfiguration.IsTemplateValued(string template)
        {
            return this;
        }

        /// <summary>
        /// Sets the term map's inverse expression template
        /// </summary>
        /// <remarks>See http://www.w3.org/TR/r2rml/#inverse</remarks>
        public ITermMapConfiguration SetInverseExpression(string stringTemplate)
        {
            throw new NotImplementedException();
        }

        ITermType ITermMap.TermType
        {
            get
            {
                return this;
            }
        }

        #endregion

        #region Implementation of IUriValuedTermMap

        /// <summary>
        /// Get the GraphUri URI or null if no URI has been set
        /// </summary>
        public Uri URI
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets constant object literal value or null if absent
        /// </summary>
        /// <exception cref="InvalidTriplesMapException"></exception>
        string IObjectMap.Literal
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region Implementation of ISubjectMap

        /// <summary>
        /// Gets object maps associated with this <see cref="IPredicateObjectMap"/>
        /// </summary>
        public IEnumerable<IObjectMap> ObjectMaps
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets ref object maps associated with this <see cref="IPredicateObjectMap"/>
        /// </summary>
        public IEnumerable<IRefObjectMap> RefObjectMaps
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets predicate maps associated with this <see cref="IPredicateObjectMap"/>
        /// </summary>
        public IEnumerable<IPredicateMap> PredicateMaps
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets graph maps associated with this <see cref="IPredicateObjectMap"/>
        /// </summary>
        IEnumerable<IGraphMap> IPredicateObjectMap.GraphMaps
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets the graph maps associated with this <see cref="ISubjectMap"/>
        /// </summary>
        IEnumerable<IGraphMap> ISubjectMap.GraphMaps
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// All classes added to this <see cref="ISubjectMap"/>
        /// </summary>
        public Uri[] Classes
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Adds a class definition to subject map. A subject map can have many classes which will be used to construct
        /// triples for each RDF term as described on http://www.w3.org/TR/r2rml/#typing
        /// </summary>
        public ISubjectMapConfiguration AddClass(Uri classIri)
        {
            return this;
        }

        #endregion

        #region Implementation of IPredicateObjectMapConfiguration

        /// <summary>
        /// Creates a new object map
        /// </summary>
        /// <remarks><see cref="IPredicateObjectMapConfiguration"/> can have many object maps</remarks>
        public IObjectMapConfiguration CreateObjectMap()
        {
            return this;
        }

        /// <summary>
        /// Creates a new predicate map
        /// </summary>
        /// <remarks><see cref="IPredicateObjectMapConfiguration"/> can have many predicate maps</remarks>
        public ITermMapConfiguration CreatePredicateMap()
        {
            return this;
        }

        /// <summary>
        /// Creates a new ref object map
        /// </summary>
        /// <remarks><see cref="IPredicateObjectMapConfiguration"/> can have many ref object maps</remarks>
        public IRefObjectMapConfiguration CreateRefObjectMap(ITriplesMapConfiguration referencedTriplesMap)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Implementation of ITriplesMapFromR2RMLViewConfiguration

        /// <summary>
        /// Sets the sql query to be conformat with a specific SQL language specification
        /// </summary>
        /// <param name="uri">Usually on of the URIs listed on http://www.w3.org/2001/sw/wiki/RDB2RDF/SQL_Version_IRIs </param>
        public ITriplesMapFromR2RMLViewConfiguration SetSqlVersion(Uri uri)
        {
            return this;
        }

        /// <summary>
        /// Sets the sql query to be conformat with a specific SQL language specification
        /// </summary>
        /// <param name="uri">String representation of the sql version URI. Usually on of the URIs listed on http://www.w3.org/2001/sw/wiki/RDB2RDF/SQL_Version_IRIs </param>
        public ITriplesMapFromR2RMLViewConfiguration SetSqlVersion(string uri)
        {
            return this;
        }

        /// <summary>
        /// Gets the URIs of SQL versions set for the logical table
        /// </summary>
        public Uri[] SqlVersions
        {
            get { return new Uri[0]; }
        }

        #endregion

        #region Implementation of ITermTypeConfiguration

        /// <summary>
        /// Sets Term Map's term type to blank node. Throws an exception if term map is a graph map or a predicate map
        /// </summary>
        public ITermMapConfiguration IsBlankNode()
        {
            return this;
        }

        /// <summary>
        /// Sets Term Map's term type to uri node. Throws an exception if term map is a graph map or a predicate map
        /// </summary>
        public ITermMapConfiguration IsIRI()
        {
            return this;
        }

        /// <summary>
        /// Sets Term Map's term type to literal node.
        /// </summary>
        /// <remarks>Throws an exception if term map is not an object map</remarks>
        public ITermMapConfiguration IsLiteral()
        {
            return this;
        }

        #endregion

        #region Implementation of ITermType

        public bool IsURI
        {
            get { throw new NotImplementedException(); }
        }

        bool ITermType.IsBlankNode
        {
            get { throw new NotImplementedException(); }
        }

        bool ITermType.IsLiteral
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region Implementation of ILiteralTermMap

        public Uri DataTypeURI
        {
            get { throw new NotImplementedException(); }
        }

        public string LanguageTag
        {
            get { throw new NotImplementedException(); }
        }

        string ILiteralTermMap.Literal
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region Implementation of IObjectMapConfiguration

        /// <summary>
        /// Sets the object map as constant-valued
        /// </summary>
        /// <param name="literal">Constant value literal</param>
        /// <remarks>
        /// Asserted using the rr:constant property as described on http://www.w3.org/TR/r2rml/#constant
        /// </remarks>
        ILiteralTermMapConfiguration IObjectMapConfiguration.IsConstantValued(string literal)
        {
            return this;
        }

        /// <summary>
        /// Sets the object map as column-valued
        /// </summary>
        /// <param name="columnName">Column name</param>
        ILiteralTermMapConfiguration IObjectMapConfiguration.IsColumnValued(string columnName)
        {
            return this;
        }

        #endregion

        #region Implementation of ILiteralTermMapConfiguration

        /// <summary>
        /// Sets the datatype of the term map
        /// </summary>
        /// <remarks>language tag and datatype cannot be set simultaneously</remarks>
        public void HasDataType(Uri dataTypeUri)
        {
        }

        /// <summary>
        /// Sets the datatype of the term map
        /// </summary>
        /// <remarks>language tag and datatype cannot be set simultaneously</remarks>
        public void HasDataType(string dataTypeUri)
        {
        }

        /// <summary>
        /// Sets the language tag of the term map
        /// </summary>
        /// <remarks>language tag and datatype cannot be set simultaneously</remarks>
        public void HasLanguageTag(string languagTag)
        {
        }

        /// <summary>
        /// Sets the language tag of the term map
        /// </summary>
        /// <remarks>language tag and datatype cannot be set simultaneously</remarks>
        public void HasLanguageTag(CultureInfo cultureInfo)
        {
        }

        #endregion
    }
}