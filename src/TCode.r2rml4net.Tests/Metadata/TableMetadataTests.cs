﻿using System;
using NUnit.Framework;
using TCode.r2rml4net.RDB;

namespace TCode.r2rml4net.Tests.Metadata
{
    [TestFixture]
    public class TableMetadataTests
    {
        [Test]
        public void ReturnsSinglePrimaryKey()
        {
            // given
            var primaryKeyColumn = new ColumnMetadata { Name = "OtherColumn", IsPrimaryKey = true };
            TableMetadata table = new TableMetadata
                                      {
                                          new ColumnMetadata{Name = "Column1"},
                                          primaryKeyColumn,
                                          new ColumnMetadata{Name = "YetAnotherColumn"}
                                      };

            // when
            string[] primaryKey = table.PrimaryKey;

            // then
            Assert.AreEqual(1, primaryKey.Length);
            Assert.Contains(primaryKeyColumn.Name, primaryKey);
        }

        [Test]
        public void ReturnsCompositePrimaryKey()
        {
            // given
            var primaryKeyColumn1 = new ColumnMetadata { Name = "OtherColumn", IsPrimaryKey = true };
            var primaryKeyColumn2 = new ColumnMetadata { Name = "OtherColumn2", IsPrimaryKey = true };
            var primaryKeyColumn3 = new ColumnMetadata { Name = "OtherColumn3", IsPrimaryKey = true };
            TableMetadata table = new TableMetadata
                                      {
                                          primaryKeyColumn1,
                                          primaryKeyColumn2,
                                          primaryKeyColumn3
                                      };

            // when
            string[] primaryKey = table.PrimaryKey;

            // then
            Assert.AreEqual(3, primaryKey.Length);
            Assert.Contains(primaryKeyColumn1.Name, primaryKey);
            Assert.Contains(primaryKeyColumn2.Name, primaryKey);
            Assert.Contains(primaryKeyColumn3.Name, primaryKey);
        }

        [Test]
        public void CanBeIndexed()
        {
            // given
            var column1 = new ColumnMetadata { Name = "Column1" };
            var column2 = new ColumnMetadata { Name = "Column2" };
            var table = new TableMetadata
                             {
                                 column1,
                                 column2
                             };

            // then
            Assert.AreSame(column1, table["Column1"]);
            Assert.AreSame(column2, table["Column2"]);
        }

        [Test]
        public void ThrowsWhenIndexingWithAnInvalidColumnName()
        {
            // given
            var column1 = new ColumnMetadata { Name = "Column1" };
            var column2 = new ColumnMetadata { Name = "Column2" };
            var table = new TableMetadata
                             {
                                 column1,
                                 column2
                             };

            // then
            Assert.Throws<IndexOutOfRangeException>(() => { var column = table["column"]; });
            Assert.Throws<ArgumentOutOfRangeException>(() => { var column = table[""]; });
            Assert.Throws<ArgumentOutOfRangeException>(() => { var column = table[" "]; });
            Assert.Throws<ArgumentNullException>(() => { var column = table[null]; });
        }

        [Test]
        public void CanContainUniquellyNamedColumns()
        {
            // given
            TableMetadata table = new TableMetadata();

            // when
            table.Add(new ColumnMetadata
            {
                Name = "Column"
            });

            // then
            Assert.Throws<ArgumentException>(() => table.Add(new ColumnMetadata { Name = "Column" }));
        }

        [Test]
        public void TableWithNoForeignKeysReturnsEmptyCollection()
        {
            // given
            TableMetadata table = new TableMetadata
                {
                    new ColumnMetadata{Name="Id", IsPrimaryKey=true},
                    new ColumnMetadata{Name="A"},
                    new ColumnMetadata{Name="B"}
                };

            // when
            var foreignKeys = table.ForeignKeys;

            // then
            Assert.IsNotNull(foreignKeys);
            Assert.IsEmpty(foreignKeys);
        }
    }
}
