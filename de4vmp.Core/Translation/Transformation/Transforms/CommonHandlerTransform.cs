using AsmResolver.PE.DotNet.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Transformation.Transforms;

public class CommonHandlerTransform : ITransform {
    private readonly IDictionary<VmpCode, CilOpCode> _mapping = new Dictionary<VmpCode, CilOpCode> {
        [VmpCode.CilAddCode] = CilOpCodes.Add,
        [VmpCode.CilAddOvfCode] = CilOpCodes.Add_Ovf,
        [VmpCode.CilAddOvfUnCode] = CilOpCodes.Add_Ovf_Un,
        [VmpCode.CilSubCode] = CilOpCodes.Sub,
        [VmpCode.CilSubOvfCode] = CilOpCodes.Sub_Ovf,
        [VmpCode.CilSubOvfUnCode] = CilOpCodes.Sub_Ovf_Un,
        [VmpCode.CilMulCode] = CilOpCodes.Mul,
        [VmpCode.CilMulOvfCode] = CilOpCodes.Mul_Ovf,
        [VmpCode.CilMulOvfUnCode] = CilOpCodes.Mul_Ovf_Un,
        [VmpCode.CilDivSignedCode] = CilOpCodes.Div,
        [VmpCode.CilDivUnsignedCode] = CilOpCodes.Div_Un,
        [VmpCode.CilRemSignedCode] = CilOpCodes.Rem,
        [VmpCode.CilRemUnsignedCode] = CilOpCodes.Rem_Un,
        [VmpCode.CilShrSignedCode] = CilOpCodes.Shr,
        [VmpCode.CilShrUnsignedCode] = CilOpCodes.Shr_Un,
        [VmpCode.CilOrCode] = CilOpCodes.Or,
        [VmpCode.CilXorCode] = CilOpCodes.Xor,
        [VmpCode.CilShlCode] = CilOpCodes.Shl,
        [VmpCode.CilNotCode] = CilOpCodes.Not,
        [VmpCode.CilNegCode] = CilOpCodes.Neg,
        [VmpCode.CilDupCode] = CilOpCodes.Dup,
        [VmpCode.CilPopCode] = CilOpCodes.Pop,
        [VmpCode.CilLdlenCode] = CilOpCodes.Ldlen,
        [VmpCode.CilLdnullCode] = CilOpCodes.Ldnull,
        [VmpCode.CilCkfiniteCode] = CilOpCodes.Ckfinite,
        [VmpCode.CilLocallocCode] = CilOpCodes.Localloc,
        [VmpCode.CilRethrowCode] = CilOpCodes.Rethrow,
        [VmpCode.CilThrowCode] = CilOpCodes.Throw,
        [VmpCode.CilEndfilterCode] = CilOpCodes.Endfilter
    };

    public IEnumerable<VmpCode> Accepts {
        get {
            yield return VmpCode.CilAddCode;
            yield return VmpCode.CilAddOvfCode;
            yield return VmpCode.CilAddOvfUnCode;
            yield return VmpCode.CilSubCode;
            yield return VmpCode.CilSubOvfCode;
            yield return VmpCode.CilSubOvfUnCode;
            yield return VmpCode.CilMulCode;
            yield return VmpCode.CilMulOvfCode;
            yield return VmpCode.CilMulOvfUnCode;
            yield return VmpCode.CilDivSignedCode;
            yield return VmpCode.CilDivUnsignedCode;
            yield return VmpCode.CilRemSignedCode;
            yield return VmpCode.CilRemUnsignedCode;
            yield return VmpCode.CilShrSignedCode;
            yield return VmpCode.CilShrUnsignedCode;
            yield return VmpCode.CilOrCode;
            yield return VmpCode.CilXorCode;
            yield return VmpCode.CilShlCode;
            yield return VmpCode.CilNotCode;
            yield return VmpCode.CilNegCode;
            yield return VmpCode.CilDupCode;
            yield return VmpCode.CilPopCode;
            yield return VmpCode.CilLdlenCode;
            yield return VmpCode.CilLdnullCode;
            yield return VmpCode.CilCkfiniteCode;
            yield return VmpCode.CilLocallocCode;
            yield return VmpCode.CilRethrowCode;
            yield return VmpCode.CilThrowCode;
            yield return VmpCode.CilEndfilterCode;
        }
    }

    public void Transform(VmpRecompiler recompiler, VmpInstruction instruction) {
        recompiler.AddInstruction(instruction.Address, new CilInstruction(_mapping[instruction.Code]));
    }
}