﻿using System;
using TCode.r2rml4net.RDB;

namespace TCode.r2rml4net.RDF
{
    /// <summary>
    /// Types used by R2RML mappings as described on http://www.w3.org/TR/r2rml/#natural-mapping
    /// </summary>
    public class XsdDatatypes
    {
        /// <summary>
        /// Gets full URI string for xsd:integer
        /// </summary>
        public const string Integer = "http://www.w3.org/2001/XMLSchema#integer";

        /// <summary>
        /// http://www.w3.org/2001/XMLSchema#boolean
        /// </summary>
        public const string Boolean = "http://www.w3.org/2001/XMLSchema#boolean";

        /// <summary>
        /// http://www.w3.org/2001/XMLSchema#decimal
        /// </summary>
        public const string Decimal = "http://www.w3.org/2001/XMLSchema#decimal";

        /// <summary>
        /// http://www.w3.org/2001/XMLSchema#double
        /// </summary>
        public const string Double = "http://www.w3.org/2001/XMLSchema#double";

        /// <summary>
        /// http://www.w3.org/2001/XMLSchema#date
        /// </summary>
        public const string Date = "http://www.w3.org/2001/XMLSchema#date";

        /// <summary>
        /// http://www.w3.org/2001/XMLSchema#time
        /// </summary>
        public const string Time = "http://www.w3.org/2001/XMLSchema#time";

        /// <summary>
        /// http://www.w3.org/2001/XMLSchema#dateTime
        /// </summary>
        public const string DateTime = "http://www.w3.org/2001/XMLSchema#dateTime";

        /// <summary>
        /// http://www.w3.org/2001/XMLSchema#hexBinary
        /// </summary>
        public const string Binary = "http://www.w3.org/2001/XMLSchema#hexBinary";

        /// <summary>
        /// Get a xsd type URI for the given <see cref="R2RMLType"/>
        /// </summary>
        /// <returns>a URI or null for string/undefined type</returns>
        /// <remarks>Read more on http://www.w3.org/TR/r2rml/#natural-mapping</remarks>
        public static Uri GetDataType(R2RMLType columnType)
        {
            switch (columnType)
            {
                case R2RMLType.Binary:
                    return new Uri(Binary);
                case R2RMLType.Integer:
                    return new Uri(Integer);
                case R2RMLType.Decimal:
                    return new Uri(Decimal);
                case R2RMLType.FloatingPoint:
                    return new Uri(Double);
                case R2RMLType.Date:
                    return new Uri(Date);
                case R2RMLType.Time:
                    return new Uri(Time);
                case R2RMLType.DateTime:
                    return new Uri(DateTime);
                case R2RMLType.Boolean:
                    return new Uri(Boolean);
                default:
                    return null;
            }
        }
    }
}
