﻿#region Licence
// Copyright (C) 2012 Tomasz Pluskiewicz
// http://r2rml.net/
// r2rml@r2rml.net
// 	
// ------------------------------------------------------------------------
// 	
// This file is part of r2rml4net.
// 	
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal 
// in the Software without restriction, including without limitation the rights 
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
// copies of the Software, and to permit persons to whom the Software is 
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS 
// OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE 
// OR OTHER DEALINGS IN THE SOFTWARE.
// 	
// ------------------------------------------------------------------------
// 
// r2rml4net may alternatively be used under the LGPL licence
// 
// http://www.gnu.org/licenses/lgpl.html
// 
// If these licenses are not suitable for your intended use please contact
// us at the above stated email address to discuss alternative
// terms.
#endregion

using System;
using System.Globalization;
using System.Linq;
using TCode.r2rml4net.Exceptions;
using TCode.r2rml4net.Validation;
using VDS.RDF;

namespace TCode.r2rml4net.Mapping.Fluent
{
    internal class ObjectMapConfiguration : TermMapConfiguration, IObjectMapConfiguration, ILiteralTermMapConfiguration, ITermType
    {
        public ILanguageTagValidator LanguageTagValidator { get; set; }

        internal ObjectMapConfiguration(ITriplesMapConfiguration parentTriplesMap, IPredicateObjectMapConfiguration parentMap, IGraph r2RMLMappings)
            : this(parentTriplesMap, parentMap, r2RMLMappings, r2RMLMappings.CreateBlankNode())
        {
        }

        internal ObjectMapConfiguration(ITriplesMapConfiguration parentTriplesMap, IPredicateObjectMapConfiguration parentMap, IGraph r2RMLMappings, INode node)
            : base(parentTriplesMap, parentMap, r2RMLMappings, node)
        {
            LanguageTagValidator = new Bcp47RegexLanguageTagValidator();
        }

        #region Implementation of ITermMap

        bool ITermMap.IsConstantValued
        {
            get
            {
                return ConstantValue != null || !string.IsNullOrEmpty(Literal);
            }
        }

        #endregion

        #region Implementation of IObjectMapConfiguration

        public ILiteralTermMapConfiguration IsConstantValued(string literal)
        {
            if (Literal != null)
                throw new InvalidMapException("Term map can have at most one constant value");

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

                var languageNode = R2RMLMappings.CreateUriNode(R2RMLUris.RrLanguagePropety);
                var languageTriples = R2RMLMappings.GetTriplesWithSubjectPredicate(Node, languageNode).ToArray();

                var datatypeNode = R2RMLMappings.CreateUriNode(R2RMLUris.RrDatatypePropety);
                var datatypeTriples = R2RMLMappings.GetTriplesWithSubjectPredicate(Node, datatypeNode).ToArray();

                if (languageTriples.Any() && datatypeTriples.Any())
                    throw new InvalidMapException("Object map cannot have both a rr:language and rr:datatype properties set");

                return datatypeTriples.Any() || languageTriples.Any();
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
            EnsureOnlyLanguageOrDatatype();

            R2RMLMappings.Assert(Node, R2RMLMappings.CreateUriNode(R2RMLUris.RrDatatypePropety), R2RMLMappings.CreateUriNode(dataTypeUri));
        }

        public void HasLanguage(string languageTag)
        {
            EnsureOnlyLanguageOrDatatype();
            if(!LanguageTagValidator.LanguageTagIsValid(languageTag))
                throw new ArgumentException(string.Format("Language tag '{0}' is invalid", languageTag), languageTag);

            R2RMLMappings.Assert(Node, R2RMLMappings.CreateUriNode(R2RMLUris.RrLanguagePropety), R2RMLMappings.CreateLiteralNode(languageTag.ToLower()));
        }

        public void HasLanguage(CultureInfo cultureInfo)
        {
            HasLanguage(cultureInfo.Name);
        }

        private void EnsureOnlyLanguageOrDatatype()
        {
            var datatypeTriples = R2RMLMappings.GetTriplesWithSubjectPredicate(Node, R2RMLMappings.CreateUriNode(R2RMLUris.RrDatatypePropety));
            var languageTriples = R2RMLMappings.GetTriplesWithSubjectPredicate(Node, R2RMLMappings.CreateUriNode(R2RMLUris.RrLanguagePropety));

            if (datatypeTriples.Any())
                throw new InvalidMapException("Object map already has a datatype");
            if (languageTriples.Any())
                throw new InvalidMapException("Object map already has a language tag");
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

        #region Implementation of ILiteralTermMap

        public Uri DataTypeURI
        {
            get
            {
                var datatypeTriples = R2RMLMappings.GetTriplesWithSubjectPredicate(Node, R2RMLMappings.CreateUriNode(R2RMLUris.RrDatatypePropety));
                var languageTriples = R2RMLMappings.GetTriplesWithSubjectPredicate(Node, R2RMLMappings.CreateUriNode(R2RMLUris.RrLanguagePropety));

                var datatypeTriple = datatypeTriples.SingleOrDefault();
                if (datatypeTriple != null)
                {
                    if (languageTriples.Any())
                        throw new InvalidMapException("Object map has both language tag and datatype set");

                    IUriNode dataTypeUriNode = datatypeTriple.Object as IUriNode;
                    if (dataTypeUriNode == null)
                        throw new InvalidMapException("Object map has datatype set but it is not a URI");

                    return dataTypeUriNode.Uri;
                }

                return null;
            }
        }

        public string Language
        {
            get
            {
                var datatypeTriples = R2RMLMappings.GetTriplesWithSubjectPredicate(Node, R2RMLMappings.CreateUriNode(R2RMLUris.RrDatatypePropety));
                var languageTriples = R2RMLMappings.GetTriplesWithSubjectPredicate(Node, R2RMLMappings.CreateUriNode(R2RMLUris.RrLanguagePropety));

                var languagTriple = languageTriples.SingleOrDefault();
                if (languagTriple != null)
                {
                    if (datatypeTriples.Any())
                        throw new InvalidTermException(this, "Object map has both language tag and datatype set");

                    ILiteralNode languageNode = languagTriple.Object as ILiteralNode;
                    if (languageNode == null)
                        throw new InvalidTermException(this, "Object map has literal set but it is not a literal");

                    var languageTag = languageNode.Value;
                    if(!LanguageTagValidator.LanguageTagIsValid(languageTag))
                        throw new InvalidTermException(this, string.Format("Language tag '{0}' is invalid", languageTag));

                    return languageTag;
                }

                return null;
            }
        }

        #endregion
    }
}