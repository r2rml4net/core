﻿using System.Collections.Generic;
using System.Data;
using TCode.r2rml4net.Mapping;
using VDS.RDF;

namespace TCode.r2rml4net.TriplesGeneration
{
    public interface IRefObjectMapProcessor
    {
        void ProcessRefObjectMap(IRefObjectMap refObjectMap, IDbConnection dbConnection, ISubjectMap subjectMap, IEnumerable<IGraphMap> predicateObjectMapGraphMaps);
    }
}