﻿using System;
using System.Data;
using Moq;
using NUnit.Framework;
using TCode.r2rml4net.Log;
using TCode.r2rml4net.Mapping;
using TCode.r2rml4net.TriplesGeneration;
using VDS.RDF;

namespace TCode.r2rml4net.Tests.TriplesGeneration
{
    [TestFixture]
    public class RDFTermGeneratorTests
    {
        private RDFTermGenerator _termGenerator;
        private Mock<ISubjectMap> _subjectMap;
        private Mock<IPredicateMap> _predicateMap;
        private Mock<ISQLValuesMappingStrategy> _lexicalFormProvider;
        private Mock<IDataRecord> _logicalRow;
        private Mock<ITermMap> _termMap;
        private Mock<ITermType> _termType;
        private Mock<IGraphMap> _graphMap;
        private Mock<IObjectMap> _objectMap;
        private Mock<IRDFTermGenerationLog> _log;

        [SetUp]
        public void Setup()
        {
            _log = new Mock<IRDFTermGenerationLog>();
            _objectMap = new Mock<IObjectMap>();
            _termMap = new Mock<ITermMap>();
            _termType = new Mock<ITermType>();
            _subjectMap = new Mock<ISubjectMap>();
            _logicalRow = new Mock<IDataRecord>(MockBehavior.Strict);
            _lexicalFormProvider = new Mock<ISQLValuesMappingStrategy>();

            _termMap.Setup(map => map.TermType).Returns(_termType.Object);

            _termGenerator = new RDFTermGenerator
                                 {
                                     SqlValuesMappingStrategy = _lexicalFormProvider.Object,
                                     Log = _log.Object
                                 };
        }

        #region Test constant valued term map

        [Test]
        public void SubjectMapsConstantIsAnIri()
        {
            // given
            _subjectMap.Setup(sm => sm.IsConstantValued).Returns(true);
            var uri = new Uri("http://www.example.com/const");
            _subjectMap.Setup(sm => sm.URI).Returns(uri);

            // when
            var node = _termGenerator.GenerateTerm<IUriNode>(_subjectMap.Object, _logicalRow.Object);

            // then
            Assert.AreEqual(uri, node.Uri);
            _log.Verify(log => log.LogTermGenerated(node));
        }

        [Test]
        public void PredicateMapsConstantIsAnIri()
        {
            // given
            _predicateMap = new Mock<IPredicateMap>();
            _predicateMap.Setup(sm => sm.IsConstantValued).Returns(true);
            var uri = new Uri("http://www.example.com/const");
            _predicateMap.Setup(sm => sm.URI).Returns(uri);

            // when
            var node = _termGenerator.GenerateTerm<IUriNode>(_predicateMap.Object, _logicalRow.Object);

            // then
            Assert.AreEqual(uri, node.Uri);
            _log.Verify(log => log.LogTermGenerated(node));
        }

        [Test]
        public void GraphMapsConstantIsAnIri()
        {
            // given
            _graphMap = new Mock<IGraphMap>();
            _graphMap.Setup(sm => sm.IsConstantValued).Returns(true);
            var uri = new Uri("http://www.example.com/const");
            _graphMap.Setup(sm => sm.URI).Returns(uri);

            // when
            var node = _termGenerator.GenerateTerm<IUriNode>(_graphMap.Object, _logicalRow.Object);

            // then
            Assert.AreEqual(uri, node.Uri);
            _log.Verify(log => log.LogTermGenerated(node));
        }

        [Test]
        public void ObjectMapsConstantCanBeIRI()
        {
            // given
            _objectMap = new Mock<IObjectMap>();
            _objectMap.Setup(sm => sm.IsConstantValued).Returns(true);
            var uri = new Uri("http://www.example.com/const");
            _objectMap.Setup(sm => sm.URI).Returns(uri);

            // when
            var node = _termGenerator.GenerateTerm<IUriNode>(_objectMap.Object, _logicalRow.Object);

            // then
            Assert.AreEqual(uri, node.Uri);
            _log.Verify(log => log.LogTermGenerated(node));
        }

        [Test]
        public void UriValuedConstantMustHaveConstantSet()
        {
            // given
            var termMap = new Mock<IUriValuedTermMap>();
            termMap.Setup(sm => sm.IsConstantValued).Returns(true);

            // when
            Assert.Throws<InvalidTermException>(() => _termGenerator.GenerateTerm<IUriNode>(termMap.Object, _logicalRow.Object));

            // then
            _logicalRow.VerifyAll();
            _objectMap.VerifyAll();
            _termType.VerifyAll();
        }

