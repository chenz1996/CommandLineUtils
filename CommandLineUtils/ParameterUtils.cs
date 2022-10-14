using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CommandLine;
using CommandLine.Text;

namespace CommandLineUtils
{
    public class InfoOptions
    {
        [Option('h', Default = false, Required = false)]
        public bool showHelp { get; set; }
        
        [Option('v', Default = false, Required = false)]
        public bool showVersion { get; set; }
        
        [Option('a', "author", Default = false, Required = false)]
        public bool showAuthor { get; set; }
    }
    public static class ParameterUtils<T>
    {
        public static void HandledEventArgs(string[] args, Action<T> onResult)
        {
            var parser = new Parser(settings =>
            {
                settings.IgnoreUnknownArguments = true;
            });
            
            var parserResult = parser.ParseArguments<T>(args);
            parser.Dispose();
            
            var helpText = HelpText.AutoBuild(parserResult, h =>
            {
                h.AdditionalNewLineAfterOption = false; //remove newline between options
                return h;
            }, e => e);
            
            var quit = false;
            parser = new Parser(settings =>
            {
                settings.IgnoreUnknownArguments = true;
                settings.AllowMultiInstance = true;
                settings.HelpWriter = Console.Out;
            });
            parser.ParseArguments<InfoOptions>(args).WithParsed(
                infos =>
                {
                    quit = true;
                    if (infos.showHelp)
                    {
                        Console.WriteLine(helpText);
                        return;
                    }
                    
                    if (infos.showVersion)
                    {
                        var assembly = typeof(T).Assembly.GetName();
                        Console.WriteLine($"{assembly.Name} {assembly.Version}");
                        return;
                    }

                    if (infos.showAuthor)
                    {
                        Console.WriteLine("Author: Plin Chen");
                        return;
                    }
                    quit = false;
                });
            parser.Dispose();
            if (quit)
            {
                System.Environment.Exit(0);
            }

            parserResult.WithParsed(onResult).WithNotParsed(errors=>HandleParseError(errors, parserResult, helpText));
        }
        
        private static void HandleParseError(IEnumerable<Error> errors,  ParserResult<T> parserResult, HelpText helpText)
        {
            // filter errors: drop errors typed HelpRequestedError or VersionRequestedError
            var validErrors = errors.Where(error => error != null && error.Tag != ErrorType.HelpRequestedError && error.Tag != ErrorType.VersionRequestedError).ToList();
            // filter errors: handle errors typed MissingRequiredOptionError manually
            var missingRequiredOptionErrors =
                validErrors.Where(error => error.Tag is ErrorType.MissingRequiredOptionError).ToList();
            if (missingRequiredOptionErrors.Any())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Missing required options]");
                
                var builder = SentenceBuilder.Create();
                var errorMessages = HelpText.RenderParsingErrorsTextAsLines(parserResult, builder.FormatError, builder.FormatMutuallyExclusiveSetErrors, 0).ToList();
                errorMessages.ForEach(Console.WriteLine);
                Console.ResetColor();
                Console.WriteLine($"\n[Help]\n{helpText}");
            }
            // print other errors
            validErrors.Except(missingRequiredOptionErrors).ToList().ForEach(Console.WriteLine);
        }
    }
}