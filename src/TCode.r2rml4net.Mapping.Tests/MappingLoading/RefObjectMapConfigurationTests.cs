﻿#region Licence
// Copyright (C) 2012-2018 Tomasz Pluskiewicz
// http://r2rml.net/
// r2rml@t-code.pl
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
using System.Linq;
using Moq;
using Xunit;
using Resourcer;
using TCode.r2rml4net.Mapping.Fluent;
using VDS.RDF;

namespace TCode.r2rml4net.Mapping.Tests.MappingLoading
{
    public class RefObjectMapConfigurationTests
    {
        RefObjectMapConfiguration _refObjectMap;
        private readonly Mock<ITriplesMapConfiguration> _parentTriplesMap;
        private readonly Mock<ITriplesMapConfiguration> _referencedTriplesMap;
        private readonly Mock<IPredicateObjectMap> _predicateObjectMap;

        public RefObjectMapConfigurationTests()
        {
            _parentTriplesMap = new Mock<ITriplesMapConfiguration>();
            _referencedTriplesMap = new Mock<ITriplesMapConfiguration>();
            _predicateObjectMap = new Mock<IPredicateObjectMap>();
        }

        [Fact]
        public void CanInitializeJoinConditions()
        {
            // given
            IGraph graph = new Graph();
            graph.LoadFromString(Resource.AsString("Graphs.RefObjectMap.JoinCondition.ttl"));
            var predicateObjectMapNode = graph.GetTriplesWithPredicate(graph.CreateUriNode("rr:predicateObjectMap")).Single().Object;
            _predicateObjectMap.Setup(map => map.Node).Returns(predicateObjectMapNode);
            _parentTriplesMap.Setup(tm => tm.Node).Returns(graph.GetUriNode("ex:TriplesMap"));
            _referencedTriplesMap.Setup(tm => tm.Node).Returns(graph.GetUriNode("ex:TriplesMap2"));

            // when
            var blankNode = graph.GetTriplesWithPredicate(graph.CreateUriNode("rr:objectMap")).Single().Object;
            _refObjectMap = new RefObjectMapConfiguration(_predicateObjectMap.Object, _parentTriplesMap.Object, _referencedTriplesMap.Object, graph, blankNode);
            _refObjectMap.RecursiveInitializeSubMapsFromCurrentGraph();

            // then
            Assert.Single(_refObjectMap.JoinConditions);
            Assert.Equal("DEPTNO", _refObjectMap.JoinConditions.ElementAt(0).ChildColumn);
            Assert.Equal("ID", _refObjectMap.JoinConditions.ElementAt(0).ParentColumn);
            Assert.Equal(blankNode, _refObjectMap.Node);
        }

        [Fact]
        public void MultipleJoinConditionsLoading()
        {
            IR2RML mappings = R2RMLLoader.Load(Resource.AsString("Graphs.RefObjectMap.MultipleJoinConditions.ttl"), new MappingOptions());
            Assert.NotNull(mappings);
            Assert.Equal(3, mappings.TriplesMaps.Count());

            var checkActionTriples = mappings.TriplesMaps.ElementAt(1);
            Assert.Equal(
                    new Uri("http://example.com/base/CheckActionSubjectTriples"),
                    ((IUriNode)checkActionTriples.Node).Uri);

            var sanctionTriples = mappings.TriplesMaps.ElementAt(2);
            Assert.Equal(
                    new Uri("http://example.com/base/SanctionReasonTriples"),
                    ((IUriNode)sanctionTriples.Node).Uri);

            Assert.Single(checkActionTriples.PredicateObjectMaps);
            Assert.Single(sanctionTriples.PredicateObjectMaps);

            var checkActionTriplesPOM = checkActionTriples.PredicateObjectMaps.First();
            var sanctionTriplesPOM = sanctionTriples.PredicateObjectMaps.First();

            Assert.Empty(checkActionTriplesPOM.ObjectMaps);
            Assert.Single(checkActionTriplesPOM.RefObjectMaps);

            Assert.Empty(sanctionTriplesPOM.ObjectMaps);
            Assert.Single(sanctionTriplesPOM.RefObjectMaps);

            var checkActionTriplesROM = checkActionTriplesPOM.RefObjectMaps.First();
            var sanctionTriplesROM = sanctionTriplesPOM.RefObjectMaps.First();

            Assert.Single(checkActionTriplesROM.JoinConditions);
            Assert.Single(sanctionTriplesROM.JoinConditions);

            var checkActionTriplesJoinCond = checkActionTriplesROM.JoinConditions.First();
            var sanctionTriplesJoinCond = sanctionTriplesROM.JoinConditions.First();

            Assert.Equal("LawChild1", checkActionTriplesJoinCond.ChildColumn);
            Assert.Equal("LawParent1", checkActionTriplesJoinCond.ParentColumn);

            Assert.Equal("LawChild2", sanctionTriplesJoinCond.ChildColumn);
            Assert.Equal("LawParent2", sanctionTriplesJoinCond.ParentColumn);
        }
    }
}
