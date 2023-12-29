using AsmResolver.DotNet.Code.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.Arithmetic; 

public class CilMulHandler : ArithmeticHandlerBase {
    public override VmpCode Translates => VmpCode.CilMulCode;
    
    public override bool Resolve(CilInstructionCollection instructions) {
        return IsMulSignature(instructions, false, false);
    }
}