using Builder.Core;
using Microsoft.Extensions.Logging;

namespace Builder.Concepts
{
    public class UxConcepts : ConceptParser
    {
        protected override string ConceptType { get; } = "ux";
        public UxConcepts(ILogger<JsConcepts> logger, BuilderSettings settings) : base(logger, settings) {}
    }
}