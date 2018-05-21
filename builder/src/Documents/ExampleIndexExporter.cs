using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Builder.Core;
using Microsoft.Extensions.Logging;

namespace Builder.Documents
{
    public class ExampleIndexExporter : ExporterBase
    {
        private const string ExampleColumnCssClass = "col-6 col-lg-4 col-md-6";

        private readonly string _baseUrl;
        private readonly ILogger<ExampleIndexExporter> _logger;
        private readonly MarkdownParser _markdownParser;
        private readonly Layout _layout;
        private readonly OutputDir _outputDir;

        public ExampleIndexExporter(BuilderSettings settings,
                                    ILogger<ExampleIndexExporter> logger,
                                    MarkdownParser markdownParser,
                                    Layout layout,
                                    OutputDir outputDir)
        {
            _baseUrl = settings.BaseUrl;
            _logger = logger;
            _markdownParser = markdownParser;
            _layout = layout;
            _outputDir = outputDir;
        }

        public async Task ExportAsync(List<DocumentExportResult> exportedDocuments)
        {
            var tagCloud = BuildTagCloud(exportedDocuments);

            await ExportMainIndexDocumentAsync(exportedDocuments, tagCloud);

            // Build a unique list of tags.
            var tags = exportedDocuments.SelectMany(e => e.Tags).Distinct().ToList();
            foreach (var tag in tags)
            {
                if (!string.IsNullOrWhiteSpace(tag))
                {
                    await ExportTagIndexDocumentAsync(tag, exportedDocuments, tagCloud);
                }
            }

            await ExportSitemapDocumentAsync(exportedDocuments);
        }

        private Task ExportMainIndexDocumentAsync(List<DocumentExportResult> exportedDocuments, List<TagCloudEntry> tagCloud)
        {
            return RenderIndexDocumentAsync("All Examples", "index.html", exportedDocuments, tagCloud);
        }

        private Task ExportTagIndexDocumentAsync(string tag, List<DocumentExportResult> exportedDocuments, List<TagCloudEntry> tagCloud)
        {
            var docs = exportedDocuments.Where(e => e.Tags.Contains(tag)).ToList();
            return RenderIndexDocumentAsync($"Examples: {tag}", GetTagFilename(tag), docs, tagCloud, tag);
        }

        private async Task ExportSitemapDocumentAsync(List<DocumentExportResult> exportedDocuments)
        {
            var lastmod = DateTime.UtcNow.ToString("yyyy-MM-dd");
            var sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");
            foreach (var doc in exportedDocuments.Where(e => !e.IsExternal)) {
                sb.AppendLine("\t<url>");
                sb.AppendLine($"\t\t<loc>{_baseUrl}{doc.Uri}</loc>");
                sb.AppendLine($"\t\t<lastmod>{lastmod}</lastmod>");
                sb.AppendLine("\t\t<changefreq>daily</changefreq>");
                sb.AppendLine("\t</url>");
            }
            sb.AppendLine("</urlset>");
            await _outputDir.WriteAsync("sitemap.xml", Encoding.UTF8.GetBytes(sb.ToString()));
        }

        private async Task RenderIndexDocumentAsync(string title, string filename, List<DocumentExportResult> documents, List<TagCloudEntry> tagCloud, string currentTag = "")
        {
            var sidebar = RenderTagCloud(tagCloud, currentTag);
            var html = _layout.Apply(await RenderIndexAsync(documents), sidebar, title);
            await _outputDir.WriteAsync(filename, Encoding.UTF8.GetBytes(html));
        }

        private List<TagCloudEntry> BuildTagCloud(List<DocumentExportResult> exportedDocuments)
        {
            var cloud = new Dictionary<string, TagCloudEntry>();

            foreach (var doc in exportedDocuments.Where(e => !e.IsExternal))
            {
                foreach (var tag in doc.Tags)
                {
                    if (string.IsNullOrWhiteSpace(tag))
                    {
                        continue;
                    }

                    var tagId = tag.ToLowerInvariant();
                    if (!cloud.ContainsKey(tagId))
                    {
                        cloud.Add(tagId, new TagCloudEntry(tag));
                    }
                    cloud[tagId].IncrementNumberOfExamples();
                }
            }

            return cloud.Values.OrderBy(e => e.TagName)
                               .ToList();
        }

