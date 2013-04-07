﻿#region Licence
// Copyright (C) 2012 Tomasz Pluskiewicz
// http://r2rml.net/
// r2rml@r2rml.net
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
using NUnit.Framework;
using Moq;
using TCode.r2rml4net.Mapping.DirectMapping;
using TCode.r2rml4net.RDB;
using VDS.RDF;
using System.IO;
using VDS.RDF.Writing;

namespace TCode.r2rml4net.Mapping.Tests.DefaultMappingGenerator
{
    [TestFixture]
    public class R2RMLMappingGeneratorCompleteCaseTests
    {
        private R2RMLMappingGenerator _r2RMLMappingGenerator;
        private Mock<IDatabaseMetadata> _databaseMetedata;
        private R2RMLConfiguration _configuration;

        [SetUp]
        public void Setup()
        {
            _databaseMetedata = new Mock<IDatabaseMetadata>();
            _configuration = new R2RMLConfiguration(new Uri("http://example.com/"));
            _r2RMLMappingGenerator = new R2RMLMappingGenerator(_databaseMetedata.Object, _configuration);
        }

        [Test]
        public void CreatedWithDefaultGenerationAlgorithm()
        {
            Assert.IsTrue(_r2RMLMappingGenerator.MappingStrategy is DirectMappingStrategy);
        }

        [Test, Description("Building graph visits the table collection")]
        public void BuildingGraphReadsTablesCollection()
        {
            // given
            var tables = new TableCollection();
            _databaseMetedata.Setup(db => db.Tables).Returns(tables);

            // when
            _r2RMLMappingGenerator.GenerateMappings();

            // then
            _databaseMetedata.Verify(db => db.Tables, Times.Exactly(2));
            Assert.IsTrue(_configuration.GraphReadOnly.IsEmpty);
        }

        private void TestMappingGeneration(TableCollection tables, string embeddedResourceGraph)
        {
            // given;
            _databaseMetedata.Setup(meta => meta.Tables).Returns(tables);

            // when 
            _r2RMLMappingGenerator.GenerateMappings();

            // then
            Graph expected = new Graph();
            expected.LoadFromEmbeddedResource(string.Format("TCode.r2rml4net.Mapping.Tests.DefaultMappingGenerator.TestGraphs.{0}, TCode.r2rml4net.Mapping.Tests", embeddedResourceGraph));

            var serializedGraph = Serialize(_configuration.GraphReadOnly);
            var message = string.Format("Graphs aren't equal. Actual graph was:\r\n\r\n{0}", serializedGraph);

            var diff = expected.Difference(_configuration.GraphReadOnly);
            Assert.IsFalse(diff.AddedMSGs.Any() || diff.RemovedMSGs.Any() || diff.AddedTriples.Any() || diff.RemovedTriples.Any(), message);
        }

        [Test]
        public void SimpleTableMappingGeneration()
        {
            TestMappingGeneration(RelationalTestMappings.D001_1table1column, "R2RMLTC0001.ttl");
        }

        [Test]
        public void TypedColumnsMappingGeneration()
        {
            TestMappingGeneration(RelationalTestMappings.TypedColumns, "R2RMLTC0002.ttl");
        }

        [Test]
        public void SimpleTableWith3ColumnsMappingGeneration()
        {
            TestMappingGeneration(RelationalTestMappings.D003_1table3columns, "R2RMLTC0003.ttl");
        }

        [Test]
        public void TableWithVarcharPrimaryKey()
        {
            TestMappingGeneration(RelationalTestMappings.D006_1table1primarykey1column, "R2RMLTC0006.ttl");
        }

        [Test]
        public void TableWithCompositePrimaryKey()
        {
            TestMappingGeneration(RelationalTestMappings.D008_1table1compositeprimarykey3columns, "R2RMLTC0008.ttl");
        }

        [Test]
        public void TwoTablesWithForeignKeyReference()
        {
            TestMappingGeneration(RelationalTestMappings.D009_2tables1primarykey1foreignkey, "R2RMLTC0009.ttl");
        }

        [Test]
        public void TableWithSpacesInNames()
        {
            TestMappingGeneration(RelationalTestMappings.D010_1table1primarykey3colums, "R2RMLTC0010.ttl");
        }

        [Test]
        public void TablesWithManyToManyRelations()
        {
            TestMappingGeneration(RelationalTestMappings.D011_M2MRelations, "R2RMLTC0011.ttl");
        }

        [Test]
        public void TablesWithReferenceToCandidateKey()
        {
            TestMappingGeneration(RelationalTestMappings.D014_3tables1primarykey1foreignkey, "R2RMLTC0014.ttl");
        }

        [Test]
        public void AnotherCompositeKeyCase()
        {
            TestMappingGeneration(RelationalTestMappings.D015_1table3columns1composityeprimarykey, "R2RMLTC0015.ttl");
        }

        [Test]
        public void TableWithManyDatatypes()
        {
            TestMappingGeneration(RelationalTestMappings.D016_1table1primarykey10columnsSQLdatatypes, "R2RMLTC0016.ttl");
        }

        [Test]
        public void InternationalizedTable()
        {
            TestMappingGeneration(RelationalTestMappings.D017_I18NnoSpecialChars, "R2RMLTC0017.ttl");
        }

        [Test]
        public void TableWithCandidateKeyReferenceAndPrimaryKey()
        {
            TestMappingGeneration(RelationalTestMappings.CandidateKeyReferencsTableWithPrimaryKey, "DirectCandidatePrimary.ttl");
        }

        private string Serialize(IGraph graph) 
        {
            using (TextWriter writer = new System.IO.StringWriter())
            {
                var turtle = new CompressingTurtleWriter(10);
                turtle.Save(graph, writer);
                return writer.ToString();
            }
        }
    }
}
