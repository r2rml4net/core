using TCode.r2rml4net.Mapping;

namespace TCode.r2rml4net
{
    public class InvalidTemplateException : InvalidTermException
    {
        public InvalidTemplateException(ITermMap termMap)
            : base(termMap, "Template is missing")
        {
        }
    }
}