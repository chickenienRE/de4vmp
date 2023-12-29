using AsmResolver.DotNet.Code.Cil;
using AsmResolver.PE.DotNet.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.Bitwise; 

public class CilNegHandler : BitwiseHandlerBase {
    public override VmpCode Translates => VmpCode.CilNegCode;
    
    public override bool Resolve(CilInstructionCollection instructions) {
        return IsSimpleSignature(instructions, CilCode.Neg);
    }
}