        [Test]
        public void ObjectMapsConstantCanBeLiteral()
        {
            // given
            _objectMap = new Mock<IObjectMap>();
            _objectMap.Setup(sm => sm.IsConstantValued).Returns(true);
            const string literal = "some value";
            _objectMap.Setup(sm => sm.Literal).Returns(literal);

            // when
            var node = _termGenerator.GenerateTerm<ILiteralNode>(_objectMap.Object, _logicalRow.Object);

            // then
            Assert.AreEqual(literal, node.Value);
            _log.Verify(log => log.LogTermGenerated(node));
        }

        [Test]
        public void ObjectMapsConstantCannotBeBothLiteralAndIri()
        {
            // given
            _objectMap = new Mock<IObjectMap>();
            _objectMap.Setup(sm => sm.IsConstantValued).Returns(true);
            const string literal = "some value";
            var uri = new Uri("http://www.example.com/const");
            _objectMap.Setup(sm => sm.Literal).Returns(literal);
            _objectMap.Setup(sm => sm.URI).Returns(uri);

            // then
            Assert.Throws<InvalidTermException>(() => _termGenerator.GenerateTerm<INode>(_objectMap.Object, _logicalRow.Object));
        }

        [Test]
        public void ObjectMapsConstantMusBeEitherLiteralOrIri()
        {
            // given
            _objectMap = new Mock<IObjectMap>();
            _objectMap.Setup(sm => sm.IsConstantValued).Returns(true);

            // when
            Assert.Throws<InvalidTermException>(() => _termGenerator.GenerateTerm<INode>(_objectMap.Object, _logicalRow.Object));

            // then
            _logicalRow.VerifyAll();
            _objectMap.VerifyAll();
            _termType.VerifyAll();
        }

        #endregion

        #region Test column-valued term maps

        private const string ColumnName = "Column";
        private const int ColumnIndex = 3;

        [Test]
        public void NullValueReturnsNullNode()
        {
            // given
            _logicalRow.Setup(rec => rec.GetOrdinal(ColumnName)).Returns(ColumnIndex).Verifiable();
            _logicalRow.Setup(rec => rec.IsDBNull(ColumnIndex)).Returns(true).Verifiable();
            _termMap.Setup(map => map.IsColumnValued).Returns(true);
            _termMap.Setup(map => map.ColumnName).Returns(ColumnName);

            // when
            var node = _termGenerator.GenerateTerm<INode>(_termMap.Object, _logicalRow.Object);

            // then
            Assert.IsNull(node);
            _logicalRow.VerifyAll();
            _log.Verify(log => log.LogNullTermGenerated(_termMap.Object));
        }

        [Test]
        public void ColumnNotFoundThrowsAndLogsError()
        {
            // given
            _logicalRow.Setup(rec => rec.GetOrdinal(ColumnName))
                       .Throws<IndexOutOfRangeException>()
                       .Verifiable();
            _termMap.Setup(map => map.IsColumnValued).Returns(true);
            _termMap.Setup(map => map.ColumnName).Returns(ColumnName);

            // when
            Assert.Throws<InvalidTriplesMapException>(() => _termGenerator.GenerateTerm<INode>(_termMap.Object, _logicalRow.Object));

            // then
            _logicalRow.VerifyAll();
            _log.Verify(log => log.LogColumnNotFound(_termMap.Object, ColumnName));
        }

        #region IRI term type

        [Test]
        public void AbsoluteUriValueCreatesUriNode()
        {
            // given
            const string expected = "http://www.example.com/value";
            _logicalRow.Setup(rec => rec.GetOrdinal(ColumnName)).Returns(ColumnIndex).Verifiable();
            _logicalRow.Setup(rec => rec.IsDBNull(ColumnIndex)).Returns(false).Verifiable();
            Uri datatype;
            _lexicalFormProvider.Setup(lex => lex.GetLexicalForm(ColumnIndex, It.IsAny<IDataRecord>(), out datatype))
                                .Returns(expected)
                                .Verifiable();
            _termMap.Setup(map => map.IsColumnValued).Returns(true).Verifiable();
            _termMap.Setup(map => map.ColumnName).Returns(ColumnName).Verifiable();
            _termType.Setup(type => type.IsURI).Returns(true).Verifiable();

            // when
            var node = _termGenerator.GenerateTerm<INode>(_termMap.Object, _logicalRow.Object);

            // then
            Assert.IsNotNull(node);
            Assert.IsTrue(node is IUriNode);
            Assert.AreEqual(expected, (node as IUriNode).Uri.AbsoluteUri);
            _logicalRow.VerifyAll();
            _termMap.VerifyAll();
            _termType.VerifyAll();
            _log.Verify(log => log.LogTermGenerated(node));
        }

