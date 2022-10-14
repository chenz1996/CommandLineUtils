using System;
using CommandLine;
using CommandLineUtils;

namespace CommandLineSample
{
    internal class Parameters
    {
        [Option('l', "logs", Default = false, HelpText = "Show logs in console.")]
        public bool ShowLogs { get; set; }
        
        [Option('f', "file", Required = true, HelpText = "Input file to be processed.")]
        public string fileName { get; set; }
        
        [Option('n', "num", Required = true, HelpText = "The number.")]
        public int number { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ParameterUtils<Parameters>.HandledEventArgs(args, RunWithOptions);
        }

        private static void RunWithOptions(Parameters parameters)
        {
            Console.WriteLine("Running with options:");
            Console.WriteLine($"Show logs: {parameters.ShowLogs}");
            Console.WriteLine($"File name: {parameters.fileName}");
        }
    }
}