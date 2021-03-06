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
namespace TCode.r2rml4net.RDB
{
    /// <summary>
    /// Enumeration of types valid for mapping to RDF
    /// </summary>
    /// <remarks>Refer to each enum member for corresponding Core SQL 2008 types. 
    /// Note that for each non-standard RDBMS implementation there may be other types or they may have different meaning</remarks>
    public enum R2RMLType
    {
        /// <summary>
        /// An undefined SQL type. Values will be interpreted as string literals
        /// </summary>
        Undefined,

        /// <summary>
        /// Any string type 
        /// </summary>
        /// <remarks>In Core SQL 2008: CHARACTER, CHARACTER VARYING, CHARACTER LARGE OBJECT, 
        /// NATIONAL CHARACTER, NATIONAL CHARACTER VARYING, NATIONAL CHARACTER LARGE OBJECT</remarks>
        String,

        /// <summary>
        /// A binary type
        /// </summary>
        /// <remarks>In Core SQL 2008: BINARY, BINARY VARYING, BINARY LARGE OBJECT</remarks>
        Binary,

        /// <summary>
        /// A floating point type
        /// </summary>
        /// <remarks>In Core SQL 2008: FLOAT, REAL, DOUBLE PRECISION</remarks>
        FloatingPoint,

        /// <summary>
        /// An integer type
        /// </summary>
        /// <remarks>In Core SQL 2008: SMALLINT, INTEGER, BIGINT</remarks>
        Integer,

        /// <summary>
        /// A boolean type
        /// </summary>
        /// <remarks>In Core SQL 2008: BOOLEAN</remarks>
        Boolean,

        /// <summary>
        /// A date type
        /// </summary>
        /// <remarks>In Core SQL 2008: DATE</remarks>
        Date,

        /// <summary>
        /// A time type
        /// </summary>
        /// <remarks>In Core SQL 2008: TIME</remarks>
        Time,

        /// <summary>
        /// A datetime type
        /// </summary>
        /// <remarks>In Core SQL 2008: DATETIME</remarks>
        DateTime,

        /// <summary>
        /// A decimal type
        /// </summary>
        /// <remarks>In Core SQL 2008: NUMERIC, DECIMAL</remarks>
        Decimal
    }
}