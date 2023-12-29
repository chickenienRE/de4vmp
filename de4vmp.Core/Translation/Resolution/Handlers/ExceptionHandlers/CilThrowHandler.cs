using AsmResolver.DotNet.Code.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.ExceptionHandlers;

public class CilThrowHandler : HandlerBase {
    public override VmpCode Translates => VmpCode.CilThrowCode;

    public override bool Resolve(CilInstructionCollection instructions) {
        return instructions.HasTypeReference(nameof(Exception));
    }
}