        private string RenderTagCloud(List<TagCloudEntry> tagCloud, string currentTag)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<h6>Categories</h6>");

            sb.AppendLine("<ul class=\"nav flex-column tag-categories\">");

            var allClasses = "nav-link";
            if (string.IsNullOrEmpty(currentTag))
            {
                allClasses = $"{allClasses} active";
            }
            sb.AppendLine($"<li class=\"nav-item\"><a href=\"index.html\" class=\"{allClasses}\">All</a></li>");

            foreach (var tag in tagCloud)
            {
                var tagClasses = "nav-link";
                if (tag.TagName == currentTag)
                {
                    tagClasses = $"{tagClasses} active";
                }
                var filename = GetTagFilename(tag.TagName);
                sb.AppendLine($"<li class=\"nav-item\"><a href=\"{filename}\" class=\"{tagClasses}\">{tag.TagName}</a></li>");
            }
            sb.AppendLine("</ul>");

            return sb.ToString();
        }

        private async Task<string> RenderIndexAsync(List<DocumentExportResult> exportedDocuments)
        {
            var sb = new StringBuilder();
            if (exportedDocuments.Where(e => !e.IsExternal).Any())
            {
                sb.AppendLine("<div class=\"row examples-index\">");

                foreach (var example in exportedDocuments.Where(e => !e.IsExternal).OrderByDescending(e => e.PublishedAt))
                {
                    sb.AppendLine($"<div class=\"{ExampleColumnCssClass} examples-index-item\">");
                    sb.AppendLine($"<h6><a href=\"{example.Uri}\">{EscapeHtml(example.Title)}</a></h6>");
                    sb.AppendLine($"<div class=\"examples-index-item-preview examples-index-item-preview-slot-{example.Slot}\">");
                    sb.AppendLine($"<a href=\"{example.Uri}\">");
                    sb.AppendLine(example.PreviewImage);
                    sb.AppendLine("</a>");
                    sb.AppendLine("</div>");
                    sb.AppendLine("<div class=\"examples-index-item-synopsis\">");
                    sb.AppendLine(await _markdownParser.ParseAsync(example.Synopsis));
                    sb.AppendLine("</div>");
                    sb.AppendLine($"<p class=\"examples-index-item-read-more\"><a href=\"{example.Uri}\">Read full example</a></p>");
                    sb.AppendLine("</div>");
                }

                sb.AppendLine("</div>");
            }

            if (exportedDocuments.Where(e => e.IsExternal).Any())
            {
                sb.AppendLine("<h3>Code-Only</h3>");
                sb.AppendLine("<div class=\"row examples-code-only-index\">");

                foreach (var example in exportedDocuments.Where(e => e.IsExternal).OrderByDescending(e => e.PublishedAt))
                {
                    sb.AppendLine($"<div class=\"{ExampleColumnCssClass} examples-code-only-index-item\">");
                    sb.AppendLine($"<h6><a href=\"{example.Uri}\">{EscapeHtml(example.Title)}</a></h6>");
                    sb.AppendLine("<div class=\"examples-code-only-index-item-synopsis\">");
                    sb.AppendLine(await _markdownParser.ParseAsync(example.Synopsis));
                    sb.AppendLine("</div>");
                    sb.AppendLine($"<p class=\"examples-code-only-index-item-read-more\"><a href=\"{example.Uri}\">View on Github</a></p>");
                    sb.AppendLine("</div>");
                }

                sb.AppendLine("</div>");
            }

            return sb.ToString();
        }

        private class TagCloudEntry
        {
            public string TagName { get; }
            public int NumberOfExamples { get; private set; } = 0;

            public TagCloudEntry(string tagName)
            {
                TagName = tagName;
            }

            public void IncrementNumberOfExamples()
            {
                NumberOfExamples = NumberOfExamples + 1;
            }
        }
    }
}
