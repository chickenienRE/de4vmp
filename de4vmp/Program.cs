using System.Diagnostics;
using System.Reflection;
using de4vmp;
using de4vmp.Core;
using Figgle;
using Spectre.Console;

var settings = new AnsiConsoleSettings();
var console = AnsiConsole.Create(settings);

byte r = byte.MaxValue, g = byte.MaxValue, b = byte.MaxValue;

var versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
Console.Title = $"{versionInfo.ProductName} - {versionInfo.FileVersion} | {versionInfo.CompanyName}";

var font = FiggleFonts.SlantSmall;
string banner = font.Render(nameof(de4vmp));
foreach (string line in banner.Split(Environment.NewLine))
    console.WriteLine(line.PadLeft(Console.WindowWidth / 2 + line.Length / 2),
        new Style(new Color(r -= 27, g -= 18, b -= 9)));

const string tag = nameof(de4vmp
);

var stopwatch = Stopwatch.StartNew();
var logger = new ConsoleLogger(console, stopwatch);

try {
    logger.Information(tag, "Started devirtualization...");
    var context = new DevirtualizationContext(args);
    var devirtualizer = new Devirtualizer(logger);
    devirtualizer.Devirtualize(context);
}
catch (Exception exception) {
    console.WriteException(exception);

    if (exception is not DevirtualizationException) {
        console.WriteLine("Something went wrong! Please submit a bug report at the repository.");   
    }
}
finally {
    logger.Information(tag, $"Finished devirtualization in {stopwatch.ElapsedMilliseconds} Milliseconds...");
}

console.WriteLine("Press any key to continue...");
Console.ReadKey();