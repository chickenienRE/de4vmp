using AsmResolver.PE.DotNet.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Transformation.Transforms;

public class ConstantHandlerTransform : ITransform {
    private readonly IDictionary<VmpCode, CilOpCode> _mapping = new Dictionary<VmpCode, CilOpCode> {
        [VmpCode.CilLdcI4Code] = CilOpCodes.Ldc_I4,
        [VmpCode.CilLdcI8Code] = CilOpCodes.Ldc_I8,
        [VmpCode.CilLdcR4Code] = CilOpCodes.Ldc_R4,
        [VmpCode.CilLdcR8Code] = CilOpCodes.Ldc_R8
    };

    public IEnumerable<VmpCode> Accepts {
        get {
            yield return VmpCode.CilLdcI4Code;
            yield return VmpCode.CilLdcI8Code;
            yield return VmpCode.CilLdcR4Code;
            yield return VmpCode.CilLdcR8Code;
        }
    }

    public void Transform(VmpRecompiler recompiler, VmpInstruction instruction) {
        recompiler.AddInstruction(instruction.Address,
            new CilInstruction(_mapping[instruction.Code], instruction.Operand));
    }
}