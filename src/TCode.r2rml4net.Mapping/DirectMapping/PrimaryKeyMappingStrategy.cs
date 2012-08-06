using System;
using System.Collections.Generic;
using System.Linq;
using TCode.r2rml4net.RDB;

namespace TCode.r2rml4net.Mapping.DirectMapping
{
    public class PrimaryKeyMappingStrategy : MappingStrategyBase, IPrimaryKeyMappingStrategy
    {
        public PrimaryKeyMappingStrategy(DirectMappingOptions options)
            : base(options)
        {
        }

        #region Implementation of IPrimaryKeyMappingStrategy

        public Uri CreateSubjectUri(Uri baseUri, string tableName)
        {
            if (baseUri == null)
                throw new ArgumentNullException("baseUri");
            if (tableName == null)
                throw new ArgumentNullException("tableName");
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentException("Invalid table name");

            return new Uri(baseUri, DirectMappingHelper.UrlEncode(tableName));
        }

        public virtual string CreateSubjectTemplateForNoPrimaryKey(TableMetadata table)
        {
            if (table == null)
                throw new ArgumentNullException("table");

            var uniqueKeys = table.UniqueKeys.ToArray();
            var referencedUniqueKeys = uniqueKeys.Where(uq => uq.IsReferenced).ToArray();
            if (referencedUniqueKeys.Length > 1)
                ;

            ColumnCollection columnsForTemplate;

            if (uniqueKeys.Any())
            {
                if (referencedUniqueKeys.Length == 1)
                    columnsForTemplate = referencedUniqueKeys.Single();
                else
                    columnsForTemplate = uniqueKeys.OrderBy(c => c.ColumnsCount).First();
            }
            else
            {
                columnsForTemplate = table;
            }

            var columnsArray=columnsForTemplate.Select(c => c.Name).ToArray();
            var name = table.Name;
            if (!columnsArray.Any())
                throw new InvalidTriplesMapException(string.Format("No columns for table {0}", name));

            return CreateBlankNodeTemplate(name, columnsArray);
        }

        public virtual string CreateSubjectTemplateForPrimaryKey(Uri baseUri, TableMetadata table)
        {
            if (table == null)
                throw new ArgumentNullException("table");
            if (baseUri == null)
                throw new ArgumentNullException("baseUri");

            if (!table.PrimaryKey.Any())
                throw new ArgumentException(string.Format("Table {0} has no primary key", table.Name));

            string template = DirectMappingHelper.UrlEncode(CreateSubjectUri(baseUri, table.Name).ToString());
            template += "/" + string.Join(";", table.PrimaryKey.Select(pk => pk.Name).Select(pk => string.Format("{0}={1}", DirectMappingHelper.UrlEncode(pk), DirectMappingHelper.EncloseColumnName(pk))));
            return template;
        }

        #endregion
    }
}