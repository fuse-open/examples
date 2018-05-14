using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Builder.Core;
using Microsoft.Extensions.Logging;

namespace Builder.Code
{
    /// Copies all code from an example directory into the corresponding bundles
    /// directory in the output path.
    public class CodeExporter
    {
        private readonly ILogger<CodeExporter> _logger;
        private readonly OutputDir _outputDir;
        private readonly CodeStripper _stripper;

        public CodeExporter(ILogger<CodeExporter> logger,
                            OutputDir outputDir,
                            CodeStripper stripper)
        {
            _logger = logger;
            _outputDir = outputDir;
            _stripper = stripper;
        }

        public async Task<CodeExportResult> ExportForAsync(string exampleName, string directoryName)
        {
            var codeDirectory = Path.Combine(directoryName, "code");
            if (!Directory.Exists(codeDirectory))
            {
                _logger.LogDebug($"No code directory exists for example directory {directoryName}, skipping");
                return new CodeExportResult(false, null);
            }

            var files = Directory.EnumerateFiles(codeDirectory, "*.*", SearchOption.AllDirectories)
                                 .ToList();
            foreach (var file in files)
            {
                var relativePath = file.Substring(codeDirectory.Length + 1);
                var destination = Path.Combine("bundles", exampleName, relativePath);
                var buffer = _stripper.RemoveSnippets(file, File.ReadAllBytes(file));
                await _outputDir.WriteAsync(destination, buffer);
            }

            return new CodeExportResult(true, $"bundles/{exampleName}.zip");
        }
    }
}