        [Test]
        public void RelativeUriValueCreatesUriNode()
        {
            const string expected = "http://www.example.com/value";

            // given
            _logicalRow.Setup(rec => rec.GetOrdinal(ColumnName)).Returns(ColumnIndex).Verifiable();
            _logicalRow.Setup(rec => rec.IsDBNull(ColumnIndex)).Returns(false).Verifiable();
            Uri datatype;
            _lexicalFormProvider.Setup(lex => lex.GetLexicalForm(ColumnIndex, It.IsAny<IDataRecord>(), out datatype))
                                .Returns("value")
                                .Verifiable();
            _termMap.Setup(map => map.IsColumnValued).Returns(true).Verifiable();
            _termMap.Setup(map => map.ColumnName).Returns(ColumnName).Verifiable();
            _termMap.Setup(map => map.BaseURI).Returns(new Uri("http://www.example.com/")).Verifiable();
            _termType.Setup(type => type.IsURI).Returns(true).Verifiable();

            // when
            var node = _termGenerator.GenerateTerm<INode>(_termMap.Object, _logicalRow.Object);

            // then
            Assert.IsNotNull(node);
            Assert.IsTrue(node is IUriNode);
            Assert.AreEqual(expected, (node as IUriNode).Uri.AbsoluteUri);
            _logicalRow.VerifyAll();
            _termMap.VerifyAll();
            _termType.VerifyAll();
            _log.Verify(log => log.LogTermGenerated(node));
        }

        [Test, Ignore]
        public void InvalidUriValueThrowsException()
        {
            // given
            _logicalRow.Setup(rec => rec.GetOrdinal(ColumnName)).Returns(ColumnIndex).Verifiable();
            _logicalRow.Setup(rec => rec.IsDBNull(ColumnIndex)).Returns(false).Verifiable();
            Uri datatype;
            _lexicalFormProvider.Setup(lex => lex.GetLexicalForm(ColumnIndex, It.IsAny<IDataRecord>(), out datatype))
                                .Returns("../../value")
                                .Verifiable();
            _termMap.Setup(map => map.IsColumnValued).Returns(true).Verifiable();
            _termMap.Setup(map => map.ColumnName).Returns(ColumnName).Verifiable();
            _termMap.Setup(map => map.BaseURI).Returns(new Uri("http://www.example.com/")).Verifiable();
            _termType.Setup(type => type.IsURI).Returns(true).Verifiable();

            // when
            Assert.Throws<InvalidTermException>(() => _termGenerator.GenerateTerm<INode>(_termMap.Object, _logicalRow.Object));

            // then
            _logicalRow.VerifyAll();
            _termMap.VerifyAll();
            _termType.VerifyAll();
        }

        #endregion

        #region Literal term type

        [Test]
        public void GeneratesLiteralWithoutDatatypeOrLanguageTag()
        {
            // given
            _logicalRow.Setup(rec => rec.GetOrdinal(ColumnName)).Returns(ColumnIndex).Verifiable();
            _logicalRow.Setup(rec => rec.IsDBNull(ColumnIndex)).Returns(false).Verifiable();
            Uri datatype;
            _lexicalFormProvider.Setup(lex => lex.GetLexicalForm(ColumnIndex, It.IsAny<IDataRecord>(), out datatype))
                                .Returns("value")
                                .Verifiable();
            _objectMap.Setup(map => map.IsColumnValued).Returns(true).Verifiable();
            _objectMap.Setup(map => map.ColumnName).Returns(ColumnName).Verifiable();
            _objectMap.Setup(map => map.TermType).Returns(_termType.Object).Verifiable();
            _termType.Setup(type => type.IsLiteral).Returns(true).Verifiable();

            // when
            var node = _termGenerator.GenerateTerm<INode>(_objectMap.Object, _logicalRow.Object);

            // then
            Assert.IsNotNull(node);
            Assert.IsTrue(node is ILiteralNode);
            Assert.AreEqual("value", (node as ILiteralNode).Value);
            _logicalRow.VerifyAll();
            _objectMap.VerifyAll();
            _termType.VerifyAll();
            _log.Verify(log => log.LogTermGenerated(node));
        }

