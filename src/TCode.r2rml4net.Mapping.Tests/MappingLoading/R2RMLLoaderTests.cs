﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NUnit.Framework;
using TCode.r2rml4net.RDF;
using VDS.RDF;

namespace TCode.r2rml4net.Mapping.Tests.MappingLoading
{
    [TestFixture]
    public class R2RMLLoaderTests
    {
        #region Simple Test GraphUri
        private const string TestGraph = @"@prefix rr: <http://www.w3.org/ns/r2rml#> .
@prefix xsd: <http://www.w3.org/2001/XMLSchema#> .
@base <http://mappingpedia.org/rdb2rdf/r2rml/tc/> .

<StudentTriplesMap>
        a rr:TriplesMap;
    
    rr:logicalTable [ rr:tableName ""Student""; ];

    rr:subjectMap 
	[ 
	    rr:termType rr:BlankNode;  
	    rr:class <http://example.com/Student>
	];

    rr:predicateObjectMap
    [ 
        rr:predicateMap	[ rr:constant <http://example.com/Student#Name> ] ;
        rr:objectMap		[ rr:column ""Name"" ]
    ]
    .

<CourseTriplesMap> a rr:TriplesMap .";
        #endregion

        [Test]
        public void CanLoadR2RMLFromString()
        {
            // when
            IR2RML mappings = R2RMLLoader.Load(TestGraph);

            // then
            Assert.IsNotNull(mappings);
            Assert.AreEqual(2, mappings.TriplesMaps.Count());
            Assert.AreEqual(
                new Uri("http://mappingpedia.org/rdb2rdf/r2rml/tc/StudentTriplesMap"),
                ((IUriNode)mappings.TriplesMaps.Cast<TriplesMapConfiguration>().ElementAt(0).ConfigurationNode).Uri);
            Assert.AreEqual(
                new Uri("http://mappingpedia.org/rdb2rdf/r2rml/tc/CourseTriplesMap"),
                ((IUriNode)mappings.TriplesMaps.Cast<TriplesMapConfiguration>().ElementAt(1).ConfigurationNode).Uri);
        }

        [Test]
        public void CanLoadR2RMLFromStream()
        {
            IR2RML mappings;

            // when
            using (Stream turtle = Assembly.GetExecutingAssembly().GetManifestResourceStream("TCode.r2rml4net.Mapping.Tests.MappingLoading.ComplexTestGraph.ttl"))
            {
                mappings = R2RMLLoader.Load(turtle);
            }

            // then
            Assert.IsNotNull(mappings);
            Assert.AreEqual(5, mappings.TriplesMaps.Count());
            Assert.AreEqual(
                new Uri("http://mappingpedia.org/rdb2rdf/r2rml/tc/StudentTriplesMap"),
                ((IUriNode)mappings.TriplesMaps.Cast<TriplesMapConfiguration>().ElementAt(0).ConfigurationNode).Uri);
            Assert.AreEqual(
                new Uri("http://mappingpedia.org/rdb2rdf/r2rml/tc/TriplesMap2"),
                ((IUriNode)mappings.TriplesMaps.Cast<TriplesMapConfiguration>().ElementAt(1).ConfigurationNode).Uri);
            Assert.AreEqual(
                new Uri("http://mappingpedia.org/rdb2rdf/r2rml/tc/TriplesMap1"),
                ((IUriNode)mappings.TriplesMaps.Cast<TriplesMapConfiguration>().ElementAt(2).ConfigurationNode).Uri);
            Assert.AreEqual(
                new Uri("http://mappingpedia.org/rdb2rdf/r2rml/tc/ManyToManyTriplesMap"),
                ((IUriNode)mappings.TriplesMaps.Cast<TriplesMapConfiguration>().ElementAt(3).ConfigurationNode).Uri);
            Assert.AreEqual(
                new Uri("http://mappingpedia.org/rdb2rdf/r2rml/tc/SimplerManyToManyTriplesMap"),
                ((IUriNode)mappings.TriplesMaps.Cast<TriplesMapConfiguration>().ElementAt(4).ConfigurationNode).Uri);
        }
    }
}
