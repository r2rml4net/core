﻿#region Licence
			
/* 
Copyright (C) 2013 Tomasz Pluskiewicz
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
using System.Linq;
using VDS.RDF;

namespace TCode.r2rml4net.Configuration
{
    internal static class ConfigurationGraphExtensions
    {
        public static INode GetSingleTripleObject(this IGraph graph, INode objNode, string predicateUri)
        {
            return graph.GetTriples(objNode, predicateUri).Single().Object;
        }
        public static INode GetSingleOrDefaultTripleObject(this IGraph graph, INode objNode, string predicateUri)
        {
            var singleOrDefault = graph.GetTriples(objNode, predicateUri).SingleOrDefault();
            if (singleOrDefault != null)
            {
                return singleOrDefault.Object;
            }

            return null;
        }

        private static IEnumerable<Triple> GetTriples(this IGraph graph, INode objNode, string predicateUri)
        {
            var pred = graph.CreateUriNode(UriFactory.Create(predicateUri));
            return graph.GetTriplesWithSubjectPredicate(objNode, pred);
        }
    }
}