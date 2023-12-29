namespace de4vmp.Core.Architecture.ExceptionHandlers.Variants; 

public class VmpFilterExceptionHandler : VmpExceptionHandlerBase {
    public VmpFilterExceptionHandler(uint handlerStart, uint filterStart) : base(handlerStart) {
        FilterStart = filterStart;
    }
    
    public uint FilterStart { get; }

    public override VmpExceptionHandlerType Type => VmpExceptionHandlerType.Filter;
}