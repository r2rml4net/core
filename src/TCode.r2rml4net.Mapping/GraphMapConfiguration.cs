﻿#region Licence
			
/* 
Copyright (C) 2012 Tomasz Pluskiewicz
http://r2rml.net/
r2rml@r2rml.net
	
------------------------------------------------------------------------
	
This file is part of r2rml4net.
	
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal 
in the Software without restriction, including without limitation the rights 
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
copies of the Software, and to permit persons to whom the Software is 
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all 
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS 
OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE 
OR OTHER DEALINGS IN THE SOFTWARE.
	
------------------------------------------------------------------------

r2rml4net may alternatively be used under the LGPL licence

http://www.gnu.org/licenses/lgpl.html

If these licenses are not suitable for your intended use please contact
us at the above stated email address to discuss alternative
terms. */
			
#endregion

using System;
using TCode.r2rml4net.Exceptions;
using VDS.RDF;

namespace TCode.r2rml4net.Mapping
{
    internal class GraphMapConfiguration : TermMapConfiguration, INonLiteralTermMapConfigutarion, IGraphMap
    {
        internal GraphMapConfiguration(ITriplesMapConfiguration parentTriplesMap, IGraphMapParent parentMap, IGraph r2RMLMappings, MappingOptions mappingOptions)
            : this(parentTriplesMap, parentMap, r2RMLMappings, r2RMLMappings.CreateBlankNode(), mappingOptions)
        {
        }

        internal GraphMapConfiguration(ITriplesMapConfiguration parentTriplesMap, IGraphMapParent parentMap, IGraph r2RMLMappings, INode node, MappingOptions mappingOptions) 
            : base(parentTriplesMap, parentMap, r2RMLMappings, node, mappingOptions)
        {
        }

        #region Overrides of TermMapConfiguration

        protected internal override IUriNode CreateMapPropertyNode()
        {
            return R2RMLMappings.CreateUriNode(R2RMLUris.RrGraphMapPropety);
        }

        protected internal override IUriNode CreateShortcutPropertyNode()
        {
            return R2RMLMappings.CreateUriNode(R2RMLUris.RrGraphPropety);
        }

        public override ITermMapConfiguration IsBlankNode()
        {
            throw new InvalidMapException("Only object map and subject map can be of term type rr:Literal");
        }

        #endregion

        #region Implementation of IGraphMap

        public Uri URI
        {
            get { return ConstantValue; }
        }

        #endregion

        #region Overrides of BaseConfiguration

        protected override void InitializeSubMapsFromCurrentGraph()
        {
            
        }

        #endregion
    }
}