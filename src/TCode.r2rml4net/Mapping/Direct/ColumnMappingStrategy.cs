﻿#region Licence
// Copyright (C) 2012-2014 Tomasz Pluskiewicz
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
using TCode.r2rml4net.RDB;

namespace TCode.r2rml4net.Mapping.Direct
{
    /// <summary>
    /// Default implementation of <see cref="IColumnMappingStrategy"/>,  which creates mapping graph
    /// consistent with the official <a href="www.w3.org/TR/rdb-direct-mapping/">Direct Mapping specfication</a>
    /// </summary>
    public class ColumnMappingStrategy : IColumnMappingStrategy
    {
        #region Implementation of IColumnMappingStrategy

        /// <summary>
        /// Creates a predicate URI for a <paramref name="column"/>
        /// </summary>
        /// <example>For table 'Student', column 'Last name' and base URI 'http://example.com/' it returns URI 'http://example.com/Student#Last%20name'</example>
        public virtual Uri CreatePredicateUri(Uri BaseUri, ColumnMetadata column)
        {
            if(BaseUri == null)
                throw new ArgumentNullException("BaseUri");
            if(column == null)
                throw new ArgumentNullException("column");
            if(string.IsNullOrWhiteSpace(column.Name))
                throw new ArgumentException("Column name invalid", "column");

            string predicateUriString = string.Format("{0}#{1}", column.Table.Name, column.Name);
            return new Uri(BaseUri, predicateUriString);
        }

        #endregion
    }
}