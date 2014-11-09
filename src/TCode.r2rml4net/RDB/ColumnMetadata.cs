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
using NullGuard;

namespace TCode.r2rml4net.RDB
{
    /// <summary>
    /// Represents a column from a relational database
    /// </summary>
    public class ColumnMetadata : IVistitable<IDatabaseMetadataVisitor>
    {
        /// <summary>
        /// Column name
        /// </summary>
        public string Name { [return: AllowNull] get; internal set; }
        /// <summary>
        /// Gets the .NET column <see cref="Type"/> as mapped in ADO.NET
        /// </summary>
        public R2RMLType Type { get; internal set; }
        /// <summary>
        /// Gets the native column type as declared in the source RDBMS
        /// </summary>
        public string NativeType { get; internal set; }
        /// <summary>
        /// The <see cref="TableMetadata" />, which contains this column
        /// </summary>
        public TableMetadata Table { get; internal set; }
        /// <summary>
        /// Returns true is this column is (part of) the primary key
        /// </summary>
        public bool IsPrimaryKey { get; internal set; }
        /// <summary>
        /// Implementation of <see cref="IVistitable{T}"/>. Only visits this
        /// </summary>
        public void Accept(IDatabaseMetadataVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
