namespace Builder.Core
{
    public class BuilderSettings
    {
        public string RootPath { get; }
        public string BaseUrl { get; }

        public BuilderSettings(string rootPath, string baseUrl)
        {
            RootPath = rootPath;
            BaseUrl = baseUrl;
        }
    }
}
