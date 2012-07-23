﻿using TCode.r2rml4net.Mapping;

namespace TCode.r2rml4net.TriplesGeneration
{
    /// <summary>
    /// Interface for generating triples from R2RML mappings
    /// </summary>
    public interface IR2RMLProcessor
    {
        /// <summary>
        /// Generates triples from <paramref name="r2RML"/> mappings
        /// </summary>
        void GenerateTriples(IR2RML r2RML);
    }
}