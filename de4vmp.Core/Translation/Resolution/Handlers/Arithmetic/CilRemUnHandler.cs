using AsmResolver.DotNet.Code.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.Arithmetic; 

public class CilRemUnHandler : ArithmeticHandlerBase {
    public override VmpCode Translates => VmpCode.CilRemUnsignedCode;
    
    public override bool Resolve(CilInstructionCollection instructions) {
        return IsRemSignature(instructions, true);
    }
}