        [Test]
        public void GeneratesLiteralWithDatatype()
        {
            // given
            const string expectedDatatype = "http://www.example.com/types/inch";
            _logicalRow.Setup(rec => rec.GetOrdinal(ColumnName)).Returns(ColumnIndex).Verifiable();
            _logicalRow.Setup(rec => rec.IsDBNull(ColumnIndex)).Returns(false).Verifiable();
            Uri datatype;
            _lexicalFormProvider.Setup(lex => lex.GetLexicalForm(ColumnIndex, It.IsAny<IDataRecord>(), out datatype))
                                .Returns("value")
                                .Verifiable();
            _objectMap.Setup(map => map.IsColumnValued).Returns(true).Verifiable();
            _objectMap.Setup(map => map.ColumnName).Returns(ColumnName).Verifiable();
            _objectMap.Setup(map => map.TermType).Returns(_termType.Object).Verifiable();
            _objectMap.Setup(map => map.DataTypeURI).Returns(new Uri(expectedDatatype)).Verifiable();
            _termType.Setup(type => type.IsLiteral).Returns(true).Verifiable();

            // when
            var node = _termGenerator.GenerateTerm<INode>(_objectMap.Object, _logicalRow.Object);

            // then
            Assert.IsNotNull(node);
            Assert.IsTrue(node is ILiteralNode);
            Assert.AreEqual("value", (node as ILiteralNode).Value);
            Assert.AreEqual(expectedDatatype, (node as ILiteralNode).DataType.AbsoluteUri);
            _logicalRow.VerifyAll();
            _objectMap.VerifyAll();
            _termType.VerifyAll();
            _log.Verify(log => log.LogTermGenerated(node));
        }

        [Test]
        public void GeneratesLiteralWithLanguageTag()
        {
            // given
            _logicalRow.Setup(rec => rec.GetOrdinal(ColumnName)).Returns(ColumnIndex).Verifiable();
            _logicalRow.Setup(rec => rec.IsDBNull(ColumnIndex)).Returns(false).Verifiable();
            Uri datatype;
            _lexicalFormProvider.Setup(lex => lex.GetLexicalForm(ColumnIndex, It.IsAny<IDataRecord>(), out datatype))
                                .Returns("value")
                                .Verifiable();
            _objectMap.Setup(map => map.IsColumnValued).Returns(true).Verifiable();
            _objectMap.Setup(map => map.ColumnName).Returns(ColumnName).Verifiable();
            _objectMap.Setup(map => map.TermType).Returns(_termType.Object).Verifiable();
            _objectMap.Setup(map => map.LanguageTag).Returns("pl").Verifiable();
            _termType.Setup(type => type.IsLiteral).Returns(true).Verifiable();

            // when
            var node = _termGenerator.GenerateTerm<INode>(_objectMap.Object, _logicalRow.Object);

            // then
            Assert.IsNotNull(node);
            Assert.IsTrue(node is ILiteralNode);
            Assert.AreEqual("value", (node as ILiteralNode).Value);
            Assert.AreEqual("pl", (node as ILiteralNode).Language);
            _logicalRow.VerifyAll();
            _objectMap.VerifyAll();
            _termType.VerifyAll();
            _log.Verify(log => log.LogTermGenerated(node));
        }

