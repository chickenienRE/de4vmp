using AsmResolver.DotNet.Code.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.Arithmetic;

public class CilSubHandler : ArithmeticHandlerBase {
    public override VmpCode Translates => VmpCode.CilSubCode;

    public override bool Resolve(CilInstructionCollection instructions) {
        return IsSubSignature(instructions, false, false);
    }
}