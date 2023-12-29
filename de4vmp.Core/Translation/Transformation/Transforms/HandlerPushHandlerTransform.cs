using AsmResolver.PE.DotNet.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Transformation.Transforms;

public class HandlerPushHandlerTransform : ITransform {
    public IEnumerable<VmpCode> Accepts {
        get { yield return VmpCode.VmilPushEhCode; }
    }

    public void Transform(VmpRecompiler recompiler, VmpInstruction instruction) {
        recompiler.AddInstruction(new CilInstruction(CilOpCodes.Pop));
        recompiler.AddInstruction(instruction.Address, new CilInstruction(CilOpCodes.Nop));
    }
}