        [Test]
        public void CannotGenerateLiteralWithBothLanguageTagAndDatatype()
        {
            // given
            _logicalRow.Setup(rec => rec.GetOrdinal(ColumnName)).Returns(ColumnIndex).Verifiable();
            _logicalRow.Setup(rec => rec.IsDBNull(ColumnIndex)).Returns(false).Verifiable();
            Uri datatype;
            _lexicalFormProvider.Setup(lex => lex.GetLexicalForm(ColumnIndex, It.IsAny<IDataRecord>(), out datatype))
                                .Returns("value")
                                .Verifiable();
            _objectMap.Setup(map => map.IsColumnValued).Returns(true).Verifiable();
            _objectMap.Setup(map => map.ColumnName).Returns(ColumnName).Verifiable();
            _objectMap.Setup(map => map.TermType).Returns(_termType.Object).Verifiable();
            _objectMap.Setup(map => map.LanguageTag).Returns("pl").Verifiable();
            _objectMap.Setup(map => map.DataTypeURI)
                      .Returns(new Uri("http://www.example.com/types/inch"))
                      .Verifiable();
            _termType.Setup(type => type.IsLiteral).Returns(true).Verifiable();

            // when
            Assert.Throws<InvalidTermException>(() => _termGenerator.GenerateTerm<INode>(_objectMap.Object, _logicalRow.Object));

            // then
            _logicalRow.VerifyAll();
            _objectMap.VerifyAll();
            _termType.VerifyAll();
        }

        #endregion

        #endregion

        #region Test template-valued term map

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void ThrowsIsTemplateIsNotSet(string template)
        {
            // given
            _termMap = new Mock<ITermMap>();
            _termMap.Setup(map => map.IsTemplateValued).Returns(true);
            _termMap.Setup(map => map.Template).Returns(template);

            // when
            Assert.Throws<InvalidTemplateException>(() => _termGenerator.GenerateTerm<INode>(_termMap.Object, _logicalRow.Object));

            // then
            _termMap.VerifyAll();
        }

        [Test]
        public void NullValueReturnsNullNodeForTemplateTermMap()
        {
            // given
            _termMap = new Mock<ITermMap>();
            _logicalRow.Setup(rec => rec.GetOrdinal("id")).Returns(ColumnIndex).Verifiable();
            _logicalRow.Setup(rec => rec.IsDBNull(ColumnIndex)).Returns(true).Verifiable();
            _termMap.Setup(map => map.IsTemplateValued).Returns(true);
            _termMap.Setup(map => map.Template).Returns("http://www.example.com/person/{id}");
            _termMap.Setup(map => map.TermType).Returns(_termType.Object);
            _termType.Setup(tt => tt.IsURI).Returns(false);

            // when
            var node = _termGenerator.GenerateTerm<INode>(_termMap.Object, _logicalRow.Object);

            // then
            Assert.IsNull(node);
            _logicalRow.VerifyAll();
            _termMap.VerifyAll();
            _log.Verify(log => log.LogNullTermGenerated(_termMap.Object));
        }

        [Test]
        public void AnyNullValueReturnsNullNodeForTemplateTermMap()
        {
            // given
            _termMap = new Mock<ITermMap>();
            _logicalRow.Setup(rec => rec.GetOrdinal("id")).Returns(1).Verifiable();
            _logicalRow.Setup(rec => rec.GetOrdinal("name")).Returns(2).Verifiable();
            _logicalRow.Setup(rec => rec.IsDBNull(1)).Returns(false).Verifiable();
            _logicalRow.Setup(rec => rec.IsDBNull(2)).Returns(true).Verifiable();
            _termMap.Setup(map => map.IsTemplateValued).Returns(true);
            _termMap.Setup(map => map.Template).Returns("http://www.example.com/person/{id}/{name}");
            _termMap.Setup(map => map.TermType).Returns(_termType.Object);
            _termType.Setup(tt => tt.IsURI).Returns(true);
            Uri typeUri;
            _lexicalFormProvider.Setup(lex => lex.GetLexicalForm(1, It.IsAny<IDataRecord>(), out typeUri)).Returns("value");

            // when
            var node = _termGenerator.GenerateTerm<INode>(_termMap.Object, _logicalRow.Object);

            // then
            Assert.IsNull(node);
            _lexicalFormProvider.VerifyAll();
            _logicalRow.VerifyAll();
            _termMap.VerifyAll();
            _log.Verify(log => log.LogNullTermGenerated(_termMap.Object));
        }

