using System.Reflection;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.PE.DotNet.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.TypeSystem;

public class CilStfldHandler : TypeSystemHandlerBase {
    private readonly IEnumerable<CilCode> _signature = new List<CilCode> {
        CilCode.Ldarg,
        CilCode.Ldarg,
        CilCode.Call,
        CilCode.Callvirt,
        CilCode.Call,
        CilCode.Stloc,
        CilCode.Ldarg,
        CilCode.Call,
        CilCode.Stloc,
        CilCode.Ldarg,
        CilCode.Call,
        CilCode.Dup,
        CilCode.Callvirt,
        CilCode.Pop,
        CilCode.Ldloc,
        CilCode.Ldarg,
        CilCode.Ldloc,
        CilCode.Ldloc,
        CilCode.Callvirt,
        CilCode.Call,
        CilCode.Callvirt,
        CilCode.Callvirt,
        CilCode.Ret
    };

    public override VmpCode Translates => VmpCode.CilStfldCode;

    public override bool Resolve(CilInstructionCollection instructions) {
        return (instructions.HasMethodReference(nameof(FieldInfo.SetValue)) &&
                instructions.HasExceptionReference(nameof(NullReferenceException))) ||
               instructions.AreSignatureEqual(_signature);
    }
}