using System;
using System.Linq;
using System.Collections.Generic;
using TCode.r2rml4net.RDF;
using VDS.RDF;

namespace TCode.r2rml4net.Mapping
{
    class PredicateObjectMapConfiguration : BaseConfiguration, IPredicateObjectMapConfiguration, IPredicateObjectMap
    {
        private INode _predicateObjectMapNode;
        private readonly IList<ObjectMapConfiguration> _objectMaps = new List<ObjectMapConfiguration>();
        private readonly IList<RefObjectMapConfiguration> _refObjectMaps = new List<RefObjectMapConfiguration>();
        private readonly IList<PredicateMapConfiguration> _predicateMaps = new List<PredicateMapConfiguration>();
        private readonly IList<GraphMapConfiguration> _graphMaps = new List<GraphMapConfiguration>();

        internal PredicateObjectMapConfiguration(INode triplesMapNode, IGraph r2RMLMappings)
            : base(r2RMLMappings)
        {
            _predicateObjectMapNode = R2RMLMappings.CreateBlankNode();
            R2RMLMappings.Assert(triplesMapNode, R2RMLMappings.CreateUriNode(R2RMLUris.RrPredicateObjectMapPropety), _predicateObjectMapNode);
        }

        #region Implementation of IPredicateObjectMapConfiguration

        public IObjectMapConfiguration CreateObjectMap()
        {
            var objectMap = new ObjectMapConfiguration(_predicateObjectMapNode, R2RMLMappings);
            _objectMaps.Add(objectMap);
            return objectMap;
        }

        public ITermMapConfiguration CreatePredicateMap()
        {
            var propertyMap = new PredicateMapConfiguration(_predicateObjectMapNode, R2RMLMappings);
            _predicateMaps.Add(propertyMap);
            return propertyMap;
        }

        public IGraphMap CreateGraphMap()
        {
            var graphMap = new GraphMapConfiguration(_predicateObjectMapNode, R2RMLMappings);
            _graphMaps.Add(graphMap);
            return graphMap;
        }

        public IRefObjectMapConfiguration CreateRefObjectMap(ITriplesMapConfiguration triplesMap)
        {
            var refObjectMap = new RefObjectMapConfiguration(_predicateObjectMapNode, R2RMLMappings.CreateUriNode(triplesMap.Uri), R2RMLMappings);
            _refObjectMaps.Add(refObjectMap);
            return refObjectMap;
        }

        #endregion

        #region Overrides of BaseConfiguration

        protected override void InitializeSubMapsFromCurrentGraph()
        {
            CreateSubMaps(_predicateObjectMapNode, R2RMLUris.RrGraphMapPropety, (node, graph) => new GraphMapConfiguration(node, graph), _graphMaps);
            CreateSubMaps(_predicateObjectMapNode, R2RMLUris.RrObjectMapProperty, (node, graph) => new ObjectMapConfiguration(node, graph), _objectMaps);
            CreateSubMaps(_predicateObjectMapNode, R2RMLUris.RrPredicateMapPropety, (node, graph) => new PredicateMapConfiguration(node, graph), _predicateMaps);
        }

        protected internal override void RecursiveInitializeSubMapsFromCurrentGraph(INode currentNode)
        {
            if (currentNode == null)
                throw new ArgumentNullException("currentNode");

            _predicateObjectMapNode = currentNode;

            base.RecursiveInitializeSubMapsFromCurrentGraph(currentNode);
        }

        #endregion

        #region Implementation of IPredicateObjectMap

        public IEnumerable<IObjectMap> ObjectMaps
        {
            get { return _objectMaps; }
        }

        public IEnumerable<IRefObjectMap> RefObjectMaps
        {
            get { return _refObjectMaps; }
        }

        public IEnumerable<IPredicateMap> PredicateMaps
        {
            get { return _predicateMaps; }
        }

        #endregion
    }
}