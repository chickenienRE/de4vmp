namespace de4vmp.Core; 

public class DevirtualizationException : Exception {
    public DevirtualizationException(string message) : base(message) {
        
    }

    public DevirtualizationException(string message, Exception exception) : base(message, exception) {
        
    }
}