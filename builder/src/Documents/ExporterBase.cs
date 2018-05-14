using System.Text.RegularExpressions;
using System.Text.Encodings.Web;

namespace Builder.Documents
{
    public abstract class ExporterBase
    {
        private readonly Regex TagFilenamePattern = new Regex("[^a-zA-Z0-9-_]");

        protected string EscapeHtml(string str)
        {
            return HtmlEncoder.Default.Encode(str);
        }

        protected string GetTagFilename(string tag)
        {
            var filename = TagFilenamePattern.Replace(tag.ToLowerInvariant(), "");
            return $"tag_{filename}.html";
        }
    }
}