        [Test]
        public void ReplacesColumnNameWithValuesForLiteralTemplatedTermMap()
        {
            // given
            _logicalRow.Setup(rec => rec.GetOrdinal("id")).Returns(1).Verifiable();
            _logicalRow.Setup(rec => rec.GetOrdinal("name")).Returns(2).Verifiable();
            _logicalRow.Setup(rec => rec.IsDBNull(It.IsAny<int>())).Returns(false).Verifiable();
            _objectMap.Setup(map => map.IsTemplateValued).Returns(true);
            _objectMap.Setup(map => map.Template).Returns("http://www.example.com/person/{id}/{name}");
            _objectMap.Setup(map => map.TermType).Returns(_termType.Object);
            _termType.Setup(tt => tt.IsLiteral).Returns(true).Verifiable();
            _termType.Setup(tt => tt.IsURI).Returns(false).Verifiable();
            Uri type;
            _lexicalFormProvider.Setup(lex => lex.GetLexicalForm(1, It.IsAny<IDataRecord>(), out type)).Returns("5").Verifiable();
            _lexicalFormProvider.Setup(lex => lex.GetLexicalForm(2, It.IsAny<IDataRecord>(), out type)).Returns("Tomasz Pluskiewicz").Verifiable();

            // when
            var node = _termGenerator.GenerateTerm<INode>(_objectMap.Object, _logicalRow.Object);

            // then
            Assert.IsNotNull(node);
            Assert.IsTrue(node is ILiteralNode);
            Assert.AreEqual("http://www.example.com/person/5/Tomasz%20Pluskiewicz", (node as ILiteralNode).Value);
            _logicalRow.VerifyAll();
            _logicalRow.Verify();
            _objectMap.VerifyAll();
            _termType.VerifyAll();
        }

        [Test]
        public void ReplacesAndEscapesColumnNameWithValuesForIRITemplatedTermMap()
        {
            // given
            _logicalRow.Setup(rec => rec.GetOrdinal("id")).Returns(1).Verifiable();
            _logicalRow.Setup(rec => rec.GetOrdinal("name")).Returns(2).Verifiable();
            _logicalRow.Setup(rec => rec.IsDBNull(It.IsAny<int>())).Returns(false).Verifiable();
            _termMap.Setup(map => map.IsTemplateValued).Returns(true);
            _termMap.Setup(map => map.Template).Returns("http://www.example.com/person/{id}/{name}");
            _termType.Setup(tt => tt.IsURI).Returns(true).Verifiable();
            Uri type;
            _lexicalFormProvider.Setup(lex => lex.GetLexicalForm(1, It.IsAny<IDataRecord>(), out type)).Returns("5").Verifiable();
            _lexicalFormProvider.Setup(lex => lex.GetLexicalForm(2, It.IsAny<IDataRecord>(), out type)).Returns("Tomasz Pluskiewicz").Verifiable();

            // when
            var node = _termGenerator.GenerateTerm<INode>(_termMap.Object, _logicalRow.Object);

            // then
            Assert.IsNotNull(node);
            Assert.IsTrue(node is IUriNode);
            Assert.AreEqual(new Uri("http://www.example.com/person/5/Tomasz%20Pluskiewicz"), (node as IUriNode).Uri);
            _logicalRow.VerifyAll();
            _logicalRow.Verify();
            _termMap.VerifyAll();
            _termType.VerifyAll();
        }

        #endregion

        [TestCase("0number_first")]
        [TestCase("has spaces")]
        [TestCase("-non_letter_first")]
        public void LogsPotentiallyInvalidBlankNodeIds(string identifier)
        {
            // given
            _termMap.Setup(map => map.TermType.IsBlankNode).Returns(true);

            // when
            _termGenerator.GenerateTermForValue(_termMap.Object, identifier);

            // then
            _log.Verify(log => log.LogInvalidBlankNode(_termMap.Object, identifier));
        }

        [Test]
        public void ThrowsWhenSubjectMapIsLiteral()
        {
            // given
            _subjectMap.Setup(map => map.IsColumnValued).Returns(true);
            _subjectMap.Setup(map => map.ColumnName).Returns(ColumnName);
            _subjectMap.Setup(map => map.TermType.IsLiteral).Returns(true);
            _logicalRow.Setup(rec => rec.GetOrdinal(ColumnName)).Returns(ColumnIndex);
            _logicalRow.Setup(rec => rec.IsDBNull(ColumnIndex)).Returns(false);

            // then
            Assert.Throws<InvalidTermException>(() => _termGenerator.GenerateTerm<INode>(_subjectMap.Object, _logicalRow.Object));
        }

