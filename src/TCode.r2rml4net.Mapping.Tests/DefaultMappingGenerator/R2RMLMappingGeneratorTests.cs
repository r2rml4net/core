﻿using System;
using Moq;
using NUnit.Framework;
using TCode.r2rml4net.Mapping.DirectMapping;
using TCode.r2rml4net.Mapping.Tests.Mocks;
using TCode.r2rml4net.RDB;

namespace TCode.r2rml4net.Mapping.Tests.DefaultMappingGenerator
{
    [TestFixture]
    public class R2RMLMappingGeneratorTests
    {
        private R2RMLMappingGenerator _generator;
        private Mock<IR2RMLConfiguration> _configuration;
        private Mock<IDatabaseMetadata> _databaseMetedata;
        private Mock<ISqlQueryBuilder> _sqlBuilder;
        private Mock<IDirectMappingStrategy> _mappingStrategy;
        private readonly Uri _mappingBaseUri = new Uri("http://base.uri/");

        [SetUp]
        public void Setup()
        {
            _configuration = new Mock<IR2RMLConfiguration>();
            _databaseMetedata = new Mock<IDatabaseMetadata>();
            _sqlBuilder = new Mock<ISqlQueryBuilder>();
            _mappingStrategy = new Mock<IDirectMappingStrategy>();
            _generator = new R2RMLMappingGenerator(_databaseMetedata.Object, _configuration.Object)
            {
                MappingBaseUri = _mappingBaseUri,
                SqlBuilder = _sqlBuilder.Object,
                MappingStrategy = _mappingStrategy.Object
            };
            _configuration.Setup(c => c.CreateTriplesMapFromTable(It.IsAny<string>()))
                          .Returns(new MockConfiguration(_mappingBaseUri, _configuration.Object));
            _configuration.Setup(c => c.CreateTriplesMapFromR2RMLView(It.IsAny<string>()))
                          .Returns(new MockConfiguration(_mappingBaseUri, _configuration.Object));
        }

        [Test]
        public void CreatesTriplesMapFromTableWithPrimaryKey()
        {
            //given
            const string tableName = "Country Info";
            var table = RelationalTestMappings.D010_1table1primarykey3colums[tableName];

            // when
            _generator.Visit(table);

            // then
            _configuration.Verify(conf => conf.CreateTriplesMapFromTable(tableName), Times.Once());
        }

        [Test]
        public void CreatesTriplesMapFromTableWithoutPrimaryKey()
        {
            //given
            const string tableName = "Student";
            var table = RelationalTestMappings.D001_1table1column[tableName];

            // when
            _generator.Visit(table);

            // then
            _configuration.Verify(conf => conf.CreateTriplesMapFromTable(tableName), Times.Once());
        }

        [Test]
        public void CreatesTriplesMapFromTableWithCandidateKeyReference()
        {
            //given
            const string tableName = "EMP";
            var table = RelationalTestMappings.D014_3tables1primarykey1foreignkey[tableName];

            // when
            _generator.Visit(table);

            // then
            _configuration.Verify(conf => conf.CreateTriplesMapFromTable(tableName), Times.Once());
        }

        [TestCase(true)]
        [TestCase(false)]
        public void CreatesTriplesMapFromSqlViewForTableWithCandidateKeyReferenceReferencingTablePrimaryKey(bool isCandidateKeyRef)
        {
            //given
            const string expectedQuery = @"SELECT something";
            var referencedTable = new TableMetadata
                {
                    new ColumnMetadata {Name = "Id", IsPrimaryKey = true},
                    new ColumnMetadata {Name = "Id2", IsPrimaryKey = true},
                    new ColumnMetadata {Name = "unique1"},
                    new ColumnMetadata {Name = "unique2"}
                };
            referencedTable.Name = "Referenced";
            var table = new TableMetadata
                {
                    ForeignKeys = new[]
                        {
                            new ForeignKeyMetadata
                                {
                                    TableName = "Referencing",
                                    IsCandidateKeyReference = isCandidateKeyRef,
                                    ReferencedTableHasPrimaryKey = true,
                                    ReferencedColumns = new[] {"unique1", "unique2"},
                                    ForeignKeyColumns = new[] {"fk1", "fk2"},
                                    ReferencedTable = referencedTable,
                                }
                        }
                };
            table.Name = "TableName";
            _sqlBuilder.Setup(sql => sql.GetR2RMLViewForJoinedTables(table)).Returns(expectedQuery);

            // when
            _generator.Visit(table);

            // then
            if (isCandidateKeyRef)
            {
                _sqlBuilder.Verify(sql => sql.GetR2RMLViewForJoinedTables(table), Times.Once()); 
                _configuration.Verify(conf => conf.CreateTriplesMapFromR2RMLView(expectedQuery), Times.Once());
            }
            else
                _configuration.Verify(conf => conf.CreateTriplesMapFromTable("TableName"), Times.Once());
        }

        [Test]
        public void CreatesUriTemplateIfCandidateForeignKeyTargetHasPrimaryKey()
        {
            // given 
            TableMetadata referenced = new TableMetadata
                {
                    new ColumnMetadata { Name = "Id" }
                };
            referenced.Name = "target";
            var fk = new ForeignKeyMetadata
                {
                    ReferencedColumns = new[] { "unique1", "unique2" },
                    ForeignKeyColumns = new[] { "candidate1", "candidate2" },
                    IsCandidateKeyReference = true,
                    ReferencedTableHasPrimaryKey = true,
                    TableName = "source",
                    ReferencedTable = referenced
                };
            _generator.CurrentTriplesMapConfiguration = new MockConfiguration(_mappingBaseUri, _configuration.Object);

            // when
            _generator.Visit(fk);

            // then
            _mappingStrategy.Verify(ms =>
                                    ms.CreateObjectMapForPrimaryKeyReference(
                                        It.IsAny<IObjectMapConfiguration>(),
                                        _mappingBaseUri,
                                        fk),
                                    Times.Once());
        }
    }
}