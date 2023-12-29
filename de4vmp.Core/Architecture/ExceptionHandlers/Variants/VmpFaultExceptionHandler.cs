namespace de4vmp.Core.Architecture.ExceptionHandlers.Variants; 

public class VmpFaultExceptionHandler : VmpExceptionHandlerBase {
    public VmpFaultExceptionHandler(uint handlerStart) : base(handlerStart) {
        
    }

    public override VmpExceptionHandlerType Type => VmpExceptionHandlerType.Fault;
}