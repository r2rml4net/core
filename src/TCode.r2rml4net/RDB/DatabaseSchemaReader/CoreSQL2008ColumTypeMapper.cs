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
using DatabaseSchemaReader.DataSchema;

namespace TCode.r2rml4net.RDB.DatabaseSchemaReader
{
    // todo: to be removed?
    /// <summary>
    /// Implementation of <see cref="IColumnTypeMapper"/> which maps Core SQL:2008 datatypes
    /// </summary>
    public class CoreSQL2008ColumTypeMapper : IColumnTypeMapper
    {
        #region Implementation of IColumnTypeMapper

        /// <summary>
        /// Gets a member of <see cref="R2RMLType"/> enumeration for a given Core SQL:2008 <see cref="DataType"/>
        /// </summary>
        /// <remarks>Expects db typename to be one of the values described on http://www.w3.org/TR/r2rml/#natural-mapping</remarks>
        public R2RMLType GetColumnTypeFromColumn(DataType dataType)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}