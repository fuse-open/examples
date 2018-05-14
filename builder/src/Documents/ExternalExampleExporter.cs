using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Builder.Documents
{
    public class ExternalExampleExporter
    {
        private readonly ILogger<ExternalExampleExporter> _logger;
        private readonly HttpClient _httpClient;

        public ExternalExampleExporter(ILogger<ExternalExampleExporter> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
        }

        public Task<DocumentExportResult> ExportAsync(string exampleName, string exampleJsonPath)
        {
            var metaData = JsonConvert.DeserializeObject<DocumentMetaData>(File.ReadAllText(exampleJsonPath));
            if (metaData == null) throw new ArgumentException($"Unable to deserialize external example {exampleJsonPath}");
            if (string.IsNullOrWhiteSpace(metaData.Title)) throw new ArgumentException($"Missing title in external example {exampleJsonPath}");
            if (string.IsNullOrWhiteSpace(metaData.Synopsis)) throw new ArgumentException($"Missing synopsis in external example {exampleJsonPath}");
            if (string.IsNullOrWhiteSpace(metaData.Url)) throw new ArgumentException($"Missing url in external example {exampleJsonPath}");

            int statusCode;
            if (!TryVerifyUrl(metaData.Url, out statusCode))
            {
                throw new ArgumentException($"Unable to verify url '{metaData.Url}' for external example {exampleJsonPath}  - got status code {statusCode}");
            }

            if (metaData.PublishedAt.HasValue)
            {
                return Task.FromResult(new DocumentExportResult(isExternal: true,
                                                                slot: 0,
                                                                id: exampleName,
                                                                title: metaData.Title,
                                                                synopsis: metaData.Synopsis,
                                                                previewImage: null,
                                                                detailedPreviewImage: null,
                                                                uri: metaData.Url,
                                                                publishedAt: metaData.PublishedAt.Value,
                                                                tags: metaData.Tags ?? new List<string>()));
            }
            return Task.FromResult<DocumentExportResult>(null);
        }

        private bool TryVerifyUrl(string uri, out int statusCode)
        {
            try
            {
                using (var req = new HttpRequestMessage(HttpMethod.Get, uri))
                {
                    using (var res = _httpClient.SendAsync(req).Result)
                    {
                        statusCode = (int) res.StatusCode;
                        return res.StatusCode == HttpStatusCode.OK;
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Unable to verify URL {uri}: {e}");
                statusCode = 0;
                return false;
            }
        }
    }
}