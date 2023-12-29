using AsmResolver.PE.DotNet.Cil;
using de4vmp.Core.Architecture;
using de4vmp.Core.Architecture.Annotations.ControlFlow;
using de4vmp.Core.Services;
using de4vmp.Core.Translation.Transformation.Collections.Branches;

namespace de4vmp.Core.Translation.Transformation.Transforms; 

public class HandlerLeaveHandlerTransform : ITransform {
    public IEnumerable<VmpCode> Accepts {
        get {
            yield return VmpCode.CilLeaveCode;
        }
    }
    
    public void Transform(VmpRecompiler recompiler, VmpInstruction instruction) {
        if (instruction.Annotation is not SingleJumpAnnotation annotation)
            throw ExceptionService.ThrowInvalidAnnotation(instruction);

        recompiler.AddInstruction(new CilInstruction(CilOpCodes.Pop));
        recompiler.AddInstruction(new CilInstruction(CilOpCodes.Pop));
        recompiler.AddInstruction(instruction.Address, new CilInstruction(CilOpCodes.Leave), new SingleReference(annotation.Address));
    }
}