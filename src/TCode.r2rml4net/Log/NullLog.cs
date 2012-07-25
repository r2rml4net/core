using TCode.r2rml4net.Mapping;
using VDS.RDF;

namespace TCode.r2rml4net.Log
{
    class NullLog : ITriplesGenerationLog, IRDFTermGenerationLog
    {
        static readonly object ClassLock = new object();
        private static NullLog _instance;

        private NullLog()
        {
        }

        internal static NullLog Instance
        {
            get
            {
                lock(ClassLock)
                {
                    if(_instance == null)
                    {
                        lock (ClassLock)
                        {
                          _instance = new NullLog();  
                        }
                    }
                }

                return _instance;
            }
        }

        #region Implementation of ITriplesGenerationLog

        public void LogMissingSubject(ITriplesMap triplesMap)
        {
        }

        #endregion

        #region Implementation of IRDFTermGenerationLog

        public void LogColumnNotFound(ITermMap termMap, string columnName)
        {
        }

        public void LogTermGenerated(INode node)
        {
        }

        public void LogNullTermGenerated(ITermMap termMap)
        {
        }

        #endregion
    }
}