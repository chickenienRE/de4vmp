using AsmResolver.DotNet.Code.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.Arithmetic; 

public class CilMulOvfHandler : ArithmeticHandlerBase {
    public override VmpCode Translates => VmpCode.CilMulOvfCode;
    
    public override bool Resolve(CilInstructionCollection instructions) {
        return IsMulSignature(instructions, true, false);
    }
}