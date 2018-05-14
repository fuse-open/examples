using System;
using System.Linq;
using Builder.Code;
using Builder.Concepts;
using Builder.Core;
using Builder.Documents;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace Builder
{
    public class Program
    {
        public static int Main(string[] args)
        {
            if (args?.Length < 2)
            {
                Console.Error.WriteLine($"Usage: builder.exe [path to repository root directory] [base URL] (--debug)");
            }

            var rootDirectory = args[0];
            var baseUrl = args[1];
            var isDebug = args.Contains("--debug");

            var settings = new BuilderSettings(rootDirectory, baseUrl);
            var services = new ServiceCollection().AddLogging()

                                                  // Core features
                                                  .AddSingleton(settings)
                                                  .AddSingleton<OutputDir>()
                                                  .AddSingleton<MarkdownParser>()

                                                  // Code features
                                                  .AddSingleton<CodeExporter>()
                                                  .AddSingleton<CodeStripper>()

                                                  // Concept features
                                                  .AddSingleton<JsConcepts>()
                                                  .AddSingleton<UxConcepts>()

                                                  // Document features
                                                  .AddSingleton<Layout>()
                                                  .AddSingleton<ExampleExporter>()
                                                  .AddSingleton<ExternalExampleExporter>()
                                                  .AddSingleton<ExampleIndexExporter>()

                                                  // CLI
                                                  .AddSingleton<Cli>()

                                                  .BuildServiceProvider();
            services.GetRequiredService<ILoggerFactory>()
                    .AddProvider(new ConsoleLoggerProvider(GetLoggingFilter(isDebug), true));

            try
            {
                services.GetRequiredService<Cli>().BuildAsync().Wait();
                return 0;
            }
            catch (AggregateException e)
            {
                Console.Error.WriteLine($"Builder failed:");
                foreach (var i in e.InnerExceptions)
                {
                    Console.Error.WriteLine(i);
                }
                return 1;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Generator failed: {e}");
                return 1;
            }
        }

        private static Func<string, LogLevel, bool> GetLoggingFilter(bool isDebug)
        {
            return (logger, logLevel) => true;
        }
    }
}
