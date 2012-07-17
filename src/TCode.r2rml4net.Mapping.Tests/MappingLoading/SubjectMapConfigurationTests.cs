﻿using System;
using System.Linq;
using NUnit.Framework;
using TCode.r2rml4net.RDF;
using VDS.RDF;

namespace TCode.r2rml4net.Mapping.Tests.MappingLoading
{
    [TestFixture]
    public class SubjectMapConfigurationTests
    {
        [Test]
        public void CanInitizalieFromGraph()
        {
            // given
            IGraph graph = new Graph();
            graph.LoadFromString(@"@prefix ex: <http://www.example.com/>.
                                   @prefix rr: <http://www.w3.org/ns/r2rml#>.

                                   ex:triplesMap rr:subjectMap ex:subject .
  
                                   ex:subject 
	                                   rr:template ""http://data.example.com/employee/{EMPNO}"".");

            // when
            var subjectMap = new SubjectMapConfiguration(graph.GetUriNode("ex:triplesMap"), graph);
            subjectMap.RecursiveInitializeSubMapsFromCurrentGraph(graph.GetUriNode("ex:subject"));

            // then
            Assert.AreEqual("http://data.example.com/employee/{EMPNO}", subjectMap.Template);
            Assert.AreEqual("http://www.example.com/triplesMap", ((IUriNode)subjectMap.ParentMapNode).Uri.ToString());
            Assert.AreEqual(graph.GetUriNode("ex:subject"), subjectMap.ConfigurationNode);
        }

        [Test]
        public void CanInitializeWithGraphMaps()
        {
            // given
            IGraph graph = new Graph();
            graph.LoadFromString(@"@prefix ex: <http://www.example.com/>.
                                   @prefix rr: <http://www.w3.org/ns/r2rml#>.

                                   ex:triplesMap rr:subjectMap ex:subject .
  
                                   ex:subject 
	                                   rr:template ""http://data.example.com/employee/{EMPNO}"";
	                                   rr:graphMap [ rr:template ""http://data.example.com/jobgraph/{JOB}"" ] ;
	                                   rr:graphMap [ rr:constant <http://data.example.com/agraph/> ] .");

            // when
            var subjectMap = new SubjectMapConfiguration(graph.GetUriNode("ex:triplesMap"), graph);
            subjectMap.RecursiveInitializeSubMapsFromCurrentGraph(graph.GetUriNode("ex:subject"));

            // then
            Assert.AreEqual(graph.GetUriNode("ex:subject"), subjectMap.ConfigurationNode);
            Assert.AreEqual(2, subjectMap.Graphs.Count());
            Assert.AreEqual("http://data.example.com/jobgraph/{JOB}", subjectMap.Graphs.ElementAt(0).Template);
            Assert.AreEqual(new Uri("http://data.example.com/agraph/"), subjectMap.Graphs.ElementAt(1).GraphUri);
            Assert.AreEqual(graph.GetBlankNode("autos1"), subjectMap.Graphs.Cast<GraphMapConfiguration>().ElementAt(0).ConfigurationNode);
            Assert.AreEqual(graph.GetBlankNode("autos2"), subjectMap.Graphs.Cast<GraphMapConfiguration>().ElementAt(1).ConfigurationNode);
        }

        [Test]
        public void CanInitializeWithShortcutGraphMaps()
        {
            // given
            IGraph graph = new Graph();
            graph.LoadFromString(@"@prefix ex: <http://www.example.com/>.
                                   @prefix rr: <http://www.w3.org/ns/r2rml#>.

                                   ex:triplesMap rr:subjectMap ex:subject .
  
                                   ex:subject 
	                                   rr:template ""http://data.example.com/employee/{EMPNO}"";
	                                   rr:graph <http://data.example.com/shortGraph/> ;
	                                   rr:graph <http://data.example.com/agraph/> .");

            // when
            var subjectMap = new SubjectMapConfiguration(graph.GetUriNode("ex:triplesMap"), graph);
            subjectMap.RecursiveInitializeSubMapsFromCurrentGraph(graph.GetUriNode("ex:subject"));

            // then
            Assert.AreEqual(graph.GetUriNode("ex:subject"), subjectMap.ConfigurationNode);
            Assert.AreEqual(2, subjectMap.Graphs.Count());
            Assert.AreEqual(new Uri("http://data.example.com/shortGraph/"), subjectMap.Graphs.ElementAt(0).GraphUri);
            Assert.AreEqual(new Uri("http://data.example.com/agraph/"), subjectMap.Graphs.ElementAt(1).GraphUri);
            Assert.AreEqual(graph.GetBlankNode("autos1"), subjectMap.Graphs.Cast<GraphMapConfiguration>().ElementAt(0).ConfigurationNode);
            Assert.AreEqual(graph.GetBlankNode("autos2"), subjectMap.Graphs.Cast<GraphMapConfiguration>().ElementAt(1).ConfigurationNode);
        }

        [Test]
        public void CanBeInitializedWithConstantValueUsingShortcut()
        {
            // given
            IGraph graph = new Graph();
            graph.LoadFromString(@"@prefix ex: <http://www.example.com/>.
@prefix rr: <http://www.w3.org/ns/r2rml#>.

ex:TriplesMap rr:subject ex:Value .");

            // when
            var subjectMap = new SubjectMapConfiguration(graph.GetUriNode("ex:TriplesMap"), graph);
            subjectMap.RecursiveInitializeSubMapsFromCurrentGraph(graph.GetBlankNode("autos1"));

            // then
            Assert.AreEqual(graph.CreateUriNode("ex:Value").Uri, subjectMap.ConstantValue);
            Assert.AreEqual(graph.GetBlankNode("autos1"), subjectMap.ConfigurationNode);
        }
    }
}
