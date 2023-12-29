using System.Diagnostics;
using de4vmp.Core;
using Spectre.Console;

namespace de4vmp; 

public class ConsoleLogger : ILogger {
    private readonly Style _debugStyle = new(Color.FromConsoleColor(ConsoleColor.Gray));
    private readonly Style _warningStyle = new(Color.FromConsoleColor(ConsoleColor.DarkRed));
    private readonly Style _informationStyle = new(Color.FromConsoleColor(ConsoleColor.White));
    
    private readonly IAnsiConsole _console;
    private readonly Stopwatch _stopwatch;

    public ConsoleLogger(IAnsiConsole console, Stopwatch stopwatch) {
        _console = console ?? throw new ArgumentNullException(nameof(console));
        _stopwatch = stopwatch ?? throw new ArgumentNullException(nameof(stopwatch));
    }
    
    public void Debug(object sender, string message) => 
        WriteMessage(sender, message, _debugStyle);

    public void Warning(object sender, string message) =>
        WriteMessage(sender, message, _warningStyle);

    public void Information(object sender, string message) =>
        WriteMessage(sender, message, _informationStyle);

    private void WriteMessage(object sender, string message, Style style) {
        var elapsed = _stopwatch.Elapsed;
        string callee = sender as string ?? sender.GetType().Name;
        
        _console.WriteLine($"{elapsed.Minutes:00}:{elapsed.Seconds:00}:{elapsed.Milliseconds:000} | [{callee}]: {message}", style);
    }
}