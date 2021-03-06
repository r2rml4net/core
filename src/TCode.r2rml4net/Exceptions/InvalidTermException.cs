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
using System;
using TCode.r2rml4net.Mapping;

namespace TCode.r2rml4net.Exceptions
{
    /// <summary>
    /// Represents errors occuring during <a href="http://www.w3.org/TR/r2rml/#generated-rdf-term">RDF term generation</a>
    /// </summary>
    public class InvalidTermException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="InvalidTermException"/> for a <paramref name="termMap"/>
        /// with a given <paramref name="reason"/> why it occured
        /// </summary>
        public InvalidTermException(ITermMap termMap, string reason)
            : base(string.Format("Cannot generate RDF term for '{0}'. {1}", termMap.Node, reason))
        {
            TermMap = termMap;
        }

        /// <summary>
        /// Gets the <a href="http://www.w3.org/TR/r2rml/#term-map">term map</a>, 
        /// which cannot be used to <a href="http://www.w3.org/TR/r2rml/#generated-rdf-term">generate an RDF term</a>
        /// </summary>
        public ITermMap TermMap { get; private set; }
    }
}