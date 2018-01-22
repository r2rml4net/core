﻿#region Licence
// Copyright (C) 2012-2018 Tomasz Pluskiewicz
// http://r2rml.net/
// r2rml@t-code.pl
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

using NullGuard;
using TCode.r2rml4net.Exceptions;
using TCode.r2rml4net.RDF;
using VDS.RDF;

namespace TCode.r2rml4net.Mapping.Fluent
{
    [NullGuard(ValidationFlags.All)]
    internal class PredicateMapConfiguration : TermMapConfiguration, INonLiteralTermMapConfigutarion, IPredicateMap
    {
        internal PredicateMapConfiguration(ITriplesMapConfiguration parentTriplesMap, IMapBase parentMap, IGraph r2RMLMappings)
            : this(parentTriplesMap, parentMap, r2RMLMappings, r2RMLMappings.CreateBlankNode())
        {
        }

        internal PredicateMapConfiguration(ITriplesMapConfiguration parentTriplesMap, IMapBase parentMap, IGraph r2RMLMappings, INode node)
            : base(parentTriplesMap, parentMap, r2RMLMappings, node)
        {
        }

        public System.Uri URI
        {
            [return: AllowNull]
            get { return ConstantValue; }
        }

        public override ITermMapConfiguration IsBlankNode()
        {
            throw new InvalidMapException("Only obj map and subject map can be of term type rr:BlankNode");
        }

        protected internal override IUriNode CreateMapPropertyNode()
        {
            return R2RMLMappings.CreateUriNode(R2RMLUris.RrPredicateMapPropety);
        }

        protected internal override IUriNode CreateShortcutPropertyNode()
        {
            return R2RMLMappings.CreateUriNode(R2RMLUris.RrPredicatePropety);
        }

        protected override void InitializeSubMapsFromCurrentGraph()
        {
            // predicate maps have no submaps
        }
    }
}