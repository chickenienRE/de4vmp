namespace de4vmp.Core.Pipeline; 

public interface IPhase {
    public void Run(ILogger logger, DevirtualizationContext context);
}