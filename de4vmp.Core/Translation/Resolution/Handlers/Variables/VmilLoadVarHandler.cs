using AsmResolver.DotNet.Code.Cil;
using AsmResolver.PE.DotNet.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.Variables;

public class VmilLoadVarHandler : VariableHandlerBase {
    private readonly IEnumerable<CilCode> _signature = new List<CilCode> {
        CilCode.Ldarg,
        CilCode.Ldarg,
        CilCode.Ldfld,
        CilCode.Ldarg,
        CilCode.Call,
        CilCode.Callvirt,
        CilCode.Callvirt,
        CilCode.Call,
        CilCode.Ret
    };

    private readonly IEnumerable<CilCode> _signatureOld = new List<CilCode> {
        CilCode.Ldarg,
        CilCode.Ldarg,
        CilCode.Ldfld,
        CilCode.Ldarg,
        CilCode.Call,
        CilCode.Conv_U2,
        CilCode.Callvirt,
        CilCode.Callvirt,
        CilCode.Call,
        CilCode.Ret
    };

    public override VmpCode Translates => VmpCode.VmilLoadVarCode;

    public override bool Resolve(CilInstructionCollection instructions) {
        return instructions.AreSignatureEqual(_signature) || instructions.AreSignatureEqual(_signatureOld);
    }
}