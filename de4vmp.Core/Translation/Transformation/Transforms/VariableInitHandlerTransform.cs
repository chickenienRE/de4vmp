using AsmResolver.PE.DotNet.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Transformation.Transforms;

public class VariableInitHandlerTransform : ITransform {
    public IEnumerable<VmpCode> Accepts {
        get { yield return VmpCode.VmilInitVarCode; }
    }

    public void Transform(VmpRecompiler recompiler, VmpInstruction instruction) {
        recompiler.AddInstruction(new CilInstruction(CilOpCodes.Pop));
        recompiler.AddInstruction(new CilInstruction(CilOpCodes.Pop));
        recompiler.AddInstruction(new CilInstruction(CilOpCodes.Pop));
        recompiler.AddInstruction(instruction.Address, new CilInstruction(CilOpCodes.Nop));
    }
}