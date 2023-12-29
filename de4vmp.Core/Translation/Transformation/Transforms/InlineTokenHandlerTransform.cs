using AsmResolver.PE.DotNet.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Transformation.Transforms;

public class InlineTokenHandlerTransform : ITransform {
    private readonly IDictionary<VmpCode, CilOpCode> _mapping = new Dictionary<VmpCode, CilOpCode> {
        [VmpCode.CilCallCode] = CilOpCodes.Call,
        [VmpCode.CilCalliCode] = CilOpCodes.Calli,
        [VmpCode.CilCallvirtCode] = CilOpCodes.Callvirt,
        [VmpCode.VmilStaticCallCode] = CilOpCodes.Call,
        [VmpCode.VmilInstanceCallCode] = CilOpCodes.Callvirt,
        [VmpCode.CilNewobjCode] = CilOpCodes.Newobj,
        [VmpCode.CilStfldCode] = CilOpCodes.Stfld,
        [VmpCode.CilStsfldCode] = CilOpCodes.Stsfld,
        [VmpCode.CilLdstrCode] = CilOpCodes.Ldstr,
        [VmpCode.CilLdtokenCode] = CilOpCodes.Ldtoken,
        [VmpCode.CilIsinstCode] = CilOpCodes.Isinst,
        [VmpCode.CilCastclassCode] = CilOpCodes.Castclass,
        [VmpCode.CilNewarrCode] = CilOpCodes.Newarr,
        [VmpCode.CilUnboxCode] = CilOpCodes.Unbox,
        [VmpCode.CilUnboxAnyCode] = CilOpCodes.Unbox_Any,
        [VmpCode.CilLdftnCode] = CilOpCodes.Ldftn,
        [VmpCode.CilLdvirtftnCode] = CilOpCodes.Ldvirtftn,
        [VmpCode.CilSizeOfCode] = CilOpCodes.Sizeof,
        [VmpCode.CilInitobjCode] = CilOpCodes.Initobj,
        [VmpCode.VmilLoadTypeCode] = CilOpCodes.Nop
    };

    public IEnumerable<VmpCode> Accepts {
        get {
            yield return VmpCode.CilCallCode;
            yield return VmpCode.CilCalliCode;
            yield return VmpCode.CilCallvirtCode;
            yield return VmpCode.VmilStaticCallCode;
            yield return VmpCode.VmilInstanceCallCode;
            yield return VmpCode.CilNewobjCode;
            yield return VmpCode.CilStfldCode;
            yield return VmpCode.CilStsfldCode;
            yield return VmpCode.CilLdstrCode;
            yield return VmpCode.CilLdtokenCode;
            yield return VmpCode.CilIsinstCode;
            yield return VmpCode.CilCastclassCode;
            yield return VmpCode.CilNewarrCode;
            yield return VmpCode.CilUnboxCode;
            yield return VmpCode.CilUnboxAnyCode;
            yield return VmpCode.CilLdftnCode;
            yield return VmpCode.CilLdvirtftnCode;
            yield return VmpCode.CilSizeOfCode;
            yield return VmpCode.CilInitobjCode;
            yield return VmpCode.VmilLoadTypeCode;
        }
    }

    public void Transform(VmpRecompiler recompiler, VmpInstruction instruction) {
        recompiler.AddInstruction(new CilInstruction(CilOpCodes.Pop));
        recompiler.AddInstruction(instruction.Address,
            new CilInstruction(_mapping[instruction.Code], instruction.Operand));
    }
}