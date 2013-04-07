﻿#region Licence
// Copyright (C) 2012 Tomasz Pluskiewicz
// http://r2rml.net/
// r2rml@r2rml.net
// 	
// ------------------------------------------------------------------------
// 	
// This file is part of r2rml4net.
// 	
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal 
// in the Software without restriction, including without limitation the rights 
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
// copies of the Software, and to permit persons to whom the Software is 
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS 
// OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE 
// OR OTHER DEALINGS IN THE SOFTWARE.
// 	
// ------------------------------------------------------------------------
// 
// r2rml4net may alternatively be used under the LGPL licence
// 
// http://www.gnu.org/licenses/lgpl.html
// 
// If these licenses are not suitable for your intended use please contact
// us at the above stated email address to discuss alternative
// terms.
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Query.Datasets;
using VDS.RDF.Update;

namespace TCode.r2rml4net.Mapping
{
    /// <summary>
    /// Base for DotNetRDF-backed fluent R2RML configuration
    /// </summary>
    public abstract class BaseConfiguration : IMapBase
    {
        private readonly INode _node;
        private readonly ITriplesMapConfiguration _triplesMap;

        private const string ShortcutSubmapsReplaceSparql = @"PREFIX rr: <http://www.w3.org/ns/r2rml#>
DELETE { ?map rr:graph ?value . }
INSERT { ?map rr:graphMap [ rr:constant ?value ] . }
WHERE { ?map rr:graph ?value } ;

DELETE { ?map rr:object ?value . }
INSERT { ?map rr:objectMap [ rr:constant ?value ] . }
WHERE { ?map rr:object ?value } ;

DELETE { ?map rr:predicate ?value . }
INSERT { ?map rr:predicateMap [ rr:constant ?value ] . }
WHERE { ?map rr:predicate ?value } ;

DELETE { ?map rr:subject ?value . }
INSERT { ?map rr:subjectMap [ rr:constant ?value ] . }
WHERE { ?map rr:subject ?value }";

        /// <summary>
        /// DotNetRDF graph containing the R2RML mappings
        /// </summary>
        protected internal IGraph R2RMLMappings { get; private set; }

        /// <summary>
        /// </summary>
        protected MappingOptions MappingOptions { get; private set; }

        /// <summary>
        /// Constructor used by <see cref="R2RMLConfiguration"/>
        /// </summary>
        /// <param name="graph">existing graph with mappings</param>
        /// <param name="mappingOptions"><see cref="MappingOptions"/></param>
        internal BaseConfiguration(IGraph graph, MappingOptions mappingOptions)
        {
            if(mappingOptions == null)
                throw new ArgumentNullException("mappingOptions");

            R2RMLMappings = graph;
            MappingOptions = mappingOptions;
            EnsurePrefixes();
        }

        /// <summary>
        /// Constructor used by <see cref="R2RMLConfiguration"/>
        /// </summary>
        /// <param name="baseUri">R2RML graph's base URI</param>
        /// <param name="mappingOptions"><see cref="MappingOptions"/></param>
        protected BaseConfiguration(Uri baseUri, MappingOptions mappingOptions)
            : this(new Graph { BaseUri = baseUri }, mappingOptions)
        {
        }

        /// <summary>
        /// Constructor used by <see cref="TriplesMapConfiguration"/>
        /// </summary>
        protected BaseConfiguration(IGraph existingMappingsGraph, INode node, MappingOptions mappingOptions)
            : this(existingMappingsGraph, mappingOptions)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            _node = node;
            EnsureNoShortcutSubmaps();
        }

        /// <summary>
        /// Constructor used by implementations other than <see cref="R2RMLConfiguration"/> and <see cref="TriplesMapConfiguration"/>
        /// </summary>
        protected BaseConfiguration(ITriplesMapConfiguration triplesMap, IGraph existingMappingsGraph, INode node, MappingOptions mappingOptions)
            : this(existingMappingsGraph, node, mappingOptions)
        {
            _triplesMap = triplesMap;
        }

        #region Implementation of IMapBase

        /// <summary>
        /// Gets the RDF node representing this map
        /// </summary>
        public INode Node
        {
            get
            {
                return _node;
            }
        }

        /// <summary>
        /// Base mapping URI. It will be used to resolve relative values when generating terms
        /// </summary>
        public Uri BaseURI
        {
            get { return R2RMLMappings.BaseUri; }
        }

        #endregion

        private void EnsurePrefixes()
        {
            if (!R2RMLMappings.NamespaceMap.HasNamespace("rr"))
                R2RMLMappings.NamespaceMap.AddNamespace("rr", new Uri("http://www.w3.org/ns/r2rml#"));
        }

        /// <summary>
        /// Reads all maps contained in the current configuration and creates configuration objects
        /// </summary>
        /// <remarks>Used in loading configuration from an exinsting graph</remarks>
        protected internal void RecursiveInitializeSubMapsFromCurrentGraph()
        {
            InitializeSubMapsFromCurrentGraph();
        }

        /// <summary>
        /// Gets value indicating whether configuration class is represented by an RDF node
        /// </summary>
        protected virtual bool UsesNode { get { return true; } }

        /// <summary>
        /// Implemented in child classes should create submaps and for each of the run the <see cref="BaseConfiguration.RecursiveInitializeSubMapsFromCurrentGraph"/> method
        /// </summary>
        protected abstract void InitializeSubMapsFromCurrentGraph();

        /// <summary>
        /// Overriden in child classes should change shortcut properties to maps
        /// </summary>
        /// <example>{ [] rr:graph ex:instance } should become { [] rr:graphMap [ rr:constant ex:instance ] }</example>
        protected void EnsureNoShortcutSubmaps()
        {
            TripleStore store = new TripleStore();
            store.Add(R2RMLMappings);

            var dataset = new InMemoryDataset(store, R2RMLMappings.BaseUri);
            ISparqlUpdateProcessor processor = new LeviathanUpdateProcessor(dataset);
            var updateParser = new SparqlUpdateParser();

            processor.ProcessCommandSet(updateParser.ParseFromString(ShortcutSubmapsReplaceSparql));
        }

        /// <summary>
        /// </summary>
        protected void CreateSubMaps<TConfiguration>(string property, Func<IGraph, INode, TConfiguration> createSubConfiguration, IList<TConfiguration> subMaps)
            where TConfiguration : BaseConfiguration
        {
            var mapPropety = R2RMLMappings.CreateUriNode(property);
            var triples = R2RMLMappings.GetTriplesWithSubjectPredicate(this.Node, mapPropety);

            foreach (var triple in triples.ToArray())
            {
                var subConfiguration = createSubConfiguration(R2RMLMappings, triple.Object);
                subConfiguration.RecursiveInitializeSubMapsFromCurrentGraph();
                subMaps.Add(subConfiguration);
            }
        }

        /// <summary>
        /// Gets the parent <see cref="ITriplesMapConfiguration"/> containing this map
        /// </summary>
        protected internal virtual ITriplesMapConfiguration TriplesMap
        {
            get { return _triplesMap; }
        }
    }
}
