using AsmResolver.PE.DotNet.Cil;
using de4vmp.Core.Architecture;
using de4vmp.Core.Architecture.Annotations;

namespace de4vmp.Core.Translation.Transformation.Transforms;

public class LdelemaHandlerTransform : ITransform {
    public IEnumerable<VmpCode> Accepts {
        get { yield return VmpCode.VmilLdelemaCode; }
    }

    public void Transform(VmpRecompiler recompiler, VmpInstruction instruction) {
        if (instruction.Annotation is NopAnnotation) {
            recompiler.AddInstruction(new CilInstruction(CilOpCodes.Pop));
            recompiler.AddInstruction(instruction.Address, new CilInstruction(CilOpCodes.Nop));
        }
        else {
            recompiler.AddInstruction(instruction.Address,
                new CilInstruction(CilOpCodes.Ldelema, recompiler.TypeFactory.Object.ToTypeDefOrRef()));
        }
    }
}