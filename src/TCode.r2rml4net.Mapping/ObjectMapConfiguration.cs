using System;
using System.Globalization;
using System.Linq;
using VDS.RDF;

namespace TCode.r2rml4net.Mapping
{
    internal class ObjectMapConfiguration : TermMapConfiguration, IObjectMapConfiguration, ILiteralTermMapConfiguration
    {
        internal ObjectMapConfiguration(ITriplesMapConfiguration parentTriplesMap, IPredicateObjectMapConfiguration parentMap, IGraph r2RMLMappings)
            : base(parentTriplesMap, parentMap, r2RMLMappings)
        {
        }

        #region Implementation of IObjectMapConfiguration

        public ILiteralTermMapConfiguration IsConstantValued(string literal)
        {
            if(Literal != null)
                throw new InvalidTriplesMapException("Term map can have at most one constant value");

            EnsureRelationWithParentMap();

            R2RMLMappings.Assert(Node, R2RMLMappings.CreateUriNode(R2RMLUris.RrConstantProperty), R2RMLMappings.CreateLiteralNode(literal));

            return this;
        }

        ILiteralTermMapConfiguration IObjectMapConfiguration.IsColumnValued(string columnName)
        {
            IsColumnValued(columnName);
            return this;
        }

        #endregion

        #region Overrides of TermMapConfiguration

        protected internal override IUriNode CreateMapPropertyNode()
        {
            return R2RMLMappings.CreateUriNode(R2RMLUris.RrObjectMapProperty);
        }

        protected internal override IUriNode CreateShortcutPropertyNode()
        {
            return R2RMLMappings.CreateUriNode(R2RMLUris.RrObjectProperty);
        }

        /// <summary>
        /// Overriden, because object maps can be of term type rr:Literal
        /// </summary>
        public override Uri TermTypeURI
        {
            get
            {
                if (ExplicitTermType != null)
                    return ExplicitTermType;

                // term type is literal is column valued, or has datatype or language tag
                if (IsLiteralTermType)
                    return R2RMLMappings.CreateUriNode(R2RMLUris.RrLiteral).Uri;

                // in other cases is rr:IRI
                return R2RMLMappings.CreateUriNode(R2RMLUris.RrIRI).Uri;
            }
        }

        /// <summary>
        /// See http://www.w3.org/TR/r2rml/#termtype
        /// </summary>
        private bool IsLiteralTermType
        {
            get
            {
                var columnPropertyNode = R2RMLMappings.CreateUriNode(R2RMLUris.RrColumnProperty);
                var columnTriples = R2RMLMappings.GetTriplesWithSubjectPredicate(Node, columnPropertyNode).ToArray();

                if (columnTriples.Any())
                    return true;

                var languageTagNode = R2RMLMappings.CreateUriNode(R2RMLUris.RrLanguageTagPropety);
                var languageTagTriples = R2RMLMappings.GetTriplesWithSubjectPredicate(Node, languageTagNode).ToArray();

                var datatypeNode = R2RMLMappings.CreateUriNode(R2RMLUris.RrDatatypePropety);
                var datatypeTriples = R2RMLMappings.GetTriplesWithSubjectPredicate(Node, datatypeNode).ToArray();

                if (languageTagTriples.Any() && datatypeTriples.Any())
                    throw new InvalidTriplesMapException("Object map cannot have both a rr:languageTag and rr:datatype properties set");

                return datatypeTriples.Any() || languageTagTriples.Any();
            }
        }

        public override ITermMapConfiguration IsLiteral()
        {
            AssertTermTypeNotSet();
            EnsureRelationWithParentMap();

            R2RMLMappings.Assert(Node, R2RMLMappings.CreateUriNode(R2RMLUris.RrTermTypeProperty), R2RMLMappings.CreateUriNode(R2RMLUris.RrLiteral));
            return this;
        }

        #endregion

        #region Implementation of ILiteralTermMapConfiguration

        public void HasDataType(string dataTypeUri)
        {
            HasDataType(new Uri(dataTypeUri));
        }

        public void HasDataType(Uri dataTypeUri)
        {
            EnsureOnlyLanguageTagOrDatatype();

            R2RMLMappings.Assert(Node, R2RMLMappings.CreateUriNode(R2RMLUris.RrDatatypePropety), R2RMLMappings.CreateUriNode(dataTypeUri));
        }

        public void HasLanguageTag(string languagTag)
        {
            EnsureOnlyLanguageTagOrDatatype();

            R2RMLMappings.Assert(Node, R2RMLMappings.CreateUriNode(R2RMLUris.RrLanguageTagPropety), R2RMLMappings.CreateLiteralNode(languagTag.ToLower()));
        }

        public void HasLanguageTag(CultureInfo cultureInfo)
        {
            HasLanguageTag(cultureInfo.Name);
        }

        private void EnsureOnlyLanguageTagOrDatatype()
        {
            var datatypeTriples = R2RMLMappings.GetTriplesWithSubjectPredicate(Node, R2RMLMappings.CreateUriNode(R2RMLUris.RrDatatypePropety));
            var languageTagTriples = R2RMLMappings.GetTriplesWithSubjectPredicate(Node, R2RMLMappings.CreateUriNode(R2RMLUris.RrLanguageTagPropety));

            if (datatypeTriples.Any())
                throw new InvalidTriplesMapException("Object map already has a datatype");
            if (languageTagTriples.Any())
                throw new InvalidTriplesMapException("Object map already has a language tag");
        }

        #endregion

        #region Implementation of IObjectMap

        public Uri URI
        {
            get { return ConstantValue; }
        }

        public string Literal
        {
            get { return GetSingleLiteralValueForPredicate(R2RMLMappings.CreateUriNode(R2RMLUris.RrConstantProperty)); }
        }

        #endregion

        #region Overrides of BaseConfiguration

        protected override void InitializeSubMapsFromCurrentGraph()
        {
            // object map contains no submaps
        }

        #endregion
    }
}