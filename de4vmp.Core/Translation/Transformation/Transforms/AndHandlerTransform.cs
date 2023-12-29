using AsmResolver.PE.DotNet.Cil;
using de4vmp.Core.Architecture;
using de4vmp.Core.Architecture.Annotations.Comparision;

namespace de4vmp.Core.Translation.Transformation.Transforms;

public class AndHandlerTransform : ITransform {
    public IEnumerable<VmpCode> Accepts {
        get { yield return VmpCode.CilAndCode; }
    }

    public void Transform(VmpRecompiler recompiler, VmpInstruction instruction) {
        if (instruction.Annotation is VmpCmp annotation) {
            recompiler.AddInstruction(new CilInstruction(CilOpCodes.Pop));
            recompiler.AddInstruction(new CilInstruction(CilOpCodes.Pop));
            recompiler.AddInstruction(new CilInstruction(annotation.OpCode));
        }
        else {
            recompiler.AddInstruction(instruction.Address, new CilInstruction(CilOpCodes.And));
        }
    }
}