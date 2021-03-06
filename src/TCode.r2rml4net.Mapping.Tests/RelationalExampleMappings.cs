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
using TCode.r2rml4net.RDB;

namespace TCode.r2rml4net.Mapping.Tests
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
                                    Type = R2RMLType.String
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
                                    Type = R2RMLType.Integer
                                },
                                new ColumnMetadata
                                {
                                    Name = "Varchar",
                                    Type = R2RMLType.String
                                },
                                new ColumnMetadata
                                {
                                    Name = "NVarchar",
                                    Type = R2RMLType.String
                                },
                                new ColumnMetadata
                                {
                                    Name = "Float",
                                    Type = R2RMLType.FloatingPoint
                                },
                                new ColumnMetadata
                                {
                                    Name = "Double",
                                    Type = R2RMLType.FloatingPoint
                                },
                                new ColumnMetadata
                                {
                                    Name = "Decimal",
                                    Type = R2RMLType.Decimal
                                },
                                new ColumnMetadata
                                {
                                    Name = "Money",
                                    Type = R2RMLType.Decimal
                                },
                                new ColumnMetadata
                                {
                                    Name = "Date",
                                    Type = R2RMLType.Date
                                },
                                new ColumnMetadata
                                {
                                    Name = "Time",
                                    Type = R2RMLType.Time
                                },
                                new ColumnMetadata
                                {
                                    Name = "DateTime",
                                    Type = R2RMLType.DateTime
                                },
                                new ColumnMetadata
                                {
                                    Name = "DateTime2",
                                    Type = R2RMLType.DateTime
                                },
                                new ColumnMetadata
                                {
                                    Name = "Boolean",
                                    Type = R2RMLType.Boolean
                                },
                                new ColumnMetadata
                                {
                                    Name = "Binary",
                                    Type = R2RMLType.Binary
                                },
                                new ColumnMetadata
                                {
                                    Name = "Int16",
                                    Type = R2RMLType.Integer
                                },
                                new ColumnMetadata
                                {
                                    Name = "Int64",
                                    Type = R2RMLType.Integer
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
                                    Type = R2RMLType.Integer
                                },
                                new ColumnMetadata
                                {
                                    Name = "Name",
                                    Type = R2RMLType.String
                                },
                                new ColumnMetadata
                                {
                                    Name = "LastName",
                                    Type = R2RMLType.String
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
        /// http://www.w3.org/2001/sw/rdb2rdf/test-cases/#D006-1table1primarykey1column1row
        /// </summary>
        public static TableCollection D006_1table1primarykey1column
        {
            get
            {
                var studentsTable = new TableMetadata
                                        {
                                            new ColumnMetadata
                                                {
                                                    Name = "Name",
                                                    Type = R2RMLType.String,
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

        /// <summary>
        /// http://www.w3.org/2001/sw/rdb2rdf/test-cases/#D008-1table1compositeprimarykey3columns1row
        /// </summary>
        public static TableCollection D008_1table1compositeprimarykey3columns
        {
            get
            {
                var studentsTable = new TableMetadata
                                        {
                                            new ColumnMetadata
                                                {
                                                    Name = "Name",
                                                    Type = R2RMLType.String,
                                                    IsPrimaryKey = true
                                                },
                                            new ColumnMetadata
                                                {
                                                    Name = "LastName",
                                                    Type = R2RMLType.String,
                                                    IsPrimaryKey = true
                                                },
                                            new ColumnMetadata
                                                {
                                                    Name = "Age",
                                                    Type = R2RMLType.Integer
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
        /// http://www.w3.org/2001/sw/rdb2rdf/test-cases/#D009-2tables1primarykey1foreignkey
        /// </summary>
        public static TableCollection D009_2tables1primarykey1foreignkey
        {
            get
            {
                var studentsTable = new TableMetadata
                                        {
                                            new ColumnMetadata
                                                {
                                                    Name = "ID",
                                                    Type = R2RMLType.Integer,
                                                    IsPrimaryKey = true
                                                },
                                            new ColumnMetadata
                                                {
                                                    Name = "Name",
                                                    Type = R2RMLType.String
                                                },
                                            new ColumnMetadata
                                                {
                                                    Name = "Sport",
                                                    Type = R2RMLType.Integer
                                                }
                                        };
                studentsTable.Name = "Student";

                var sportTable = new TableMetadata
                                     {
                                         new ColumnMetadata
                                             {
                                                 Name = "ID",
                                                 Type = R2RMLType.Integer,
                                                 IsPrimaryKey = true
                                             },
                                         new ColumnMetadata
                                             {
                                                 Name = "Name",
                                                 Type = R2RMLType.String
                                             }
                                     };
                sportTable.Name = "Sport";
                studentsTable.ForeignKeys = new[]
                                                {
                                                    new ForeignKeyMetadata
                                                        {
                                                            TableName = "Student",
                                                            ReferencedTable = sportTable,
                                                            ForeignKeyColumns = new[] {"Sport"},
                                                            ReferencedColumns = new[] {"ID"}
                                                        }
                                                };

                return new TableCollection
                       {
                           studentsTable,
                           sportTable
                       };
            }
        }

        /// <summary>
        /// http://www.w3.org/2001/sw/rdb2rdf/test-cases/#D010-1table1primarykey3colums3rows
        /// </summary>
        public static TableCollection D010_1table1primarykey3colums
        {
            get
            {
                TableMetadata countryTable = new TableMetadata
                                                 {
                                                     new ColumnMetadata
                                                         {
                                                             Name = "Country Code",
                                                             Type = R2RMLType.Integer,
                                                             IsPrimaryKey = true
                                                         },
                                                     new ColumnMetadata
                                                         {
                                                             Name = "Name",
                                                             Type = R2RMLType.String
                                                         },
                                                     new ColumnMetadata
                                                         {
                                                             Name = "ISO 3166",
                                                             Type = R2RMLType.String
                                                         }
                                                 };
                countryTable.Name = "Country Info";

                return new TableCollection
                           {
                               countryTable
                           };
            }
        }

        /// <summary>
        /// http://www.w3.org/2001/sw/rdb2rdf/test-cases/#D011-M2MRelations
        /// </summary>
        public static TableCollection D011_M2MRelations
        {
            get
            {
                TableMetadata studentTable = new TableMetadata
                                                 {
                                                     new ColumnMetadata
                                                         {
                                                             Name = "ID",
                                                             Type = R2RMLType.Integer,
                                                             IsPrimaryKey = true
                                                         },
                                                     new ColumnMetadata
                                                         {
                                                             Name = "FirstName",
                                                             Type = R2RMLType.String
                                                         },
                                                     new ColumnMetadata
                                                         {
                                                             Name = "LastName",
                                                             Type = R2RMLType.String
                                                         }
                                                 };
                TableMetadata sportTable = new TableMetadata
                                               {
                                                   new ColumnMetadata
                                                       {
                                                           Name = "ID",
                                                           Type = R2RMLType.Integer,
                                                           IsPrimaryKey = true
                                                       },
                                                   new ColumnMetadata
                                                       {
                                                           Name = "Description",
                                                           Type = R2RMLType.String
                                                       }
                                               };
                TableMetadata relationTable = new TableMetadata
                                                  {
                                                      new ColumnMetadata
                                                          {
                                                              Name = "ID_Student",
                                                             Type = R2RMLType.Integer,
                                                              IsPrimaryKey = true
                                                          },
                                                      new ColumnMetadata
                                                          {
                                                              Name = "ID_Sport",
                                                             Type = R2RMLType.Integer,
                                                              IsPrimaryKey = true
                                                          }
                                                  };

                studentTable.Name = "Student";
                sportTable.Name = "Sport";
                relationTable.Name = "Student_Sport";

                relationTable.ForeignKeys = new[]
                                                {
                                                    new ForeignKeyMetadata
                                                        {
                                                            ForeignKeyColumns = new[] {"ID_Student"},
                                                            ReferencedColumns = new[] {"ID"},
                                                            ReferencedTable = studentTable,
                                                            TableName = "Student_Sport"
                                                        },
                                                    new ForeignKeyMetadata
                                                        {
                                                            ForeignKeyColumns = new[] {"ID_Sport"},
                                                            ReferencedColumns = new[] {"ID"},
                                                            ReferencedTable = sportTable,
                                                            TableName = "Student_Sport"
                                                        }
                                                };

                return new TableCollection
                           {
                               studentTable,
                               sportTable,
                               relationTable
                           };
            }
        }

        /// <summary>
        /// http://www.w3.org/2001/sw/rdb2rdf/test-cases/#D014-3tables1primarykey1foreignkey
        /// </summary>
        public static TableCollection D014_3tables1primarykey1foreignkey
        {
            get
            {
                TableMetadata empTable = new TableMetadata
                                             {
                                                 new ColumnMetadata
                                                     {
                                                         Name ="empno",
                                                         IsPrimaryKey = true,
                                                         Type = R2RMLType.Integer
                                                     },
                                                  new ColumnMetadata
                                                      {
                                                          Name = "deptno",
                                                          Type = R2RMLType.Integer
                                                      },
                                                  new ColumnMetadata
                                                      {
                                                          Name = "ename",
                                                          Type = R2RMLType.String
                                                      },
                                                  new ColumnMetadata
                                                      {
                                                          Name = "job",
                                                          Type = R2RMLType.String
                                                      },
                                                  new ColumnMetadata
                                                      {
                                                          Name = "etype",
                                                          Type = R2RMLType.String
                                                      }
                                             };
                TableMetadata likesTable = new TableMetadata
                                              {
                                                  new ColumnMetadata
                                                      {
                                                          Name = "id",
                                                          Type = R2RMLType.Integer
                                                      },
                                                  new ColumnMetadata
                                                      {
                                                          Name = "likeType",
                                                          Type = R2RMLType.String
                                                      },
                                                  new ColumnMetadata
                                                      {
                                                          Name = "likedObj",
                                                          Type = R2RMLType.String
                                                      }
                                              };
                TableMetadata deptTable = new TableMetadata
                                              {
                                                  new ColumnMetadata
                                                      {
                                                          Name = "deptno",
                                                          Type = R2RMLType.Integer
                                                      },
                                                  new ColumnMetadata
                                                      {
                                                          Name = "dname",
                                                          Type = R2RMLType.String
                                                      },
                                                  new ColumnMetadata
                                                      {
                                                          Name = "loc",
                                                          Type = R2RMLType.String
                                                      }
                                              };

                empTable.Name = "EMP";
                likesTable.Name = "LIKES";
                deptTable.Name = "DEPT";

                empTable.ForeignKeys = new[]
                                           {
                                               new ForeignKeyMetadata
                                                   {
                                                       ForeignKeyColumns = new[] {"deptno"},
                                                       ReferencedColumns = new[] {"deptno"},
                                                       ReferencedTable = deptTable,
                                                       TableName = "EMP",
                                                       IsCandidateKeyReference = true
                                                   }
                                           };
                var uniqueKey = new UniqueKeyMetadata { deptTable["deptno"] };
                deptTable.UniqueKeys.Add(uniqueKey);

                return new TableCollection
                           {
                               empTable,
                               likesTable,
                               deptTable
                           };
            }
        }

        /// <summary>
        /// http://www.w3.org/2001/sw/rdb2rdf/test-cases/#D015-1table3columns1composityeprimarykey3rows2languages
        /// </summary>
        public static TableCollection D015_1table3columns1composityeprimarykey
        {
            get
            {
                TableMetadata countryTable = new TableMetadata
                                             {
                                                 new ColumnMetadata
                                                     {
                                                         Name ="Code",
                                                         IsPrimaryKey = true,
                                                         Type = R2RMLType.String
                                                     },
                                                  new ColumnMetadata
                                                      {
                                                          Name = "Lan",
                                                          IsPrimaryKey = true,
                                                          Type = R2RMLType.String
                                                      },
                                                  new ColumnMetadata
                                                      {
                                                          Name = "Name",
                                                          Type = R2RMLType.String
                                                      }
                                             };

                countryTable.Name = "Country";

                return new TableCollection
                           {
                               countryTable
                           };
            }
        }

        /// <summary>
        /// http://www.w3.org/2001/sw/rdb2rdf/test-cases/#D016-1table1primarykey10columns3rowsSQLdatatypes
        /// </summary>
        public static TableCollection D016_1table1primarykey10columnsSQLdatatypes
        {
            get
            {
                TableMetadata patientTable = new TableMetadata
                                             {
                                                 new ColumnMetadata
                                                 {
                                                     Name = "ID",
                                                     IsPrimaryKey = true,
                                                     Type = R2RMLType.Integer
                                                 },
                                                 new ColumnMetadata
                                                 {
                                                     Name = "FirstName",
                                                     Type = R2RMLType.String
                                                 },
                                                 new ColumnMetadata
                                                 {
                                                     Name = "LastName",
                                                     Type = R2RMLType.String
                                                 },
                                                 new ColumnMetadata
                                                 {
                                                     Name = "Sex",
                                                     Type = R2RMLType.String
                                                 },
                                                 new ColumnMetadata
                                                 {
                                                     Name = "Weight",
                                                     Type = R2RMLType.FloatingPoint
                                                 },
                                                 new ColumnMetadata
                                                 {
                                                     Name = "Height",
                                                     Type = R2RMLType.FloatingPoint
                                                 },
                                                 new ColumnMetadata
                                                 {
                                                     Name = "BirthDate",
                                                     Type = R2RMLType.Date
                                                 },
                                                 new ColumnMetadata
                                                 {
                                                     Name = "EntranceDate",
                                                     Type = R2RMLType.DateTime
                                                 },
                                                 new ColumnMetadata
                                                 {
                                                     Name = "PaidInAdvance",
                                                     Type = R2RMLType.Boolean
                                                 },
                                                 new ColumnMetadata
                                                 {
                                                     Name = "Photo",
                                                     Type = R2RMLType.Binary
                                                 }
                                             };

                patientTable.Name = "Patient";

                return new TableCollection
                {
                    patientTable
                };
            }
        }

        /// <summary>
        /// http://www.w3.org/2001/sw/rdb2rdf/test-cases/#D017-I18NnoSpecialChars
        /// </summary>
        public static TableCollection D017_I18NnoSpecialChars
        {
            get
            {
                TableMetadata i18N1 = new TableMetadata
                {
                    new ColumnMetadata
                    {
                        Name="植物名",
                        Type = R2RMLType.String
                    },
                    new ColumnMetadata
                    {
                        Name="使用部",
                        Type = R2RMLType.String
                    },
                    new ColumnMetadata
                    {
                        Name="皿",
                        Type = R2RMLType.String
                    }
                };

                TableMetadata i18N2 = new TableMetadata
                {
                    new ColumnMetadata
                    {
                        Name="名",
                        IsPrimaryKey = true,
                        Type = R2RMLType.String
                    },
                    new ColumnMetadata
                    {
                        Name="使用部",
                        IsPrimaryKey = true,
                        Type = R2RMLType.String
                    },
                    new ColumnMetadata
                    {
                        Name="条件",
                        Type = R2RMLType.String
                    }
                };

                i18N1.ForeignKeys = new[]
                {
                    new ForeignKeyMetadata
                    {
                        ForeignKeyColumns=new[] {"植物名", "使用部"},
                        ReferencedColumns = new [] {"名", "使用部"},
                        ReferencedTable = i18N2,
                        TableName = "成分"
                    }
                };

                i18N1.Name = "成分";
                i18N2.Name = "植物";

                return new TableCollection
                {
                    i18N1, i18N2
                };
            }
        }

        public static TableCollection NoPrimaryKeyThreeUniqueKeys
        {
            get
            {
                var col1 = new ColumnMetadata
                    {
                        Name = "ID",
                        Type = R2RMLType.Integer
                    };
                var col2 = new ColumnMetadata
                    {
                        Name = "Name",
                        Type = R2RMLType.String
                    };
                var col3 = new ColumnMetadata
                    {
                        Name = "LastName",
                        Type = R2RMLType.String
                    };
                var col4 = new ColumnMetadata
                    {
                        Name = "Column4",
                        Type = R2RMLType.String
                    };
                var studentsTable = new TableMetadata
                            {
                                col1,
                                col2,
                                col3,
                                col4
                            };
                studentsTable.Name = "Student";
                var uniqueKey = new UniqueKeyMetadata { col1, col2 };
                uniqueKey.IsReferenced = true;
                var uniqueKey2 = new UniqueKeyMetadata { col3, col4 };
                uniqueKey2.IsReferenced = true;
                studentsTable.UniqueKeys = new UniqueKeyCollection
                    {
                        new UniqueKeyMetadata{col1},
                        uniqueKey,
                        uniqueKey2,
                    };

                return new TableCollection
                       {
                           studentsTable
                       };
            }
        }

        public static TableCollection CandidateKeyReferencsTableWithPrimaryKey
        {
            get
            {
                TableMetadata target = new TableMetadata
                    {
                        new ColumnMetadata {Name = "Id", IsPrimaryKey = true},
                        new ColumnMetadata {Name = "UQ1"},
                        new ColumnMetadata {Name = "UQ2"}
                    };
                target.Name = "Target";
                var uniqueKey = new UniqueKeyMetadata
                    {
                        target["UQ1"],
                        target["UQ2"]
                    };
                uniqueKey.IsReferenced = true;
                target.UniqueKeys.Add(uniqueKey);

                TableMetadata source = new TableMetadata
                    {
                        new ColumnMetadata {Name = "FK1"},
                        new ColumnMetadata {Name = "FK2"},
                        new ColumnMetadata {Name = "Some column"}
                    };
                source.Name = "Source";
                source.ForeignKeys = new[]
                    {
                        new ForeignKeyMetadata
                            {
                                ForeignKeyColumns = new[] {"FK1", "FK2"},
                                ReferencedColumns = new[] {"UQ1", "UQ2"},
                                ReferencedTable = target,
                                IsCandidateKeyReference = true,
                                ReferencedTableHasPrimaryKey = true,
                                TableName = "Source"
                            }
                    };

                return new TableCollection
                    {
                        source,
                        target
                    };
            }
        }
    }
}