        [Test]
        public void ThrowsWhenGraphMapIsNonIri()
        {
            // given
            _graphMap = new Mock<IGraphMap>();
            _graphMap.Setup(map => map.IsColumnValued).Returns(true);
            _graphMap.Setup(map => map.ColumnName).Returns(ColumnName);
            _graphMap.Setup(map => map.TermType.IsURI).Returns(false);
            _graphMap.Setup(map => map.TermType.IsLiteral).Returns(true);
            _logicalRow.Setup(rec => rec.GetOrdinal(ColumnName)).Returns(ColumnIndex);
            _logicalRow.Setup(rec => rec.IsDBNull(ColumnIndex)).Returns(false);

            // then
            Assert.Throws<InvalidTermException>(() => _termGenerator.GenerateTerm<INode>(_graphMap.Object, _logicalRow.Object));
        }

        [Test]
        public void ThrowsWhenTermIsNeitherColumnNorTemplateNorConstantValue()
        {
            // given
            _termMap.Setup(tm => tm.IsConstantValued).Returns(false);
            _termMap.Setup(tm => tm.IsTemplateValued).Returns(false);
            _termMap.Setup(tm => tm.IsColumnValued).Returns(false);

            // then
            Assert.Throws<InvalidTermException>(() => _termGenerator.GenerateTerm<INode>(_termMap.Object, _logicalRow.Object));
        }

        [Test]
        public void ByDefaultDoesntPreserveDuplicateRows()
        {
            Assert.IsFalse(_termGenerator.PreserveDuplicateRows);
        }

        [Test]
        public void RetrunsDifferentSubjectBlankNodesForSameValues()
        {
            // given
            _termGenerator = new RDFTermGenerator(true);
            const string nodeId = "node id";
            _subjectMap.Setup(sm => sm.TermType.IsBlankNode).Returns(true);

            // when
            var node = _termGenerator.GenerateTermForValue(_subjectMap.Object, nodeId);
            var node2 = _termGenerator.GenerateTermForValue(_subjectMap.Object, nodeId);

            // then
            Assert.AreNotSame(node, node2);
        }

        [Test]
        public void RetrunsSameSubjectBlankNodesForSameValuesWhenPreservingDuplicateRows()
        {
            // given
            const string nodeId = "node id";
            _subjectMap.Setup(sm => sm.TermType.IsBlankNode).Returns(true);
            _termGenerator = new RDFTermGenerator(false);

            // when
            var node = _termGenerator.GenerateTermForValue(_subjectMap.Object, nodeId);
            var node2 = _termGenerator.GenerateTermForValue(_subjectMap.Object, nodeId);

            // then
            Assert.AreSame(node, node2);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ReusesNodesForObjectsAndSubjectsWhenNotPreservingDulplicateRows(bool subjectFirst)
        {
            // given
            const string nodeId = "node id";
            _subjectMap.Setup(sm => sm.TermType.IsBlankNode).Returns(true);
            _objectMap.Setup(sm => sm.TermType.IsBlankNode).Returns(true);
            _termGenerator = new RDFTermGenerator();

            // when
            INode node2;
            INode node;
            if (subjectFirst)
            {
                node = _termGenerator.GenerateTermForValue(_subjectMap.Object, nodeId);
                node2 = _termGenerator.GenerateTermForValue(_objectMap.Object, nodeId);
            }
            else
            {
                node2 = _termGenerator.GenerateTermForValue(_objectMap.Object, nodeId);
                node = _termGenerator.GenerateTermForValue(_subjectMap.Object, nodeId);
            }

            // then
            Assert.AreSame(node, node2);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ReusesNodesForObjectsAndSubjectsWhenPreservingDulplicateRows(bool subjectFirst)
        {
            // given
            const string nodeId = "node id";
            _subjectMap.Setup(sm => sm.TermType.IsBlankNode).Returns(true);
            _objectMap.Setup(sm => sm.TermType.IsBlankNode).Returns(true);

            // when
            INode node2;
            INode node;
            if (subjectFirst)
            {
                node = _termGenerator.GenerateTermForValue(_subjectMap.Object, nodeId);
                node2 = _termGenerator.GenerateTermForValue(_objectMap.Object, nodeId);
            }
            else
            {
                node2 = _termGenerator.GenerateTermForValue(_objectMap.Object, nodeId);
                node = _termGenerator.GenerateTermForValue(_subjectMap.Object, nodeId);
            }

            // then
            Assert.AreSame(node, node2);
        }
    }
}