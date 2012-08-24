﻿using System;
using TCode.r2rml4net.Exceptions;

namespace TCode.r2rml4net.Mapping
{
    /// <summary>
    /// Represents a term map
    /// </summary>
    /// <remarks>Read more on http://www.w3.org/TR/r2rml/#dfn-term-map</remarks>
    public interface ITermMap : IMapBase
    {
        /// <summary>
        /// Gets template or null if absent
        /// </summary>
        /// <exception cref="InvalidMapException"></exception>
        string Template { get; }
        /// <summary>
        /// Returns term type set with configuration
        /// or a default value
        /// </summary>
        /// <remarks>Default value is described on http://www.w3.org/TR/r2rml/#dfn-term-type</remarks>
        Uri TermTypeURI { get; }
        /// <summary>
        /// Gets column or null if not set
        /// </summary>
        /// <exception cref="InvalidMapException"></exception>
        string ColumnName { get; }
        /// <summary>
        /// Gets the inverse expression associated with this <see cref="ITermMap"/> or null if not set
        /// </summary>
        /// <exception cref="InvalidMapException"></exception>
        /// <remarks>See http://www.w3.org/TR/r2rml/#inverse</remarks>
        string InverseExpression { get; }

        bool IsConstantValued { get; }
        bool IsColumnValued { get; }
        bool IsTemplateValued { get; }

        ITermType TermType { get; }
    }
}