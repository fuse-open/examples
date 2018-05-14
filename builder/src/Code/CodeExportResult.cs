namespace Builder.Code
{
    public class CodeExportResult
    {
        public bool HasCode { get; }
        public string BundlePath { get; }

        public CodeExportResult(bool hasCode, string bundlePath)
        {
            HasCode = hasCode;
            BundlePath = bundlePath;
        }
    }
}