using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Builder.Core
{
    public class OutputDir
    {
        private readonly ILogger<OutputDir> _logger;
        private string _root;

        public OutputDir(ILogger<OutputDir> logger, BuilderSettings settings)
        {
            _logger = logger;
            _root = Path.Combine(settings.RootPath, "generated");

            if (Directory.Exists(_root))
            {
                _logger.LogInformation($"Deleting already existing output directory {_root}");
                Directory.Delete(_root, true);
            }
            Directory.CreateDirectory(_root);
        }

        public async Task WriteAsync(string relativePath, byte[] content)
        {
            var fullPath = Path.Combine(_root, relativePath);
            var directoryName = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(directoryName)) Directory.CreateDirectory(directoryName);

            using (var file = File.OpenWrite(fullPath))
            {
                await file.WriteAsync(content, 0, content.Length);
            }
            _logger.LogDebug($"Wrote file {relativePath} ({content.Length} bytes) to output directory");
        }
    }
}