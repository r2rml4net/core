﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

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
        public string Name { get; internal set; }
        /// <summary>
        /// Column <see cref="DbType"/>
        /// </summary>
        public DbType Type { get; internal set; }
        /// <summary>
        /// The <see cref="TableMetadata" />, which contains this column
        /// </summary>
        public TableMetadata Table { get; internal set; }

        /// <summary>
        /// Implementation of <see cref="IVistitable{T}"/>. Only visits this
        /// </summary>
        public void Accept(IDatabaseMetadataVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
