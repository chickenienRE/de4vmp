using AsmResolver.DotNet.Code.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.Arrays; 

public class CilLdlenHandler : HandlerBase {
    public override VmpCode Translates => VmpCode.CilLdlenCode;
    
    public override bool Resolve(CilInstructionCollection instructions) {
        return instructions.HasTypeReference(nameof(Array)) &&
               instructions.HasMethodReference("get_Length");
    }
}