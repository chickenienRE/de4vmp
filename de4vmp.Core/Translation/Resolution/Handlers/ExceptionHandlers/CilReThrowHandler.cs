using AsmResolver.DotNet.Code.Cil;
using AsmResolver.PE.DotNet.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.ExceptionHandlers;

public class CilReThrowHandler : HandlerBase {
    private readonly IEnumerable<CilCode> _signature = new List<CilCode> {
        CilCode.Ldarg,
        CilCode.Ldfld,
        CilCode.Brtrue,
        CilCode.Newobj,
        CilCode.Throw,
        CilCode.Ldarg,
        CilCode.Ldfld,
        CilCode.Throw
    };

    public override VmpCode Translates => VmpCode.CilRethrowCode;

    public override bool Resolve(CilInstructionCollection instructions) {
        return instructions.AreSignatureEqual(_signature);
    }
}