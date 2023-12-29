using AsmResolver.DotNet.Code.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.Arithmetic; 

public class CilShrHandler : ArithmeticHandlerBase {
    public override VmpCode Translates => VmpCode.CilShrSignedCode;
    
    public override bool Resolve(CilInstructionCollection instructions) {
        return IsShrSignature(instructions, false);
    }
}