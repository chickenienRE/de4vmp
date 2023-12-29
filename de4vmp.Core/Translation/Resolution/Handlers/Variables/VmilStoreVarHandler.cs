using AsmResolver.DotNet.Code.Cil;
using AsmResolver.PE.DotNet.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.Variables;

public class VmilStoreVarHandler : VariableHandlerBase {
    private readonly IEnumerable<CilCode> _signature = new List<CilCode> {
        CilCode.Ldarg,
        CilCode.Call,
        CilCode.Stloc,
        CilCode.Ldarg,
        CilCode.Call,
        CilCode.Stloc,
        CilCode.Ldarg,
        CilCode.Ldfld,
        CilCode.Ldloc,
        CilCode.Callvirt,
        CilCode.Stloc,
        CilCode.Ldloc,
        CilCode.Callvirt,
        CilCode.Brfalse,
        CilCode.Ldloc,
        CilCode.Callvirt,
        CilCode.Brtrue,
        CilCode.Newobj,
        CilCode.Throw,
        CilCode.Ldarg,
        CilCode.Ldfld,
        CilCode.Ldloc,
        CilCode.Ldloc,
        CilCode.Callvirt,
        CilCode.Ret,
        CilCode.Ldloc,
        CilCode.Ldarg,
        CilCode.Ldloc,
        CilCode.Ldloc,
        CilCode.Callvirt,
        CilCode.Call,
        CilCode.Callvirt,
        CilCode.Callvirt,
        CilCode.Ret
    };

    private readonly IEnumerable<CilCode> _signatureOld = new List<CilCode> {
        CilCode.Ldarg,
        CilCode.Call,
        CilCode.Stloc,
        CilCode.Ldarg,
        CilCode.Ldfld,
        CilCode.Ldarg,
        CilCode.Call,
        CilCode.Conv_U2,
        CilCode.Callvirt,
        CilCode.Stloc,
        CilCode.Ldloc,
        CilCode.Ldarg,
        CilCode.Ldloc,
        CilCode.Ldloc,
        CilCode.Callvirt,
        CilCode.Call,
        CilCode.Callvirt,
        CilCode.Callvirt,
        CilCode.Ret
    };

    public override VmpCode Translates => VmpCode.VmilStoreVarCode;

    public override bool Resolve(CilInstructionCollection instructions) {
        return instructions.AreSignatureEqual(_signature) || instructions.AreSignatureEqual(_signatureOld);
    }
}