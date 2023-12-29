using System.Reflection;
using AsmResolver.DotNet.Code.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.TypeSystem;

public class VmilLoadFieldHandler : TypeSystemHandlerBase {
    public override VmpCode Translates => VmpCode.VmilLoadFieldCode;

    public override bool Resolve(CilInstructionCollection instructions) {
        return !instructions.HasTypeReference(nameof(Array)) &&
               instructions.HasMethodReference(nameof(FieldInfo.GetValue));
    }
}