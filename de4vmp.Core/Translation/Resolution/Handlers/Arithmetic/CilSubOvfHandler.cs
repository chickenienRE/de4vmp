using AsmResolver.DotNet.Code.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.Arithmetic;

public class CilSubOvfHandler : ArithmeticHandlerBase {
    public override VmpCode Translates => VmpCode.CilSubOvfCode;

    public override bool Resolve(CilInstructionCollection instructions) {
        return IsSubSignature(instructions, true, false);
    }
}