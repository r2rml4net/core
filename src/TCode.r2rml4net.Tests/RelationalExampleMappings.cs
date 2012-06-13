﻿using System.Data;
using TCode.r2rml4net.RDB;

namespace TCode.r2rml4net.Tests
{
    public class RelationalTestMappings
    {
        /// <summary>
        /// http://www.w3.org/2001/sw/rdb2rdf/test-cases/#D001-1table1column1row
        /// </summary>
        public static TableCollection D001_1table1column
        {
            get
            {
                var studentsTable = new TableMetadata
                            {
                                new ColumnMetadata
                                {
                                    Name = "Name",
                                    Type = DbType.AnsiString
                                }
                            };
                studentsTable.Name = "Student";

                return new TableCollection
                        {
                            studentsTable
                        };
            }
        }

        /// <summary>
        /// http://www.w3.org/2001/sw/rdb2rdf/test-cases/#D002-1table2columns1row
        /// </summary>
        public static TableCollection TypedColumns
        {
            get
            {
                var studentsTable = new TableMetadata
                            {
                                new ColumnMetadata
                                {
                                    Name = "Int32",
                                    Type = DbType.Int32
                                },
                                new ColumnMetadata
                                {
                                    Name = "Varchar",
                                    Type = DbType.AnsiString
                                },
                                new ColumnMetadata
                                {
                                    Name = "NVarchar",
                                    Type = DbType.String
                                },
                                new ColumnMetadata
                                {
                                    Name = "Float",
                                    Type = DbType.Single
                                },
                                new ColumnMetadata
                                {
                                    Name = "Double",
                                    Type = DbType.Double
                                },
                                new ColumnMetadata
                                {
                                    Name = "Decimal",
                                    Type = DbType.Decimal
                                },
                                new ColumnMetadata
                                {
                                    Name = "Money",
                                    Type = DbType.Currency
                                },
                                new ColumnMetadata
                                {
                                    Name = "Date",
                                    Type = DbType.Date
                                },
                                new ColumnMetadata
                                {
                                    Name = "Time",
                                    Type = DbType.Time
                                },
                                new ColumnMetadata
                                {
                                    Name = "DateTime",
                                    Type = DbType.DateTime
                                },
                                new ColumnMetadata
                                {
                                    Name = "DateTime2",
                                    Type = DbType.DateTime2
                                },
                                new ColumnMetadata
                                {
                                    Name = "Boolean",
                                    Type = DbType.Boolean
                                },
                                new ColumnMetadata
                                {
                                    Name = "Binary",
                                    Type = DbType.Binary
                                },
                                new ColumnMetadata
                                {
                                    Name = "Int16",
                                    Type = DbType.Int16
                                },
                                new ColumnMetadata
                                {
                                    Name = "Int64",
                                    Type = DbType.Int64
                                }
                            };
                studentsTable.Name = "Student";

                return new TableCollection
                       {
                           studentsTable
                       };
            }
        }

        /// <summary>
        /// http://www.w3.org/2001/sw/rdb2rdf/test-cases/#D003-1table3columns1row
        /// </summary>
        public static TableCollection D003_1table3columns
        {
            get
            {
                var studentsTable = new TableMetadata
                            {
                                new ColumnMetadata
                                {
                                    Name = "ID",
                                    Type = DbType.Int32
                                },
                                new ColumnMetadata
                                {
                                    Name = "Name",
                                    Type = DbType.AnsiString
                                },
                                new ColumnMetadata
                                {
                                    Name = "LastName",
                                    Type = DbType.AnsiString
                                }
                            };
                studentsTable.Name = "Student";

                return new TableCollection
                       {
                           studentsTable
                       };
            }
        }

        public static TableCollection D006_1table1primarykey1column
        {
            get
            {
                var studentsTable = new TableMetadata
                                        {
                                            new ColumnMetadata
                                                {
                                                    Name = "Name",
                                                    Type = DbType.AnsiString,
                                                    IsPrimaryKey = true
                                                }
                                        };
                studentsTable.Name = "Student";

                return new TableCollection
                       {
                           studentsTable
                       };
            }
        }

        public static TableCollection D008_1table1compositeprimarykey3columns
        {
            get
            {
                var studentsTable = new TableMetadata
                                        {
                                            new ColumnMetadata
                                                {
                                                    Name = "Name",
                                                    Type = DbType.AnsiString,
                                                    IsPrimaryKey = true
                                                },
                                            new ColumnMetadata
                                                {
                                                    Name = "LastName",
                                                    Type = DbType.AnsiString,
                                                    IsPrimaryKey = true
                                                },
                                            new ColumnMetadata
                                                {
                                                    Name = "Age",
                                                    Type = DbType.Int32
                                                }
                                        };
                studentsTable.Name = "Student";

                return new TableCollection
                       {
                           studentsTable
                       };
            }
        }
    }
}
