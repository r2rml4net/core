﻿using System;

namespace TCode.r2rml4net.RDF
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
        /// <exception cref="InvalidTriplesMapException"></exception>
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
        /// <exception cref="InvalidTriplesMapException"></exception>
        string ColumnName { get; }
        /// <summary>
        /// Gets the inverse expression associated with this <see cref="ITermMap"/> or null if not set
        /// </summary>
        /// <exception cref="InvalidTriplesMapException"></exception>
        /// <remarks>See http://www.w3.org/TR/r2rml/#inverse</remarks>
        string InverseExpression { get; }
    }
}