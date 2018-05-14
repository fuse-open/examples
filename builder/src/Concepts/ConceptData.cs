using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Builder.Concepts
{
    public class ConceptData
    {
        private const string BaseUri = "https://docs.fusetools.com/";

        private readonly string _type;
        private readonly List<ConceptDataEntry> _entries = new List<ConceptDataEntry>();

        public ConceptData(string type)
        {
            _type = type;
        }

        public HashSet<string> GetUris() => new HashSet<string>(_entries.Select(e => e.Uri));

        public void Add(string name, string uri)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"Concept of type {_type} was missing name (uri was {uri})");
            }
            if (string.IsNullOrWhiteSpace(uri))
            {
                throw new ArgumentException($"Concept of type {_type} with name {name} was missing uri");
            }
            if (_entries.Any(e => e.Name.ToLowerInvariant() == name.ToLowerInvariant()))
            {
                throw new ArgumentException($"Concept of type {_type} with name {name} was defined multiple times");
            }

            _entries.Add(new ConceptDataEntry(name.Trim(), uri.Trim()));
        }

        public string GetUriFor(string name)
        {
            var result = _entries.FirstOrDefault(e => e.Name.ToLowerInvariant() == name.ToLowerInvariant());
            if (result == null)
            {
                throw new ArgumentException($"Unable to map {_type} concept '{name}'");
            }
            return BaseUri + result.Uri.TrimStart('/');
        }

        public void VerifyUrisOrThrow(ILogger logger)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(BaseUri) })
            {
                var testTasks = _entries.Select(e => e.Uri).Distinct().Select(e => TestConceptUriAsync(e, client)).ToArray();
                Task.WaitAll(testTasks);

                var failedTests = testTasks.Select(e => e.Result)
                                        .Where(e => !e.WasSuccessful)
                                        .ToList();
                if (failedTests.Any())
                {
                    logger.LogError($"{failedTests.Count} {_type} concept URLs were not valid:");
                    foreach (var failedTest in failedTests)
                    {
                        logger.LogError($"{failedTest.Uri}: {failedTest.ErrorMessage}");
                    }
                    throw new Exception($"Unable to verify {_type} concept URIs");
                }
            }
        }

        private async Task<ConceptDataTestResult> TestConceptUriAsync(string uri, HttpClient client)
        {
            try
            {
                using (var req = new HttpRequestMessage(HttpMethod.Get, uri))
                {
                    using (var res = await client.SendAsync(req))
                    {
                        if (res.StatusCode != HttpStatusCode.OK)
                        {
                            return new ConceptDataTestResult(false, uri, $"Got HTTP status code {(int) res.StatusCode}");
                        }
                        return new ConceptDataTestResult(true, uri, null);
                    }
                }

            }
            catch (Exception e)
            {
                return new ConceptDataTestResult(false, uri, $"Error while testing URL: {e}");
            }
        }

        public class ConceptDataEntry
        {
            public string Name { get; }
            public string Uri { get; }

            public ConceptDataEntry(string name, string uri)
            {
                Name = name;
                Uri = uri;
            }
        }

        private class ConceptDataTestResult
        {
            public bool WasSuccessful { get; }
            public string Uri { get; }
            public string ErrorMessage { get; }

            public ConceptDataTestResult(bool wasSuccessful, string uri, string errorMessage)
            {
                WasSuccessful = wasSuccessful;
                Uri = uri;
                ErrorMessage = errorMessage;
            }
        }
    }
}
