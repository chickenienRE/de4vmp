using AsmResolver.DotNet.Code.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.TypeSystem;

public class CilLdtokenHandler : TypeSystemHandlerBase {
    public override VmpCode Translates => VmpCode.CilLdtokenCode;

    public override bool Resolve(CilInstructionCollection instructions) {
        return instructions.HasMethodReference(nameof(ModuleHandle.ResolveFieldHandle));
    }
}