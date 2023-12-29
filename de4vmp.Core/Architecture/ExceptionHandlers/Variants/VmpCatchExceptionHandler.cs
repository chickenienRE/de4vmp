using AsmResolver.DotNet;

namespace de4vmp.Core.Architecture.ExceptionHandlers.Variants; 

public class VmpCatchExceptionHandler : VmpExceptionHandlerBase {
    public VmpCatchExceptionHandler(uint handlerStart, ITypeDescriptor typeDescriptor) : base(handlerStart) {
        CatchType = typeDescriptor;
    }

    public ITypeDescriptor CatchType { get; }

    public override VmpExceptionHandlerType Type => VmpExceptionHandlerType.Catch;
}