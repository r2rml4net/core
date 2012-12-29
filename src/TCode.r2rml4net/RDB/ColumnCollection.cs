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
using System.Collections.Generic;
using System.Linq;

namespace TCode.r2rml4net.RDB
{
    /// <summary>
    /// Represents a collection of columns
    /// </summary>
    public class ColumnCollection : IEnumerable<ColumnMetadata>
    {
        private readonly IList<ColumnMetadata> _columns = new List<ColumnMetadata>();

        /// <summary>
        /// Gets the table's columns count
        /// </summary>
        public int ColumnsCount
        {
            get
            {
                return _columns.Count;
            }
        }

        /// <summary>
        /// Implementation of <see cref="IEnumerable{T}"/>
        /// </summary>
        public IEnumerator<ColumnMetadata> GetEnumerator()
        {
            return _columns.GetEnumerator();
        }

        /// <summary>
        /// Implementation of <see cref="IEnumerable{T}"/>
        /// </summary>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _columns.GetEnumerator();
        }

        /// <summary>
        /// Gets the column with the specified name
        /// </summary>
        /// <exception cref="IndexOutOfRangeException" />
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="ArgumentOutOfRangeException" />
        public ColumnMetadata this[string columnName]
        {
            get
            {
                if (columnName == null)
                    throw new ArgumentNullException("columnName");
                if (string.IsNullOrWhiteSpace(columnName))
                    throw new ArgumentOutOfRangeException("columnName");

                var column = this.SingleOrDefault(c => c.Name == columnName);
                if (column == null)
                    throw new IndexOutOfRangeException(string.Format("Table does not contain column {0}", columnName));

                return column;
            }
        }

        /// <summary>
        /// Implemented to allow collection initialization
        /// </summary>
        protected internal virtual void Add(ColumnMetadata column)
        {
            if (_columns.Any(col => col.Name == column.Name))
                throw new ArgumentException(string.Format("Collection already contains column {0}", column.Name));

            _columns.Add(column);
        }
    }
}