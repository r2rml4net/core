﻿using System.Collections.Generic;
using System.Data;
using TCode.r2rml4net.Mapping;
using VDS.RDF;

namespace TCode.r2rml4net.TriplesGeneration
{
    public interface IPredicateObjectMapProcessor
    {
        void ProcessPredicateObjectMap(INode subject, IPredicateObjectMap predicateObjectMap, IEnumerable<IUriNode> subjectGraphs, IDataRecord logicalRow);
    }
}