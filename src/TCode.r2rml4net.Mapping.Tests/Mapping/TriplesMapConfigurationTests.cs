﻿using System;
using Moq;
using NUnit.Framework;
using TCode.r2rml4net.Exceptions;
using TCode.r2rml4net.RDB;
using TCode.r2rml4net.Validation;
using VDS.RDF;

namespace TCode.r2rml4net.Mapping.Tests.Mapping
{
    [TestFixture]
    public class TriplesMapConfigurationTests
    {
        private Mock<ISqlVersionValidator> _sqlVersionValidator;
        TriplesMapConfiguration _triplesMapConfiguration;
        private Mock<IR2RMLConfiguration> _r2RMLConfiguration;
        private IGraph _r2RMLMappings;
        private Mock<ISqlVersionValidator> _sqlValidator;

        private TriplesMapConfigurationStub CreatStub()
        {
            _sqlValidator = new Mock<ISqlVersionValidator>();
            _sqlValidator.Setup(v => v.SqlVersionIsValid(It.IsAny<Uri>())).Returns(true);
            _r2RMLConfiguration.Setup(config => config.SqlVersionValidator).Returns(_sqlValidator.Object);
            return new TriplesMapConfigurationStub(_r2RMLConfiguration.Object, _r2RMLMappings, new MappingOptions(),
                                                   _sqlVersionValidator.Object);
        }

        [SetUp]
        public void Setup()
        {
            // Initialize TriplesMapConfiguration with a default graph
            var r2RMLConfiguration = new R2RMLConfiguration();
            _r2RMLConfiguration = new Mock<IR2RMLConfiguration>();
            _r2RMLMappings = r2RMLConfiguration.R2RMLMappings;
            _sqlVersionValidator = new Mock<ISqlVersionValidator>(MockBehavior.Strict);
            _triplesMapConfiguration = new TriplesMapConfiguration(CreatStub(), _r2RMLMappings.CreateBlankNode());
        }

        [Test]
        public void SqlQueryIsSetAsIs()
        {
            // given
            const string query = "SELECT * from Table";

            // when
            _triplesMapConfiguration = TriplesMapConfiguration.FromSqlQuery(CreatStub(), query);

            // then
            Assert.AreEqual(query, _triplesMapConfiguration.SqlQuery);
            Assert.IsNull(_triplesMapConfiguration.TableName);
        }

        [TestCase("TableName", "TableName")]
        [TestCase("[TableName]", "TableName")]
        [TestCase("[Table1Name]", "Table1Name")]
        [TestCase("'TableName'", "TableName")]
        [TestCase("`TableName`", "TableName")]
        [TestCase("`Table12Name`", "Table12Name")]
        [TestCase("\"TableName\"", "TableName")]
        public void TriplesMapTableNameShouldBeTrimmed(string tableName, string expected)
        {
            // when
            _triplesMapConfiguration = TriplesMapConfiguration.FromTable(CreatStub(), tableName);

            // then
            Assert.AreEqual(expected, _triplesMapConfiguration.TableName);
            Assert.IsNull(_triplesMapConfiguration.SqlQuery);
        }

        [TestCase("`Schema`.`TableName`")]
        [TestCase("[Schema].[TableName]")]
        [TestCase("[Schema].[TableName]")]
        [TestCase("Schema.[TableName]")]
        [TestCase("Schema.`TableName`")]
        public void TriplesMapTableNameCanContainSchema(string tableName)
        {
            // when
            _triplesMapConfiguration = TriplesMapConfiguration.FromTable(CreatStub(), tableName);

            // then
            Assert.AreEqual("Schema.TableName", _triplesMapConfiguration.TableName);
            Assert.IsNull(_triplesMapConfiguration.SqlQuery);
        }

