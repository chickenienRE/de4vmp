using AsmResolver.DotNet.Code.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.Arithmetic; 

public class CilDivHandler : ArithmeticHandlerBase {
    public override VmpCode Translates => VmpCode.CilDivSignedCode;
    
    public override bool Resolve(CilInstructionCollection instructions) {
        return IsDivSignature(instructions, false);
    }
}