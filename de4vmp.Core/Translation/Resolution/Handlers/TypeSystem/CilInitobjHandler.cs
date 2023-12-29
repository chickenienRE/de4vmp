using System.Runtime.Serialization;
using AsmResolver.DotNet.Code.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.TypeSystem;

public class CilInitobjHandler : TypeSystemHandlerBase {
    public override VmpCode Translates => VmpCode.CilInitobjCode;

    public override bool Resolve(CilInstructionCollection instructions) {
        return instructions.HasMethodReference(nameof(FormatterServices.GetUninitializedObject)) ||
               instructions.HasMethodReference(nameof(Type.GetGenericTypeDefinition));
    }
}