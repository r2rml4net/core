﻿using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using TCode.r2rml4net.RDF;
using VDS.RDF;

namespace TCode.r2rml4net.Mapping.Tests.RDF
{
    public class BlankNodeSubjectReplaceHandlerTests
    {
        private BlankNodeSubjectReplaceHandler _handler;
        private Mock<IRdfHandler> _decoratedHandler;

        [SetUp]
        public void Setup()
        {
            _decoratedHandler = new Mock<IRdfHandler>(MockBehavior.Strict);
            _handler = new BlankNodeSubjectReplaceHandler(_decoratedHandler.Object);
            _decoratedHandler.Setup(h => h.StartRdf());
            _handler.StartRdf();
        }

        [Test]
        public void ReplacesBlankNodeSubjectOnce()
        {
            // given
            var newBlankNode = MockNode<IBlankNode>();
            var blankNode = new Mock<IBlankNode>();
            blankNode.Setup(b => b.InternalID).Returns("some invalid identifier");
            IList<INode> subjects = new List<INode>();
            Triple triple = new Triple(blankNode.Object, MockNode<IUriNode>(), MockNode<ILiteralNode>());
            Triple triple2 = new Triple(blankNode.Object, MockNode<IUriNode>(), MockNode<ILiteralNode>());
            _decoratedHandler.Setup(h => h.HandleTriple(It.IsAny<Triple>()))
                             .Callback((Triple t) => subjects.Add(t.Subject))
                             .Returns(true);
            _decoratedHandler.Setup(h => h.CreateBlankNode()).Returns(newBlankNode);

            // when
            _handler.HandleTriple(triple);
            _handler.HandleTriple(triple2);

            // then
            foreach (var subject in subjects)
            {
                Assert.IsNotNull(subject);
                Assert.AreSame(newBlankNode, subject);
            }
            _decoratedHandler.Verify(h=>h.CreateBlankNode(), Times.Once());
        }

        [Test]
        public void TestDoesNotReplaceUriSubject()
        {
            // given
            var subj = MockNode<IUriNode>();
            var subj1 = MockNode<IUriNode>();
            Triple triple = new Triple(subj, MockNode<IUriNode>(), MockNode<ILiteralNode>());
            Triple triple1 = new Triple(subj1, MockNode<IUriNode>(), MockNode<ILiteralNode>());
            _decoratedHandler.Setup(h => h.HandleTriple(It.IsAny<Triple>())).Returns(true);

            // when
            _handler.HandleTriple(triple);

            // then
            Assert.AreSame(subj, triple.Subject);
            Assert.AreSame(subj1, triple1.Subject);
        }

        [Test]
        public void AlsoReplacesBlankNodeObjects()
        {
            // given
            var newBlankNode = MockNode<IBlankNode>();
            var blankNode = new Mock<IBlankNode>();
            blankNode.Setup(b => b.InternalID).Returns("some invalid identifier");
            IList<Triple> triples = new List<Triple>();
            Triple triple = new Triple(blankNode.Object, MockNode<IUriNode>(), MockNode<ILiteralNode>());
            Triple triple2 = new Triple(MockNode<IUriNode>(), MockNode<IUriNode>(), blankNode.Object);
            _decoratedHandler.Setup(h => h.HandleTriple(It.IsAny<Triple>()))
                             .Callback((Triple t) => triples.Add(t))
                             .Returns(true);
            _decoratedHandler.Setup(h => h.CreateBlankNode()).Returns(newBlankNode);

            // when
            _handler.HandleTriple(triple);
            _handler.HandleTriple(triple2);

            // then
            _decoratedHandler.Verify(h => h.CreateBlankNode(), Times.Once()); 
            Assert.AreEqual(triples[0].Subject, triples[1].Object);
        }

        private TNode MockNode<TNode>() where TNode : class, INode
        {
            var mock = new Mock<TNode>();
            return mock.Object;
        }
    }
}