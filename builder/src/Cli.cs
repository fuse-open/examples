using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Builder.Code;
using Builder.Core;
using Builder.Documents;
using Microsoft.Extensions.Logging;

namespace Builder
{
    public class Cli
    {

        // Assigns each example to a slot, depending on publishing time
        // This is used by the site to add some simple styling to each example, depending
        // on it's position in the history.
        private const int NumberOfSlots = 8;

        private readonly ILogger<Cli> _logger;
        private readonly BuilderSettings _settings;
        private readonly CodeExporter _codeExporter;
        private readonly ExampleExporter _exampleExporter;
        private readonly ExternalExampleExporter _externalExampleExporter;
        private readonly ExampleIndexExporter _exampleIndexExporter;

        public Cli(ILogger<Cli> logger,
                   BuilderSettings settings,
                   CodeExporter codeExporter,
                   ExampleExporter exampleExporter,
                   ExternalExampleExporter externalExampleExporter,
                   ExampleIndexExporter exampleIndexExporter)
        {
            _logger = logger;
            _settings = settings;
            _codeExporter = codeExporter;
            _exampleExporter = exampleExporter;
            _externalExampleExporter = externalExampleExporter;
            _exampleIndexExporter = exampleIndexExporter;
        }

        public async Task BuildAsync()
        {
            // Export examples
            var exported = new List<DocumentExportResult>();
            exported.AddRange(await ExportContentAsync("examples", "example.md", PrepareExampleExportAsync, ExportExampleAsync));
            exported.AddRange(await ExportExternalExamplesAsync());
            await _exampleIndexExporter.ExportAsync(exported);
        }

        private async Task<List<DocumentExportResult>> ExportContentAsync(string directoryName,
                                                                          string documentFilename,
                                                                          Func<string, string, Task<PreparedDocumentExport>> prepareFunc,
                                                                          Func<PreparedDocumentExport, Task<DocumentExportResult>> exportFunc)
        {
            var contentPath = Path.Combine(_settings.RootPath, directoryName);
            var documents = Directory.EnumerateFiles(contentPath, documentFilename, SearchOption.AllDirectories)
                                     .ToList();
            var preparedResults = new List<Task<PreparedDocumentExport>>();

            foreach (var doc in documents)
            {
                var docPath = Path.GetFullPath(doc);
                var docRoot = Path.GetDirectoryName(docPath);
                var docName = new DirectoryInfo(docRoot).Name;
                _logger.LogInformation($"Preparing export of {directoryName} document {docName} in directory {docRoot} ({docPath})");
                preparedResults.Add(prepareFunc(docName, docRoot));
            }
            await Task.WhenAll(preparedResults);

            // Assign slots to each document based on the published at order
            var prepared = preparedResults.Select(e => e.Result)
                                          .OrderByDescending(e => e.MetaData.PublishedAt ?? DateTime.MinValue)
                                          .ToList();
            var slot = 0;
            foreach (var item in prepared)
            {
                if (item.MetaData.PublishedAt.HasValue)
                {
                    slot++;
                    if (slot > NumberOfSlots)
                    {
                        slot = 1;
                    }
                    item.Slot = slot;
                }
            }

            // Process documents in reverse published-at order (non-publishable documents first)
            // This lets the Slot ID generated for each document be consistent between runs as long
            // as no new documents are added to the list.
            var generatedResults = new List<Task<DocumentExportResult>>();
            foreach (var item in prepared)
            {
                _logger.LogInformation($"Generating export of {directoryName} document {item.DocumentName} in directory {item.DocumentRoot}");
                generatedResults.Add(exportFunc(item));
            }

            await Task.WhenAll(generatedResults);
            return generatedResults.Where(e => e.Result != null)
                                   .Select(e => e.Result)
                                   .ToList();
        }

        private async Task<PreparedDocumentExport> PrepareExampleExportAsync(string exampleName, string exampleRoot)
        {
            var code = await _codeExporter.ExportForAsync(exampleName, exampleRoot);
            var metaData = await _exampleExporter.LoadMetaDataAsync(exampleName, exampleRoot);
            return new PreparedDocumentExport(exampleName, exampleRoot, code, metaData);
        }

        private async Task<DocumentExportResult> ExportExampleAsync(PreparedDocumentExport preparedExport)
        {
            return await _exampleExporter.ExportAsync(preparedExport.DocumentName,
                                                      preparedExport.DocumentRoot,
                                                      preparedExport.MetaData,
                                                      preparedExport.Code,
                                                      preparedExport.Slot);
        }

        private async Task<List<DocumentExportResult>> ExportExternalExamplesAsync()
        {
            var examplesPath = Path.Combine(_settings.RootPath, "external-examples");
            var examples = Directory.EnumerateFiles(examplesPath, "*.json", SearchOption.TopDirectoryOnly)
                                    .ToList();
            var results = new List<Task<DocumentExportResult>>();

            foreach (var example in examples)
            {
                var examplePath = Path.GetFullPath(example);
                var exampleName = Path.GetFileNameWithoutExtension(examplePath);
                results.Add(_externalExampleExporter.ExportAsync(exampleName, examplePath));
            }

            await Task.WhenAll(results);
            return results.Where(e => e.Result != null)
                          .Select(e => e.Result)
                          .ToList();
        }

        private class PreparedDocumentExport
        {
            public string DocumentName { get; }
            public string DocumentRoot { get; }
            public CodeExportResult Code { get; }
            public DocumentMetaData MetaData { get; }
            public int Slot { get; set; } = 1;

            public PreparedDocumentExport(string documentName,
                                          string documentRoot,
                                          CodeExportResult code,
                                          DocumentMetaData metaData)
            {
                DocumentName = documentName;
                DocumentRoot = documentRoot;
                Code = code;
                MetaData = metaData;
            }
        }
    }
}
