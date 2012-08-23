using System;
using System.Collections.Generic;
using System.Linq;
using VDS.RDF;

namespace TCode.r2rml4net.Mapping
{
    /// <summary>
    /// Fluent configuration of subject map backed by a DotNetRDF graph (see <see cref="ISubjectMapConfiguration"/>)
    /// </summary>
    internal class SubjectMapConfiguration : TermMapConfiguration, ISubjectMapConfiguration, INonLiteralTermMapConfigutarion
    {
        private readonly IList<GraphMapConfiguration> _graphMaps = new List<GraphMapConfiguration>();

        internal SubjectMapConfiguration(ITriplesMapConfiguration parentTriplesMap, IGraph r2RMLMappings, MappingOptions mappingOptions)
            : this(parentTriplesMap, r2RMLMappings, r2RMLMappings.CreateBlankNode(), mappingOptions)
        {
        }

        internal SubjectMapConfiguration(ITriplesMapConfiguration parentTriplesMap, IGraph r2RMLMappings, INode node, MappingOptions mappingOptions)
            : base(parentTriplesMap, parentTriplesMap, r2RMLMappings, node, mappingOptions)
        {
        }

        #region Implementation of ISubjectMapConfiguration

        /// <summary>
        /// <see cref="ISubjectMapConfiguration.AddClass"/>
        /// </summary>
        public ISubjectMapConfiguration AddClass(Uri classIri)
        {
            // create SubjectMap - TriplesMap relation if no class has been added
            if(Classes.Length == 0)
                CreateParentMapRelation();

            R2RMLMappings.Assert(
                ((BaseConfiguration) this).Node,
                R2RMLMappings.CreateUriNode(R2RMLUris.RrClassProperty),
                R2RMLMappings.CreateUriNode(classIri));

            return this;
        }

        /// <summary>
        /// <see cref="ISubjectMap.Classes"/>
        /// </summary>
        public Uri[] Classes
        {
            get
            {
                var classes = R2RMLMappings.GetTriplesWithSubjectPredicate(((BaseConfiguration) this).Node, R2RMLMappings.CreateUriNode(R2RMLUris.RrClassProperty));
                return classes.Select(triple => ((IUriNode)triple.Object).Uri).ToArray();
            }
        }

        public IGraphMap CreateGraphMap()
        {
            var graphMap = new GraphMapConfiguration(TriplesMap, this, R2RMLMappings, MappingOptions);
            _graphMaps.Add(graphMap);
            return graphMap;
        }

        #endregion

        #region Overrides of TermMapConfiguration

        /// <summary>
        /// Returns rr:subjectMap
        /// </summary>
        protected internal override IUriNode CreateMapPropertyNode()
        {
            return R2RMLMappings.CreateUriNode(R2RMLUris.RrSubjectMapProperty);
        }

        protected internal override IUriNode CreateShortcutPropertyNode()
        {
            return R2RMLMappings.CreateUriNode(R2RMLUris.RrSubjectProperty);
        }

        #endregion

        #region Implementation of ISubjectMap

        /// <summary>
        /// <see cref="ISubjectMap.URI"/>
        /// </summary>
        public Uri URI
        {
            get { return ConstantValue; }
        }

        public IEnumerable<IGraphMap> GraphMaps
        {
            get { return _graphMaps; }
        }

        #endregion

        #region Overrides of BaseConfiguration

        protected override void InitializeSubMapsFromCurrentGraph()
        {
            CreateSubMaps(R2RMLUris.RrGraphMapPropety, (graph, node) => new GraphMapConfiguration(TriplesMap, this, graph, node, MappingOptions), _graphMaps);
        }

        #endregion
    }
}