        [TestCase("[Database].[Schema].[TableName]")]
        [TestCase("Database.[Schema].[TableName]")]
        [TestCase("`Database`.`Schema`.`TableName`")]
        [TestCase("Database.`Schema`.`TableName`")]
        public void TriplesMapTableNameCanContainSchemaAndDatabaseName(string tableName)
        {
            // when
            _triplesMapConfiguration = TriplesMapConfiguration.FromTable(CreatStub(), tableName);

            // then
            Assert.AreEqual("Database.Schema.TableName", _triplesMapConfiguration.TableName);
            Assert.IsNull(_triplesMapConfiguration.SqlQuery);
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentOutOfRangeException))]
        public void CannotCreateTriplesMapFromEmptyOrNullTableName(string tableName)
        {
            _triplesMapConfiguration = TriplesMapConfiguration.FromTable(CreatStub(), tableName);
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentOutOfRangeException))]
        public void CannotCreateTriplesMapFromEmptyOrNullSqlQuery(string sqlQuery)
        {
            _triplesMapConfiguration = TriplesMapConfiguration.FromSqlQuery(CreatStub(), sqlQuery);
        }

        [Test]
        public void CanSetSqlVersionTwice()
        {
            // given
            var mssql = new Uri("http://www.w3.org/ns/r2rml#MSSQL");
            var coreSql2008 = new Uri("http://www.w3.org/ns/r2rml#SQL2008");
            _sqlVersionValidator.Setup(val => val.SqlVersionIsValid(coreSql2008)).Returns(true);
            _sqlVersionValidator.Setup(val => val.SqlVersionIsValid(mssql)).Returns(true);
            _triplesMapConfiguration = TriplesMapConfiguration.FromSqlQuery(CreatStub(), "SELECT * from X");

            // when
            _triplesMapConfiguration.SetSqlVersion(coreSql2008)
                                    .SetSqlVersion(mssql);

            // then
            _triplesMapConfiguration.R2RMLMappings.VerifyHasTripleWithBlankSubject("http://www.w3.org/ns/r2rml#sqlVersion", "http://www.w3.org/ns/r2rml#SQL2008");
            _triplesMapConfiguration.R2RMLMappings.VerifyHasTripleWithBlankSubject("http://www.w3.org/ns/r2rml#sqlVersion", "http://www.w3.org/ns/r2rml#MSSQL");
        }

        [Test]
        public void CanSetSameSqlVersionTwice()
        {
            // given
            var sqlVersion = new Uri("http://www.w3.org/ns/r2rml#SQL2008");
            _sqlVersionValidator.Setup(val => val.SqlVersionIsValid(sqlVersion)).Returns(true);
            _triplesMapConfiguration = TriplesMapConfiguration.FromSqlQuery(CreatStub(), "SELECT * from X");

            // when
            _triplesMapConfiguration.SetSqlVersion(sqlVersion)
                                    .SetSqlVersion(sqlVersion);

            // then
            _triplesMapConfiguration.R2RMLMappings.VerifyHasTripleWithBlankSubject("http://www.w3.org/ns/r2rml#sqlVersion", "http://www.w3.org/ns/r2rml#SQL2008", 2);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void CanSetSqlVersion(bool fromUriString)
        {
            // given
            const string sqlVersionString = "http://www.w3.org/ns/r2rml#SQL2008";
            Uri sqlVersion = new Uri(sqlVersionString);
            _sqlVersionValidator.Setup(val => val.SqlVersionIsValid(sqlVersion)).Returns(true);
            _triplesMapConfiguration = TriplesMapConfiguration.FromSqlQuery(CreatStub(), "SELECT * from X");

            // when
            if (fromUriString)
                _triplesMapConfiguration.SetSqlVersion(sqlVersionString);
            else
                _triplesMapConfiguration.SetSqlVersion(sqlVersion);

            // then
            if (fromUriString)
                _triplesMapConfiguration.R2RMLMappings.VerifyHasTripleWithBlankSubject("http://www.w3.org/ns/r2rml#sqlVersion", sqlVersionString);
            else
                _triplesMapConfiguration.R2RMLMappings.VerifyHasTripleWithBlankSubject("http://www.w3.org/ns/r2rml#sqlVersion", sqlVersion);

            Assert.Contains(sqlVersion, _triplesMapConfiguration.SqlVersions);
        }

        [Test]
        public void CannotSetSqlVersionWhenTableNameHasAlreadyBeenSet()
        {
            // given
            _triplesMapConfiguration = TriplesMapConfiguration.FromTable(CreatStub(), "Table");

            // then
            Assert.Throws<InvalidTriplesMapException>(
                () => _triplesMapConfiguration.SetSqlVersion(new Uri("http://www.w3.org/ns/r2rml#SQL2008"))
            );
        }

        [Test]
        public void ByDefaultCannotSetInvalidSqlVersion()
        {
            // given
            var sqlVersion = new Uri("http://no-such-identifier.com");
            _sqlVersionValidator.Setup(v => v.SqlVersionIsValid(sqlVersion)).Returns(false);
            TriplesMapConfigurationStub stub = new TriplesMapConfigurationStub(_r2RMLConfiguration.Object,
                                                                               _r2RMLMappings,
                                                                               new MappingOptions(),
                                                                               _sqlVersionValidator.Object);
            _r2RMLConfiguration.Setup(config => config.SqlVersionValidator).Returns(_sqlVersionValidator.Object);
            _triplesMapConfiguration = TriplesMapConfiguration.FromSqlQuery(stub, "SELECT * FROM Table");

            // when
            Assert.Throws<InvalidSqlVersionException>(() => _triplesMapConfiguration.SetSqlVersion(sqlVersion));

            // then
            _sqlVersionValidator.Verify(v => v.SqlVersionIsValid(sqlVersion), Times.Once());
        }

        [Test]
        public void CanUseSettingToDisableSqlVersionValidation()
        {
            // given
            var sqlVersion = new Uri("http://no-such-identifier.com");
            TriplesMapConfigurationStub stub = new TriplesMapConfigurationStub(_r2RMLConfiguration.Object,
                                                                               _r2RMLMappings,
                                                                               new MappingOptions
                                                                                   {
                                                                                       ValidateSqlVersion = false
                                                                                   },
                                                                               _sqlVersionValidator.Object);
            _triplesMapConfiguration = TriplesMapConfiguration.FromSqlQuery(stub, "SELECT * FROM Table");

            // when
            Assert.DoesNotThrow(() => _triplesMapConfiguration.SetSqlVersion(sqlVersion));

            // then
            Assert.Contains(sqlVersion, _triplesMapConfiguration.SqlVersions);
        }

        [Test]
        public void CanCreateSubjectMaps()
        {
            // given
            _triplesMapConfiguration = TriplesMapConfiguration.FromTable(CreatStub(), "Table");

            // when
            var subjectMapConfiguration = (ISubjectMapConfiguration)_triplesMapConfiguration.SubjectMap;

            // then
            Assert.IsNotNull(subjectMapConfiguration);
            Assert.IsInstanceOf<TermMapConfiguration>(subjectMapConfiguration);
            Assert.IsInstanceOf<ITermMapConfiguration>(subjectMapConfiguration);
        }

        [Test]
        public void SubjectMapAlwaysReturnsSameInstance()
        {
            // given 
            _triplesMapConfiguration = TriplesMapConfiguration.FromTable(CreatStub(), "Table");
            var subjectMapConfiguration = (ISubjectMapConfiguration)_triplesMapConfiguration.SubjectMap;

            // when
            var shouldBeTheSame = (ISubjectMapConfiguration)_triplesMapConfiguration.SubjectMap;

            // then
            Assert.AreSame(subjectMapConfiguration, shouldBeTheSame);
        }

        [Test]
        public void CanCreatePropertyObjectMap()
        {
            // given
            _triplesMapConfiguration = TriplesMapConfiguration.FromTable(CreatStub(), "Table");

            // when
            IPredicateObjectMapConfiguration predicateObjectMap = _triplesMapConfiguration.CreatePropertyObjectMap();

            // then
            Assert.IsNotNull(predicateObjectMap);
            Assert.IsInstanceOf<PredicateObjectMapConfiguration>(predicateObjectMap);
        }

        [Test]
        public void UsesEffectiveSqlBuilder()
        {
            // given
            const string excpetedSql = "SELECT * FROM (SELECT * FROM A) AS tmp";
            Mock<ISqlQueryBuilder> sqlBuilder = new Mock<ISqlQueryBuilder>();
            sqlBuilder.Setup(builder => builder.GetEffectiveQueryForTriplesMap(It.IsAny<ITriplesMap>()))
                      .Returns(excpetedSql);
            _r2RMLConfiguration.Setup(config => config.SqlQueryBuilder).Returns(sqlBuilder.Object);

            // when
            string sql = _triplesMapConfiguration.EffectiveSqlQuery;

            // then
            Assert.AreEqual(excpetedSql, sql);
        }

        [Test]
        public void BlankNodeTriplesMapsDontInterfereWithEachOther()
        {
            // given
            var fromTable = TriplesMapConfiguration.FromTable(CreatStub(), "Table");
            var fromSqlQuery = TriplesMapConfiguration.FromSqlQuery(CreatStub(), "SElECT * FROM y");

            // then
            Assert.IsNull(fromTable.SqlQuery);
            Assert.IsNull(fromSqlQuery.TableName);
            Assert.AreEqual("Table", fromTable.TableName);
            Assert.AreEqual("SElECT * FROM y", fromSqlQuery.SqlQuery);
        }
    }
}
