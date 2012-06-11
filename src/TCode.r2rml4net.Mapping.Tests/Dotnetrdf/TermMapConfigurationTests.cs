﻿using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using TCode.r2rml4net.Mapping.Fluent.Dotnetrdf;
using VDS.RDF;

namespace TCode.r2rml4net.Mapping.Tests.Dotnetrdf
{
    [TestFixture]
    public class TermMapConfigurationTests
    {
        private INode _triplesMapNode;
        private Mock<TermMapConfiguration> _termMapConfigurationMock;
        private TermMapConfiguration _termMapConfiguration;
        private IGraph _graph;

        [SetUp]
        public void Setup()
        {
            _graph = new R2RMLConfiguration().R2RMLMappings;
            _triplesMapNode = _graph.CreateUriNode(new Uri("http://mapping.com/SomeMap"));
            _termMapConfigurationMock = new Mock<TermMapConfiguration>(_triplesMapNode, _graph)
                                            {
                                                CallBase = true
                                            };
            _termMapConfiguration = _termMapConfigurationMock.Object;
        }

        [Test]
        public void ConstantIRIValueCanBeSetOnlyOnce()
        {
            // given
            Uri uri = new Uri("http://example.com/SomeResource");
            _termMapConfigurationMock
                .Setup(config => config.CreateConstantPropertyNode())
                .Returns(_graph.CreateUriNode(new Uri(UriConstants.RrSubjectProperty)));
            _termMapConfigurationMock
                .Setup(config => config.CreateMapPropertyNode())
                .Returns(_graph.CreateUriNode(new Uri(UriConstants.RrSubjectMapProperty)));

            // when
            _termMapConfiguration.IsConstantValued(uri);

            // then
            Assert.Throws<InvalidTriplesMapException>(() => _termMapConfiguration.IsConstantValued(uri));
            _termMapConfigurationMock.VerifyAll();
        }

        [Test]
        public void TermMapCanBeColumnValued()
        {
            // given
            const string columnName = "Name";
            _termMapConfigurationMock
                .Setup(config => config.CreateConstantPropertyNode())
                .Returns(_graph.CreateUriNode(new Uri(UriConstants.RrSubjectProperty)));
            _termMapConfigurationMock
                .Setup(config => config.CreateMapPropertyNode())
                .Returns(_graph.CreateUriNode(new Uri(UriConstants.RrSubjectMapProperty)));

            // when
            _termMapConfiguration.IsColumnValued(columnName);

            // then
            Assert.IsTrue(_termMapConfiguration.R2RMLMappings.GetTriplesWithSubjectPredicate(
                _termMapConfiguration.TermMapNode,
                _termMapConfiguration.CreateMapPropertyNode()).Any());
            _termMapConfiguration.R2RMLMappings.VerifyHasTripleWithBlankSubjectAndLiteralObject(UriConstants.RrColumnProperty, columnName);
        }

        public void ColumnValueCanONlyBeSetOnce()
        {
            // given
            const string columnName = "Name";
            _termMapConfigurationMock
                .Setup(config => config.CreateMapPropertyNode())
                .Returns(_graph.CreateUriNode(new Uri(UriConstants.RrSubjectMapProperty)));

            // when
            _termMapConfiguration.IsColumnValued(columnName);

            // then
            Assert.Throws<InvalidTriplesMapException>(() => _termMapConfiguration.IsColumnValued(columnName));
            _termMapConfigurationMock.VerifyAll();
        }
    }
}
