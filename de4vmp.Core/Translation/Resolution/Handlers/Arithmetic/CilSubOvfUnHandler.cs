using AsmResolver.DotNet.Code.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.Arithmetic;

public class CilSubOvfUnHandler : ArithmeticHandlerBase {
    public override VmpCode Translates => VmpCode.CilSubOvfUnCode;

    public override bool Resolve(CilInstructionCollection instructions) {
        return IsSubSignature(instructions, true, true);
    }
}