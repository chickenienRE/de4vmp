using System.Reflection;
using AsmResolver.DotNet.Code.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.TypeSystem;

public class CilCallvirtHandler : TypeSystemHandlerBase {
    public override VmpCode Translates => VmpCode.CilCallvirtCode;

    public override bool Resolve(CilInstructionCollection instructions) {
        return instructions.HasMethodReference(nameof(Type.GetMethod)) &&
               !instructions.HasMethodReference(nameof(MethodInfo.GetBaseDefinition));
    }
}