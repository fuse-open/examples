using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Builder.Documents
{
    public class DocumentMetaData
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("synopsis")]
        public string Synopsis { get; set; }

        [JsonProperty("publishedAt")]
        public DateTime? PublishedAt { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("tags")]
        public List<string> Tags { get; set; }

        [JsonProperty("uxConcepts")]
        public List<string> UxConcepts { get; set; }

        [JsonProperty("jsConcepts")]
        public List<string> JsConcepts { get; set; }
    }
}