using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Builder.Code
{
    /// Strips code snippet fragments from text, to prevent internal formatting
    /// from leaking into the generated content.
    public class CodeStripper
    {
        private static readonly Regex IsSnippetEndMarkerRegex = new Regex(@"^<!--[\s]*snippet-end[\s]*-->|^/\*[\s]*snippet-end[\s]*\*/|^//[\s]*snippet-end");
        private static readonly Regex IsSnippetStartMarkerRegex = new Regex(@"^<!--[\s]*snippet-begin:[^\s]+[\s]*-->|^/\*[\s]*snippet-begin:[^\s]+[\s]*\*/|^//[\s]*snippet-begin:[^\s]+");
        private static readonly Regex IsStripEndMarkerRegex = new Regex(@"^<!--[\s]*strip-end[\s]*-->|^/\*[\s]*strip-end[\s]*\*/|^//[\s]*strip-end");
        private static readonly Regex IsStripStartMarkerRegex = new Regex(@"^<!--[\s]*strip-begin:[\S\s]+-->|^/\*[\s]*strip-begin:[\S\s]+\*/|^//[\s]*strip-begin:[\S\s]+");

        public byte[] RemoveSnippets(string filename, byte[] buffer)
        {
            if (!SupportsSnippets(filename)) return buffer;

            var text = Encoding.UTF8.GetString(buffer);
            var lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            var validLines = (from line in lines
                              let strippedLine = line.Trim()
                              where !IsSnippetEndMarkerRegex.IsMatch(strippedLine) &&
                                    !IsSnippetStartMarkerRegex.IsMatch(strippedLine) &&
                                    !IsStripEndMarkerRegex.IsMatch(strippedLine) &&
                                    !IsStripStartMarkerRegex.IsMatch(strippedLine)
                              select line).ToList();
            return Encoding.UTF8.GetBytes(string.Join("\n", validLines));
        }

        private bool SupportsSnippets(string filename)
        {
            var lower = filename.ToLowerInvariant();
            return lower.EndsWith(".js") ||
                   lower.EndsWith(".ux") ||
                   lower.EndsWith(".uno");
        }
    }
}