using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using CommonMark;

namespace Builder.Core
{
    public class MarkdownParser
    {
        private readonly ILogger<MarkdownParser> _logger;

        public MarkdownParser(ILogger<MarkdownParser> logger)
        {
            _logger = logger;
        }

        public Task<string> ParseAsync(string markdown)
        {
            var html = CommonMarkConverter.Convert(markdown);
            return Task.FromResult(ApplyPostProcessing(html));
        }

        private string ApplyPostProcessing(string html)
        {
            html = ResizeHeaderLevels(html);
            return html;
        }

        private string ResizeHeaderLevels(string html)
        {
            // Hack to increase header levels a little in markdown to make the pages look better.
            // H1 is changed to H3, H2 to H4 and so on.
            for (var i = 4; i >= 0; i--)
            {
                var prevStart = $"<h{i}";
                var prevEnd = $"</h{i}>";
                var newStart = $"<h{i + 2}";
                var newEnd = $"</h{i + 2}";
                html = html.Replace(prevStart, newStart)
                           .Replace(prevEnd, newEnd);
            }

            return html;
        }
    }
}
