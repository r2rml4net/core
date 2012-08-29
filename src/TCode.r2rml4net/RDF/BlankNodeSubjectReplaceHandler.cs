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

using System.Collections.Generic;
using VDS.Common;
using VDS.RDF;
using VDS.RDF.Parsing.Handlers;

namespace TCode.r2rml4net.RDF
{
    internal class BlankNodeSubjectReplaceHandler : BaseRdfHandler
    {
        private readonly IRdfHandler _wrapped;
        private readonly IDictionary<string, IBlankNode> _replacedNodes = new HashTable<string, IBlankNode>();

        public BlankNodeSubjectReplaceHandler(IRdfHandler wrapped)
        {
            _wrapped = wrapped;
        }

        #region Overrides of BaseRdfHandler

        /// <summary>
        /// Must be overridden by derived handlers to take appropriate Triple handling action
        /// </summary>
        /// <param name="t">Triple</param>
        /// <returns/>
        protected override bool HandleTripleInternal(Triple t)
        {
            IBlankNode subject = t.Subject as IBlankNode;
            IBlankNode @object = t.Object as IBlankNode;
            Triple toHandle = t;

            IBlankNode replacedSubject = null, replacedObject = null;
            if (subject != null)
            {
                if (!_replacedNodes.ContainsKey(subject.InternalID))
                {
                    _replacedNodes.Add(subject.InternalID, _wrapped.CreateBlankNode());
                }
                replacedSubject = _replacedNodes[subject.InternalID];
            }
            if (@object !=null)
            {
                if (!_replacedNodes.ContainsKey(@object.InternalID))
                {
                    _replacedNodes.Add(@object.InternalID, _wrapped.CreateBlankNode());
                }
                replacedObject = _replacedNodes[@object.InternalID];
            }

            if (replacedSubject != null || replacedObject != null)
                toHandle = t.CloneTriple(replacedSubject: replacedSubject, replacedObject: replacedObject);

            return _wrapped.HandleTriple(toHandle);
        }

        /// <summary>
        /// Gets whether the Handler will accept all Triples i.e. it will never abort handling early
        /// </summary>
        public override bool AcceptsAll
        {
            get { return true; }
        }

        protected override void StartRdfInternal()
        {
            _wrapped.StartRdf();
            base.StartRdfInternal();
        }

        protected override void EndRdfInternal(bool ok)
        {
            _wrapped.EndRdf(ok);
            base.EndRdfInternal(ok);
        }

        #endregion
    }
}