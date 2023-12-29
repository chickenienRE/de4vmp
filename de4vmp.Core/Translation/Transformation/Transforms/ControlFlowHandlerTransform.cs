using AsmResolver.PE.DotNet.Cil;
using de4vmp.Core.Architecture;
using de4vmp.Core.Architecture.Annotations.ControlFlow;
using de4vmp.Core.Services;
using de4vmp.Core.Translation.Transformation.Collections.Branches;

namespace de4vmp.Core.Translation.Transformation.Transforms;

public class ControlFlowHandlerTransform : ITransform {
    public IEnumerable<VmpCode> Accepts {
        get { yield return VmpCode.VmilBrCode; }
    }

    public void Transform(VmpRecompiler recompiler, VmpInstruction instruction) {
        switch (instruction.Annotation) {
            case SingleJumpAnnotation annotation:
                HandleSingleJump(recompiler, instruction, annotation);
                break;
            case DoubleJumpAnnotation annotation:
                HandleDoubleJump(recompiler, instruction, annotation);
                break;
            case MultipleJumpAnnotation annotation:
                HandleMultipleJump(recompiler, instruction, annotation);
                break;
            default:
                throw ExceptionService.ThrowInvalidAnnotation(instruction);
        }
    }

    private static void HandleSingleJump(VmpRecompiler recompiler, VmpInstruction instruction,
        SingleJumpAnnotation annotation) {
        if (annotation.WillReturn) {
            if (recompiler.ReturnType.IsFullnameType(typeof(void)))
                recompiler.AddInstruction(new CilInstruction(CilOpCodes.Pop));

            recompiler.AddInstruction(new CilInstruction(CilOpCodes.Pop));
            recompiler.AddInstruction(instruction.Address, new CilInstruction(CilOpCodes.Ret));
            return;
        }

        recompiler.AddInstruction(new CilInstruction(CilOpCodes.Pop));
        recompiler.AddInstruction(instruction.Address, new CilInstruction(CilOpCodes.Br),
            new SingleReference(annotation.Address));
    }

    private static void HandleDoubleJump(VmpRecompiler recompiler, VmpInstruction instruction,
        DoubleJumpAnnotation annotation) {
        recompiler.AddInstruction(new CilInstruction(CilOpCodes.Pop));
        recompiler.AddInstruction(new CilInstruction(CilOpCodes.Pop));

        if (annotation.Comparision is not null) recompiler.AddInstruction(new CilInstruction(CilOpCodes.Pop));

        recompiler.AddInstruction(instruction.Address, new CilInstruction(CilOpCodes.Nop));

        if (annotation.Comparision is { } cmp)
            recompiler.AddInstruction(new CilInstruction(cmp.OpCode), new SingleReference(annotation.SecondAddress));
        else
            recompiler.AddInstruction(
                annotation.Type ? new CilInstruction(CilOpCodes.Brtrue) : new CilInstruction(CilOpCodes.Brfalse),
                new SingleReference(annotation.SecondAddress));

        recompiler.AddInstruction(new CilInstruction(CilOpCodes.Br), new SingleReference(annotation.FirstAddress));
    }

    private static void HandleMultipleJump(VmpRecompiler recompiler, VmpInstruction instruction,
        MultipleJumpAnnotation annotation) {
        recompiler.AddInstruction(instruction.Address, new CilInstruction(CilOpCodes.Switch),
            new MultiReference(annotation.Addresses));

        if (!recompiler.ReturnType.IsFullnameType(typeof(void)))
            recompiler.AddInstruction(new CilInstruction(CilOpCodes.Ldnull));

        recompiler.AddInstruction(new CilInstruction(CilOpCodes.Ret));
    }
}