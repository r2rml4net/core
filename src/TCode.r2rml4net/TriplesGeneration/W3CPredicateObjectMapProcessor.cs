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
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TCode.r2rml4net.Mapping;
using VDS.RDF;

namespace TCode.r2rml4net.TriplesGeneration
{
    class W3CPredicateObjectMapProcessor : MapProcessorBase, IPredicateObjectMapProcessor
    {
        public W3CPredicateObjectMapProcessor(IRDFTermGenerator termGenerator)
            : base(termGenerator)
        {
        }

        #region Implementation of IPredicateObjectMapProcessor

        public void ProcessPredicateObjectMap(INode subject, IPredicateObjectMap predicateObjectMap, IEnumerable<IUriNode> subjectGraphs, IDataRecord logicalRow, IRdfHandler rdfHandler)
        {
            var predicates = (from predicateMap in predicateObjectMap.PredicateMaps
                              select TermGenerator.GenerateTerm<IUriNode>(predicateMap, logicalRow)).ToArray();
            var objects = (from objectMap in predicateObjectMap.ObjectMaps
                           select TermGenerator.GenerateTerm<INode>(objectMap, logicalRow)).ToArray();
            var graphs = (from graphMap in predicateObjectMap.GraphMaps
                          select TermGenerator.GenerateTerm<IUriNode>(graphMap, logicalRow)).ToArray();
            var subjectGraphsLocal = subjectGraphs.ToArray();

            AddTriplesToDataSet(subject, predicates, objects, graphs.Union(subjectGraphsLocal).ToList(), rdfHandler);
        }

        #endregion
    }
}