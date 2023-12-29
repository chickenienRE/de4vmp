using AsmResolver.PE.DotNet.Cil;
using de4vmp.Core.Architecture;
using de4vmp.Core.Architecture.Annotations;

namespace de4vmp.Core.Translation.Transformation.Transforms;

public class LdindI4HandlerTransform : ITransform {
    public IEnumerable<VmpCode> Accepts {
        get { yield return VmpCode.VmilLdindI4Code; }
    }

    public void Transform(VmpRecompiler recompiler, VmpInstruction instruction) {
        if (instruction.Annotation is NopAnnotation) {
            recompiler.InsertInstruction(3, new CilInstruction(CilOpCodes.Ldc_I4_1));

            recompiler.AddInstruction(new CilInstruction(CilOpCodes.Pop));
            recompiler.AddInstruction(instruction.Address, new CilInstruction(CilOpCodes.Nop));
        }
        else {
            recompiler.AddInstruction(instruction.Address, new CilInstruction(CilOpCodes.Ldind_I4));
        }
    }
}