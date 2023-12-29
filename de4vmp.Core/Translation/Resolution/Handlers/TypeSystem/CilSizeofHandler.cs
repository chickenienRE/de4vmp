using System.Reflection.Emit;
using System.Runtime.InteropServices;
using AsmResolver.DotNet.Code.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.TypeSystem;

public class CilSizeofHandler : TypeSystemHandlerBase {
    public override VmpCode Translates => VmpCode.CilSizeOfCode;

    public override bool Resolve(CilInstructionCollection instructions) {
        return instructions.HasMethodReference(nameof(Marshal.SizeOf)) ||
               instructions.HasMethodReference(nameof(DynamicMethod.GetILGenerator));
    }
}