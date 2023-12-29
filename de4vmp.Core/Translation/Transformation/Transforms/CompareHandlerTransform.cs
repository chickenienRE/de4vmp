using AsmResolver.PE.DotNet.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Transformation.Transforms;

public class CompareHandlerTransform : ITransform {
    public IEnumerable<VmpCode> Accepts {
        get {
            yield return VmpCode.VmilCmpSignedCode;
            yield return VmpCode.VmilCmpUnsignedCode;
        }
    }

    public void Transform(VmpRecompiler recompiler, VmpInstruction instruction) {
        recompiler.AddInstruction(new CilInstruction(CilOpCodes.Pop));
        recompiler.AddInstruction(instruction.Address, new CilInstruction(CilOpCodes.Nop));
        recompiler.AddInstruction(new CilInstruction(CilOpCodes.Ldc_I4_1));
    }
}