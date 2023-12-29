using AsmResolver.DotNet.Code.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.TypeSystem;

public class CilCastclassHandler : TypeSystemHandlerBase {
    public override VmpCode Translates => VmpCode.CilCastclassCode;

    public override bool Resolve(CilInstructionCollection instructions) {
        return instructions.HasExceptionReference(nameof(InvalidCastException)) &&
               !instructions.HasExceptionReference(nameof(NullReferenceException));
    }
}