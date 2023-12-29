using AsmResolver.DotNet.Code.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.TypeSystem;

public class CilUnboxAnyHandler : TypeSystemHandlerBase {
    public override VmpCode Translates => VmpCode.CilUnboxAnyCode;

    public override bool Resolve(CilInstructionCollection instructions) {
        return instructions.HasExceptionReference(nameof(InvalidCastException)) &&
               instructions.HasExceptionReference(nameof(NullReferenceException));
    }
}