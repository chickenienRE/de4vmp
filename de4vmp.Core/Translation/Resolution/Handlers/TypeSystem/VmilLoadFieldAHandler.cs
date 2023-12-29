using AsmResolver.DotNet.Code.Cil;
using AsmResolver.PE.DotNet.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.TypeSystem;

public class VmilLoadFieldAHandler : TypeSystemHandlerBase {
    private readonly IEnumerable<CilCode> _signature = new List<CilCode> {
        CilCode.Ldarg,
        CilCode.Ldarg,
        CilCode.Call,
        CilCode.Callvirt,
        CilCode.Call,
        CilCode.Stloc,
        CilCode.Ldarg,
        CilCode.Call,
        CilCode.Stloc,
        CilCode.Ldarg,
        CilCode.Ldloc,
        CilCode.Ldloc,
        CilCode.Newobj,
        CilCode.Call,
        CilCode.Ret
    };

    private readonly IEnumerable<CilCode> _signatureOld = new List<CilCode> {
        CilCode.Ldarg,
        CilCode.Ldarg,
        CilCode.Call,
        CilCode.Callvirt,
        CilCode.Call,
        CilCode.Stloc,
        CilCode.Ldarg,
        CilCode.Call,
        CilCode.Callvirt,
        CilCode.Stloc,
        CilCode.Ldloc,
        CilCode.Callvirt,
        CilCode.Brtrue,
        CilCode.Ldloc,
        CilCode.Brtrue,
        CilCode.Newobj,
        CilCode.Throw,
        CilCode.Ldarg,
        CilCode.Ldloc,
        CilCode.Ldloc,
        CilCode.Newobj,
        CilCode.Call,
        CilCode.Ret
    };

    public override VmpCode Translates => VmpCode.VmilLoadFieldACode;

    public override bool Resolve(CilInstructionCollection instructions) {
        return instructions.AreSignatureEqual(_signature) || instructions.AreSignatureEqual(_signatureOld);
    }
}