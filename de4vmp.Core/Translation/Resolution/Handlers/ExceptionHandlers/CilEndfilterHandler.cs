using AsmResolver.DotNet.Code.Cil;
using AsmResolver.IO;
using AsmResolver.PE.DotNet.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.ExceptionHandlers;

public class CilEndfilterHandler : HandlerBase {
    private readonly IEnumerable<CilCode> _signature = new List<CilCode> {
        CilCode.Ldarg,
        CilCode.Call,
        CilCode.Callvirt,
        CilCode.Brfalse,
        CilCode.Ldarg,
        CilCode.Ldfld,
        CilCode.Callvirt,
        CilCode.Pop,
        CilCode.Ldarg,
        CilCode.Ldfld,
        CilCode.Ldarg,
        CilCode.Ldfld,
        CilCode.Newobj,
        CilCode.Callvirt,
        CilCode.Ldarg,
        CilCode.Ldarg,
        CilCode.Ldfld,
        CilCode.Callvirt,
        CilCode.Stfld,
        CilCode.Ldarg,
        CilCode.Ldnull,
        CilCode.Stfld,
        CilCode.Ret,
        CilCode.Ldarg,
        CilCode.Call,
        CilCode.Ret
    };

    private readonly IEnumerable<CilCode> _signatureOld = new List<CilCode> {
        CilCode.Ldarg,
        CilCode.Call,
        CilCode.Callvirt,
        CilCode.Brfalse,
        CilCode.Ldarg,
        CilCode.Ldfld,
        CilCode.Callvirt,
        CilCode.Pop,
        CilCode.Ldarg,
        CilCode.Ldfld,
        CilCode.Ldarg,
        CilCode.Ldfld,
        CilCode.Newobj,
        CilCode.Callvirt,
        CilCode.Ldarg,
        CilCode.Ldarg,
        CilCode.Ldfld,
        CilCode.Callvirt,
        CilCode.Stfld,
        CilCode.Ldarg,
        CilCode.Ldnull,
        CilCode.Stfld,
        CilCode.Ret,
        CilCode.Ldarg,
        CilCode.Ldarg,
        CilCode.Ldfld,
        CilCode.Call,
        CilCode.Ret
    };

    public override VmpCode Translates => VmpCode.CilEndfilterCode;

    public override bool Resolve(CilInstructionCollection instructions) {
        return instructions.AreSignatureEqual(_signature) || instructions.AreSignatureEqual(_signatureOld);
    }
}