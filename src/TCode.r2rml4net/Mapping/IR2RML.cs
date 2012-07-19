﻿using System.Collections.Generic;
using TCode.r2rml4net.RDB;

namespace TCode.r2rml4net.Mapping
{
    /// <summary>
    /// Represents a R2RML mapping
    /// </summary>
    /// <remarks>See http://www.w3.org/TR/r2rml/</remarks>
    public interface IR2RML
    {
        /// <summary>
        /// Gets triple maps contained by this <see cref="IR2RML"/>
        /// </summary>
        IEnumerable<ITriplesMap> TriplesMaps { get; }
        /// <summary>
        /// Gets or sets the <see cref="IEffectiveSqlBuilder"/>
        /// </summary>
        IEffectiveSqlBuilder EffectiveSqlBuilder { get; set; }
    }
}