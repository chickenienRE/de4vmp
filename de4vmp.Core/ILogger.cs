namespace de4vmp.Core; 

public interface ILogger {
    public void Debug(object sender, string message);
    public void Warning(object sender, string message);
    public void Information(object sender, string message);
}