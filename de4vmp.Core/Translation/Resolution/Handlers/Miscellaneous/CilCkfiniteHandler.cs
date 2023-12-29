using AsmResolver.DotNet.Code.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.Miscellaneous;

public class CilCkfiniteHandler : HandlerBase {
    public override VmpCode Translates => VmpCode.CilCkfiniteCode;

    public override bool Resolve(CilInstructionCollection instructions) {
        return instructions.HasExceptionReference(nameof(OverflowException));
    }
}