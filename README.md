# About CommandLineUtils
CommandLineUtils is a library for parsing command line parameters in C# applications. It provides a set of abstractions for parsing commandlines that are testable, easy to configure, and easy to extend.

## Getting Started
1. Download the CommandLineUtils package.
2. Building Project with CommandLineUtils
3. Create a new class with a `Main` method.
4. Create a new class for command line options, such as `Parameters`.
    ```csharp
   class Parameters
    {
        [Option('l', "logs", Default = false, HelpText = "Show logs in console.")]
        public bool ShowLogs { get; set; }
        
        [Option('f', "file", Required = true, HelpText = "Input file to be processed.")]
        public string fileName { get; set; }
        
        [Option('n', "num", Required = true, HelpText = "The number.")]
        public int number { get; set; }
    }
   ```
5. In the `Main` method, use `ParameterUtils<Parameters>.HandledEventArgs(args, RunWithOptions)` to parse the command line arguments and run the program.

    ```csharp
    static void Main(string[] args)
    {
        ParameterUtils<Parameters>.HandledEventArgs(args, RunWithOptions);
    }

    private static void RunWithOptions(Parameters parameters)
    {
        Console.WriteLine("Running with options:");
        Console.WriteLine($"Show logs: {parameters.ShowLogs}");
        Console.WriteLine($"File name: {parameters.fileName}");
        Console.WriteLine($"Number: {parameters.number}");
    }
    ```
## Build and run
1. Build your program and run with command line arguments.
    ```bash
    ./MyApp -f "file.txt" -n 10
    ```
    The output should be:
    ```bash
    Running with options:
    Show logs: False
    File name: test.txt
    Number: 10
    ```
2. Run with help option.
    ```bash
    ./MyApp -h
    ```
    The output should be:
    ```bash
    Usage: MyApp [options]
    
    Options:
    -l, --logs    Show logs in console.
    -f, --file    Input file to be processed.
    -n, --num     The number.
    -?, -h, --help    Show help and usage information
    ```
3. Run with version option.
    ```bash
    ./MyApp -v
    ```
    The output should be:
    ```bash
    MyApp 1.2.3.4
    ```
## Dependency
The util **CommandLineUtils** depends on [CommandLineParser](https://www.nuget.org/packages/CommandLineParser/).

## License
The util **CommandLineUtils** is licensed under the [MIT license](https://en.wikipedia.org/wiki/MIT_License).