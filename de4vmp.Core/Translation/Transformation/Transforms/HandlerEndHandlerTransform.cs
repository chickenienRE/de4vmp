using AsmResolver.PE.DotNet.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Transformation.Transforms;

public class HandlerEndHandlerTransform : ITransform {
    public IEnumerable<VmpCode> Accepts {
        get { yield return VmpCode.CilEndfinallyCode; }
    }

    public void Transform(VmpRecompiler recompiler, VmpInstruction instruction) {
        recompiler.AddInstruction(instruction.Address, new CilInstruction(CilOpCodes.Endfinally));
        recompiler.AddInstruction(new CilInstruction(CilOpCodes.Nop));
    }
}