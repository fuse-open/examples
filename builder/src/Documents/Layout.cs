using System.IO;
using System.Text.Encodings.Web;
using Builder.Core;

namespace Builder.Documents
{
    public class Layout
    {
        private readonly string _template;

        public Layout(BuilderSettings settings)
        {
            _template = ReadTemplate(settings.RootPath);
        }

        public string Apply(string html, string sidebar, string title)
        {
            var fullTitle = "Fuse Examples";
            if (!string.IsNullOrEmpty(title))
            {
                fullTitle = $"{title} - {fullTitle}";
            }

            html = _template.Replace("##BODY##", html)
                            .Replace("##SIDEBAR##", sidebar)
                            .Replace("##PAGETITLE##", HtmlEncoder.Default.Encode(fullTitle))
                            .Replace("##TITLE##", HtmlEncoder.Default.Encode(title));
            return html;
        }

        private string ReadTemplate(string root)
        {
            return File.ReadAllText(Path.Combine(root, "layout.html"));
        }
    }
}
