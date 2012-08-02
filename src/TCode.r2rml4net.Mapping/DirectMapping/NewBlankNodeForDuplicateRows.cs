using System;
using TCode.r2rml4net.RDB;

namespace TCode.r2rml4net.Mapping.DirectMapping
{
    public class NewBlankNodeForDuplicateRows : DirectMappingStrategy
    {
        public NewBlankNodeForDuplicateRows()
            : this(new DirectMappingOptions())
        {

        }

        public NewBlankNodeForDuplicateRows(DirectMappingOptions options)
            : base(options)
        {
        }

        #region Overrides of DirectMappingStrategy

        public override void CreateSubjectMapForNoPrimaryKey(ISubjectMapConfiguration subjectMap, Uri baseUri, TableMetadata table)
        {
            var classIri = SubjectMappingStrategy.CreateSubjectUri(baseUri, table);

            // empty primary key generates blank node subjects
            subjectMap.AddClass(classIri).TermType.IsBlankNode();
        }

        #endregion
    }
}