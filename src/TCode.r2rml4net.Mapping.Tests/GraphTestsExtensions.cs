﻿using System;
using System.Linq;
using NUnit.Framework;
using VDS.RDF;

namespace TCode.r2rml4net.Mapping.Tests
{
    static class GraphTestsExtensions
    {
        #region VerifyHasTriple

        /// <summary>
        /// Check wheather graph <paramref name="graph"/> has a triple
        /// </summary>
        internal static void VerifyHasTriple(this IGraph graph, string subjectUri, string predicateUri, string objectUri)
        {
            graph.VerifyHasTriple(new Uri(subjectUri), new Uri(predicateUri), new Uri(objectUri));
        }

        /// <summary>
        /// Check wheather graph <paramref name="graph"/> has a triple
        /// </summary>
        internal static void VerifyHasTriple(this IGraph graph, Uri subjectUri, string predicateUri, string objectUri)
        {
            graph.VerifyHasTriple(subjectUri, new Uri(predicateUri), new Uri(objectUri));
        }

        /// <summary>
        /// Check wheather graph <paramref name="graph"/> has a triple
        /// </summary>
        internal static void VerifyHasTriple(this IGraph graph, string subjectUri, Uri predicateUri, string objectUri)
        {
            graph.VerifyHasTriple(new Uri(subjectUri), predicateUri, new Uri(objectUri));
        }

        /// <summary>
        /// Check wheather graph <paramref name="graph"/> has a triple
        /// </summary>
        internal static void VerifyHasTriple(this IGraph graph, string subjectUri, string predicateUri, Uri objectUri)
        {
            graph.VerifyHasTriple(new Uri(subjectUri), new Uri(predicateUri), objectUri);
        }

        /// <summary>
        /// Check wheather graph <paramref name="graph"/> has a triple
        /// </summary>
        internal static void VerifyHasTriple(this IGraph graph, Uri subjectUri, Uri predicateUri, string objectUri)
        {
            graph.VerifyHasTriple(subjectUri, predicateUri, new Uri(objectUri));
        }

        /// <summary>
        /// Check wheather graph <paramref name="graph"/> has a triple
        /// </summary>
        internal static void VerifyHasTriple(this IGraph graph, Uri subjectUri, string predicateUri, Uri objectUri)
        {
            graph.VerifyHasTriple(subjectUri, new Uri(predicateUri), objectUri);
        }

        /// <summary>
        /// Check wheather graph <paramref name="graph"/> has a triple
        /// </summary>
        internal static void VerifyHasTriple(this IGraph graph, string subjectUri, Uri predicateUri, Uri objectUri)
        {
            graph.VerifyHasTriple(new Uri(subjectUri), predicateUri, objectUri);
        }

        /// <summary>
        /// Check wheather graph <paramref name="graph"/> has a triple
        /// </summary>
        internal static void VerifyHasTriple(this IGraph graph, Uri subjectUri, Uri predicateUri, Uri objectUri)
        {
            Assert.IsTrue(graph.ContainsTriple(new Triple(
                graph.CreateUriNode(subjectUri),
                graph.CreateUriNode(predicateUri),
                graph.CreateUriNode(objectUri)
                )), string.Format("Triple <{0}> => <{1}> => <{2}> not found in graph", subjectUri, predicateUri, objectUri));
        } 

        #endregion

        #region VerifyHasTripleWithBlankObject

        /// <summary>
        /// Check wheather graph has triples with subject and predicate and blank node object
        /// </summary>
        internal static void VerifyHasTripleWithBlankObject(this IGraph graph, string subjectUri, string predicateUri, int expectedTriplesCount = 1)
        {
            graph.VerifyHasTripleWithBlankObject(new Uri(subjectUri), new Uri(predicateUri), expectedTriplesCount);
        }

        /// <summary>
        /// Check wheather graph has triples with subject and predicate and blank node object
        /// </summary>
        internal static void VerifyHasTripleWithBlankObject(this IGraph graph, Uri subjectUri, string predicateUri, int expectedTriplesCount = 1)
        {
            graph.VerifyHasTripleWithBlankObject(subjectUri, new Uri(predicateUri), expectedTriplesCount);
        }

        /// <summary>
        /// Check wheather graph has triples with subject and predicate and blank node object
        /// </summary>
        internal static void VerifyHasTripleWithBlankObject(this IGraph graph, string subjectUri, Uri predicateUri, int expectedTriplesCount = 1)
        {
            graph.VerifyHasTripleWithBlankObject(new Uri(subjectUri), predicateUri, expectedTriplesCount);
        }

        /// <summary>
        /// Check wheather graph has triples with subject and predicate and blank node object
        /// </summary>
        internal static void VerifyHasTripleWithBlankObject(this IGraph graph, Uri subjectUri, Uri predicateUri, int expectedTriplesCount = 1)
        {
            var triples = graph.GetTriplesWithSubjectPredicate(
                graph.CreateUriNode(subjectUri),
                graph.CreateUriNode(predicateUri)
                ).ToArray();

            Assert.AreEqual(expectedTriplesCount, triples.Count());
            foreach (var triple in triples)
            {
                Assert.AreEqual(NodeType.Blank, triple.Object.NodeType, "Triple found but object was {0}", triple.Object.NodeType);
            }
        } 

        #endregion
    }
}