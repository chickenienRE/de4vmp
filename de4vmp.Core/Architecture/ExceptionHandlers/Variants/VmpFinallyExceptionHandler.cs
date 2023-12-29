namespace de4vmp.Core.Architecture.ExceptionHandlers.Variants; 

public class VmpFinallyExceptionHandler : VmpExceptionHandlerBase {
    public VmpFinallyExceptionHandler(uint handlerStart) : base(handlerStart) {
        
    }

    public override VmpExceptionHandlerType Type => VmpExceptionHandlerType.Finally;
}