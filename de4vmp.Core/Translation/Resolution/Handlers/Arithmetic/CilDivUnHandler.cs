using AsmResolver.DotNet.Code.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.Arithmetic;

public class CilDivUnHandler : ArithmeticHandlerBase {
    public override VmpCode Translates => VmpCode.CilDivUnsignedCode;

    public override bool Resolve(CilInstructionCollection instructions) {
        return IsDivSignature(instructions, true);
    }
}