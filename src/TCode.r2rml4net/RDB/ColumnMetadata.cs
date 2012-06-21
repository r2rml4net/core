﻿using System;

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
        /// Gets the .NET column <see cref="Type"/> as mapped in ADO.NET
        /// </summary>
        public DbType Type { get; internal set; }
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
