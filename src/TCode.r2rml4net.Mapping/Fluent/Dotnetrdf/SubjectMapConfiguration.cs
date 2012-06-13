using System;
using System.Linq;
using TCode.r2rml4net.RDF;
using VDS.RDF;

namespace TCode.r2rml4net.Mapping.Fluent.Dotnetrdf
{
    /// <summary>
    /// Fluent configuration of subject map backed by a DotNetRDF graph (see <see cref="ISubjectMapConfiguration"/>)
    /// </summary>
    public class SubjectMapConfiguration : TermMapConfiguration, ISubjectMapConfiguration, INonLiteralTermMapConfigutarion, ISubjectMap
    {
        internal SubjectMapConfiguration(INode triplesMapNode, IGraph r2RMLMappings) : base(triplesMapNode, r2RMLMappings)
        {
        }

        #region Implementation of ISubjectMapConfiguration

        /// <summary>
        /// <see cref="ISubjectMapConfiguration.AddClass"/>
        /// </summary>
        public ISubjectMapConfiguration AddClass(Uri classIri)
        {
            // create SubjectMap - TriplesMap relation if no class has been added
            if(ClassIris.Length == 0)
                CreateParentMapRelation();

            R2RMLMappings.Assert(
                TermMapNode,
                R2RMLMappings.CreateUriNode(UrisHelper.RrClassProperty),
                R2RMLMappings.CreateUriNode(classIri));

            return this;
        }

        /// <summary>
        /// <see cref="ISubjectMapConfiguration.ClassIris"/>
        /// </summary>
        public Uri[] ClassIris
        {
            get
            {
                var classes = R2RMLMappings.GetTriplesWithSubjectPredicate(TermMapNode, R2RMLMappings.CreateUriNode(UrisHelper.RrClassProperty));
                return classes.Select(triple => ((IUriNode)triple.Object).Uri).ToArray();
            }
        }

        #endregion

        #region Overrides of TermMapConfiguration

        /// <summary>
        /// Returns rr:subject
        /// </summary>
        protected internal override IUriNode CreateConstantPropertyNode()
        {
            return R2RMLMappings.CreateUriNode(UrisHelper.RrSubjectProperty);
        }

        /// <summary>
        /// Returns rr:subjectMap
        /// </summary>
        protected internal override IUriNode CreateMapPropertyNode()
        {
            return R2RMLMappings.CreateUriNode(UrisHelper.RrSubjectMapProperty);
        }

        #endregion

        #region Implementation of ISubjectMap

        public Uri Subject
        {
            get { return GetSingleUriValueForPredicate(R2RMLMappings.CreateUriNode(UrisHelper.RrConstantProperty)); }
        }

        #endregion
    }
}