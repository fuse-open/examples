using Builder.Core;
using Microsoft.Extensions.Logging;

namespace Builder.Concepts
{
    public class JsConcepts : ConceptParser
    {
        protected override string ConceptType { get; } = "js";
        public JsConcepts(ILogger<JsConcepts> logger, BuilderSettings settings) : base(logger, settings) {}
    }
}