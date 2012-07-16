﻿using System;
using System.Linq;
using NUnit.Framework;
using TCode.r2rml4net.RDF;
using VDS.RDF;
using Moq;

namespace TCode.r2rml4net.Mapping.Tests.MappingLoading
{
    [TestFixture]
    public class PredicateObjectMapConfigurationTests
    {
        [Test]
        public void CanBeInitializedWithPredicateMaps()
        {
            // given
            IGraph graph = new Graph();
            graph.LoadFromString(@"@prefix ex: <http://www.example.com/>.
@prefix rr: <http://www.w3.org/ns/r2rml#>.

ex:triplesMap rr:predicateObjectMap ex:PredicateObjectMap .
  
ex:PredicateObjectMap 
    rr:predicateMap [ rr:template ""http://data.example.com/employee/{EMPNO}"" ] ;
    rr:predicateMap [ rr:template ""http://data.example.com/user/{EMPNO}"" ].");

            // when
            var predicateObjectMap = new PredicateObjectMapConfiguration(graph.GetUriNode("ex:triplesMap"), graph);
            predicateObjectMap.RecursiveInitializeSubMapsFromCurrentGraph(graph.GetUriNode("ex:PredicateObjectMap"));

            // then
            Assert.AreEqual(2, predicateObjectMap.PredicateMaps.Count());
            Assert.AreEqual("http://data.example.com/employee/{EMPNO}", predicateObjectMap.PredicateMaps.ElementAt(0).Template);
            Assert.AreEqual("http://data.example.com/user/{EMPNO}", predicateObjectMap.PredicateMaps.ElementAt(1).Template);
            Assert.AreEqual(graph.GetBlankNode("autos1"), predicateObjectMap.PredicateMaps.Cast<PredicateMapConfiguration>().ElementAt(0).ConfigurationNode);
            Assert.AreEqual(graph.GetBlankNode("autos2"), predicateObjectMap.PredicateMaps.Cast<PredicateMapConfiguration>().ElementAt(1).ConfigurationNode);
        }

        [Test]
        public void CanBeInitializedWithPredicateMapsUsingShortcut()
        {
            // given
            IGraph graph = new Graph();
            graph.LoadFromString(@"@prefix ex: <http://www.example.com/>.
@prefix rr: <http://www.w3.org/ns/r2rml#>.

ex:triplesMap rr:predicateObjectMap ex:PredicateObjectMap .
  
ex:PredicateObjectMap rr:predicate ex:Employee, ex:Worker .");

            // when
            var predicateObjectMap = new PredicateObjectMapConfiguration(graph.GetUriNode("ex:triplesMap"), graph);
            predicateObjectMap.RecursiveInitializeSubMapsFromCurrentGraph(graph.GetUriNode("ex:PredicateObjectMap"));

            // then
            Assert.AreEqual(2, predicateObjectMap.PredicateMaps.Count());
            Assert.AreEqual(new Uri("http://www.example.com/Employee"), predicateObjectMap.PredicateMaps.ElementAt(0).Predicate);
            Assert.AreEqual(new Uri("http://www.example.com/Worker"), predicateObjectMap.PredicateMaps.ElementAt(1).Predicate);
            Assert.AreEqual(graph.GetBlankNode("autos1"), predicateObjectMap.PredicateMaps.Cast<PredicateMapConfiguration>().ElementAt(0).ConfigurationNode);
            Assert.AreEqual(graph.GetBlankNode("autos2"), predicateObjectMap.PredicateMaps.Cast<PredicateMapConfiguration>().ElementAt(1).ConfigurationNode);
        }

        [Test]
        public void CanBeInitializedWithGraphMapsUsingShortcut()
        {
            // given
            IGraph graph = new Graph();
            graph.LoadFromString(@"@prefix ex: <http://www.example.com/>.
@prefix rr: <http://www.w3.org/ns/r2rml#>.

ex:triplesMap rr:predicateObjectMap ex:PredicateObjectMap .
  
ex:PredicateObjectMap rr:graph ex:Employee, ex:Worker .");

            // when
            var predicateObjectMap = new PredicateObjectMapConfiguration(graph.GetUriNode("ex:triplesMap"), graph);
            predicateObjectMap.RecursiveInitializeSubMapsFromCurrentGraph(graph.GetUriNode("ex:PredicateObjectMap"));

            // then
            Assert.AreEqual(2, predicateObjectMap.GraphMaps.Count());
            Assert.AreEqual(new Uri("http://www.example.com/Employee"), predicateObjectMap.GraphMaps.ElementAt(0).GraphUri);
            Assert.AreEqual(new Uri("http://www.example.com/Worker"), predicateObjectMap.GraphMaps.ElementAt(1).GraphUri);
            Assert.AreEqual(graph.GetBlankNode("autos1"), predicateObjectMap.GraphMaps.Cast<GraphMapConfiguration>().ElementAt(0).ConfigurationNode);
            Assert.AreEqual(graph.GetBlankNode("autos2"), predicateObjectMap.GraphMaps.Cast<GraphMapConfiguration>().ElementAt(1).ConfigurationNode);
        }

        [Test]
        public void CanBeInitializedWithObjectMaps()
        {
            // given
            IGraph graph = new Graph();
            graph.LoadFromString(@"@prefix ex: <http://www.example.com/>.
@prefix rr: <http://www.w3.org/ns/r2rml#>.

ex:triplesMap rr:predicateObjectMap ex:PredicateObjectMap .
  
ex:PredicateObjectMap rr:objectMap 
    [ rr:constant ex:Employee ], 
    [ rr:template ""http://data.example.com/user/{EMPNO}"" ] .");

            // when
            var predicateObjectMap = new PredicateObjectMapConfiguration(graph.GetUriNode("ex:triplesMap"), graph);
            predicateObjectMap.RecursiveInitializeSubMapsFromCurrentGraph(graph.GetUriNode("ex:PredicateObjectMap"));

            // then
            Assert.AreEqual(2, predicateObjectMap.ObjectMaps.Count());
            Assert.AreEqual(new Uri("http://www.example.com/Employee"), predicateObjectMap.ObjectMaps.ElementAt(0).Object);
            Assert.AreEqual("http://data.example.com/user/{EMPNO}", predicateObjectMap.ObjectMaps.ElementAt(1).Template);
            Assert.AreEqual(graph.GetBlankNode("autos1"), predicateObjectMap.ObjectMaps.Cast<ObjectMapConfiguration>().ElementAt(0).ConfigurationNode);
            Assert.AreEqual(graph.GetBlankNode("autos2"), predicateObjectMap.ObjectMaps.Cast<ObjectMapConfiguration>().ElementAt(1).ConfigurationNode);
        }

        [Test]
        public void CanBeInitializedWithObjectMapsUsingShortcut()
        {
            // given
            IGraph graph = new Graph();
            graph.LoadFromString(@"@prefix ex: <http://www.example.com/>.
@prefix rr: <http://www.w3.org/ns/r2rml#>.

ex:triplesMap rr:predicateObjectMap ex:PredicateObjectMap .
  
ex:PredicateObjectMap rr:object ex:Employee, ex:Worker .");

            // when
            var predicateObjectMap = new PredicateObjectMapConfiguration(graph.GetUriNode("ex:triplesMap"), graph);
            predicateObjectMap.RecursiveInitializeSubMapsFromCurrentGraph(graph.GetUriNode("ex:PredicateObjectMap"));

            // then
            Assert.AreEqual(2, predicateObjectMap.ObjectMaps.Count());
            Assert.AreEqual(new Uri("http://www.example.com/Employee"), predicateObjectMap.ObjectMaps.ElementAt(0).Object);
            Assert.AreEqual(new Uri("http://www.example.com/Worker"), predicateObjectMap.ObjectMaps.ElementAt(1).Object);
            Assert.AreEqual(graph.GetBlankNode("autos1"), predicateObjectMap.ObjectMaps.Cast<ObjectMapConfiguration>().ElementAt(0).ConfigurationNode);
            Assert.AreEqual(graph.GetBlankNode("autos2"), predicateObjectMap.ObjectMaps.Cast<ObjectMapConfiguration>().ElementAt(1).ConfigurationNode);
        }
    }
}
