using AsmResolver.DotNet.Code.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.Arithmetic;

public class CilAddOvfUnHandler : ArithmeticHandlerBase {
    public override VmpCode Translates => VmpCode.CilAddOvfUnCode;

    public override bool Resolve(CilInstructionCollection instructions) {
        return IsAddSignature(instructions, true, true);
    }
}