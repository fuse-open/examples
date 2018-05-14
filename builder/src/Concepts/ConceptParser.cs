using System;
using System.Collections.Generic;
using System.IO;
using Builder.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Builder.Concepts
{
    public abstract class ConceptParser
    {
        private readonly ILogger<ConceptParser> _logger;
        private readonly BuilderSettings _settings;
        private readonly ConceptData _parsed;

        protected abstract string ConceptType { get; }

        public ConceptParser(ILogger<ConceptParser> logger, BuilderSettings settings)
        {
            _logger = logger;
            _settings = settings;
            _parsed = Read(ConceptType);
        }

        private ConceptData Read(string type)
        {
            var filename = Path.Combine(_settings.RootPath, $"{type}-concepts.json");
            if (!File.Exists(filename))
            {
                throw new ArgumentException($"Unable to find {type} concepts file at {filename}");
            }

            var result = JsonConvert.DeserializeObject<List<RawConceptEntry>>(File.ReadAllText(filename));
            if (result == null || result.Count == 0)
            {
                throw new ArgumentException($"Unable to deserialize {type} concept file {filename} - no concept definitions found");
            }

            var data = new ConceptData(type);
            foreach (var item in result)
            {
                var uri = $"/docs/{item.Uri}";
                data.Add(item.Name, item.Uri);
            }

            data.VerifyUrisOrThrow(_logger);
            return data;
        }

        public string GetUriFor(string name)
        {
            return _parsed.GetUriFor(name);
        }

        private class RawConceptEntry
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("uri")]
            public string Uri { get; set; }
        }
    }
}