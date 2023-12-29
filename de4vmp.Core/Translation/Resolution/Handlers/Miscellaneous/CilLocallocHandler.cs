using System.Runtime.InteropServices;
using AsmResolver.DotNet.Code.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.Miscellaneous; 

public class CilLocallocHandler : HandlerBase {
    public override VmpCode Translates => VmpCode.CilLocallocCode;
    
    public override bool Resolve(CilInstructionCollection instructions) {
        return instructions.HasMethodReference(nameof(Marshal.AllocHGlobal));
    }
}