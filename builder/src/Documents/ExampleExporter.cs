using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Builder.Code;
using Builder.Concepts;
using Builder.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Builder.Documents
{
    public class ExampleExporter : ExporterBase
    {

        private readonly ILogger<ExampleExporter> _logger;
        private readonly MarkdownParser _markdownParser;
        private readonly JsConcepts _jsConcepts;
        private readonly UxConcepts _uxConcepts;
        private readonly OutputDir _outputDir;
        private readonly Layout _layout;

        public ExampleExporter(ILogger<ExampleExporter> logger,
                               MarkdownParser markdownParser,
                               JsConcepts jsConcepts,
                               UxConcepts uxConcepts,
                               OutputDir outputDir,
                               Layout layout)
        {
            _logger = logger;
            _markdownParser = markdownParser;
            _jsConcepts = jsConcepts;
            _uxConcepts = uxConcepts;
            _outputDir = outputDir;
            _layout = layout;
        }

        public Task<DocumentMetaData> LoadMetaDataAsync(string exampleName, string directoryName)
        {
            var metaDataFile = Path.Combine(directoryName, "example.json");
            if (!File.Exists(metaDataFile))
            {
                throw new ArgumentException($"No example meta data file found at {metaDataFile}");
            }
            var metaData = LoadMetaData(metaDataFile);
            return Task.FromResult(metaData);
        }

        public async Task<DocumentExportResult> ExportAsync(string exampleName, string directoryName, DocumentMetaData metaData, CodeExportResult code, int slot)
        {
            var exampleFile = Path.Combine(directoryName, "example.md");
            if (!File.Exists(exampleFile))
            {
                throw new ArgumentException($"No example file found at {exampleFile}");
            }

            var mediaFiles = await WriteMediaAsync(exampleName, directoryName);

            if (metaData.PublishedAt.HasValue)
            {
                var content = await RenderContentAsync(File.ReadAllText(exampleFile), metaData, mediaFiles, slot);
                var sidebar = RenderNavigation(metaData, code);
                var html = _layout.Apply(content, sidebar, metaData.Title);
                await _outputDir.WriteAsync(Path.Combine(exampleName, "index.html"), Encoding.UTF8.GetBytes(html));
                return new DocumentExportResult(isExternal: false,
                                                slot: slot,
                                                id: exampleName,
                                                title: metaData.Title,
                                                synopsis: metaData.Synopsis,
                                                previewImage: GetPreviewImage(mediaFiles, $"{exampleName}/"),
                                                detailedPreviewImage: null,
                                                uri: $"{exampleName}/index.html",
                                                publishedAt: metaData.PublishedAt.Value,
                                                tags: metaData.Tags ?? new List<string>());
            }

            return null;
        }

        private DocumentMetaData LoadMetaData(string filename)
        {
            var metaData = JsonConvert.DeserializeObject<DocumentMetaData>(File.ReadAllText(filename));
            if (metaData == null) throw new ArgumentException($"Unable to parse example meta data JSON in {filename}");
            if (string.IsNullOrWhiteSpace(metaData.Title)) throw new ArgumentException($"Missing title in meta data JSON {filename}");
            if (string.IsNullOrWhiteSpace(metaData.Synopsis)) throw new ArgumentException($"Missing synopsis in meta data JSON {filename}");
            return metaData;
        }

        private async Task<HashSet<string>> WriteMediaAsync(string exampleName, string exampleRoot)
        {
            var writtenFilenames = new HashSet<string>();

            var mediaDirectory = Path.Combine(exampleRoot, "media");
            if (!Directory.Exists(mediaDirectory))
            {
                _logger.LogDebug($"Media directory {mediaDirectory} does not exist, skipping media export");
                return writtenFilenames;
            }

            var files = Directory.EnumerateFiles(mediaDirectory, "*.*", SearchOption.AllDirectories)
                                 .ToList();
            foreach (var file in files)
            {
                var relativeName = Path.Combine(exampleName, file.Substring(exampleRoot.Length + 1));
                var relativeToMediaName = file.Substring(mediaDirectory.Length + 1);
                await _outputDir.WriteAsync(relativeName, File.ReadAllBytes(file));
                writtenFilenames.Add(relativeToMediaName);
            }

            return writtenFilenames;
        }

        private string RenderNavigation(DocumentMetaData metaData, CodeExportResult code)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<p class=\"example-back-link\">");
            sb.AppendLine("&lt; <a href=\"../index.html\">Go back</a>");
            sb.AppendLine("</p>");

            if (code.HasCode)
            {
                sb.AppendLine("<p class=\"example-download-link\">");
                sb.AppendLine($"<a href=\"../{code.BundlePath}\" class=\"btn btn-primary btn-block\">Download example</a>");
                sb.AppendLine("</p>");
            }

            if (metaData.Tags?.Count > 0)
            {
                var tags = new List<string>();
                foreach (var tag in metaData.Tags)
                {
                    var filename = GetTagFilename(tag);
                    tags.Add($"<a href=\"../{filename}\">{EscapeHtml(tag)}</a>");
                }

                sb.AppendLine("<h6>Tags</h6>");
                sb.AppendLine("<p class=\"example-tags\">");
                sb.AppendLine(string.Join(", ", tags));
                sb.AppendLine("</p>");
            }

            sb.AppendLine(RenderConcepts(metaData.UxConcepts, _uxConcepts, "UX Concepts"));
            sb.AppendLine(RenderConcepts(metaData.JsConcepts, _jsConcepts, "JS Concepts"));

            return sb.ToString();
        }


        private async Task<string> RenderContentAsync(string markdown, DocumentMetaData metaData, HashSet<string> mediaFiles, int slot)
        {
            var body = await _markdownParser.ParseAsync(markdown);

            // Hack: Replace all links to .md files with links to .html files,
            // as we're changing the extension of the files during local generation.
            body = body.Replace(".md)", ".html)")
                       .Replace(".md\"", ".html\"")
                       .Replace(".md#", ".html#");

            var sb = new StringBuilder();
            sb.AppendLine($"<div class=\"example-content-preview example-content-preview-slot-{slot}\">");
            sb.AppendLine(GetPreviewVideo(mediaFiles));
            sb.AppendLine("</div>");
            sb.AppendLine("<div class=\"example-content-body\">");
            sb.AppendLine(body);
            sb.AppendLine("</div>");
            return sb.ToString();
        }

        private string GetPreviewImage(HashSet<string> mediaFiles, string pathPrefix = null)
        {
            var extensions = new[] { "png", "jpg" };
            foreach (var ext in extensions)
            {
                if (mediaFiles.Contains($"preview.{ext}"))
                {
                    return $"<img src=\"{pathPrefix}media/preview.{ext}\" alt=\"\">";
                }
            }

            return "";
        }

        private string GetPreviewVideo(HashSet<string> mediaFiles, string pathPrefix = null)
        {
            return mediaFiles.Contains("preview.mp4")
                     ? $"<video autoplay=\"true\" loop=\"true\"><source src=\"{pathPrefix}media/preview.mp4\" type=\"video/mp4\"></video>"
                     : GetPreviewImage(mediaFiles);
        }

        private string RenderConcepts(List<string> concepts, ConceptParser conceptParser, string title)
        {
            if (concepts?.Count == 0) return "";

            var sb = new StringBuilder();
            sb.AppendLine($"<h6>{EscapeHtml(title)}</h6>");
            sb.AppendLine("<ul class=\"nav flex-column concepts-nav\">");

            foreach (var concept in concepts)
            {
                sb.AppendLine($"<li class=\"nav-item\"><a href=\"{conceptParser.GetUriFor(concept)}\" class=\"nav-link\">{EscapeHtml(concept)}</a></li>");
            }

            sb.AppendLine("</ul>");
            return sb.ToString();
        }
    }
}
