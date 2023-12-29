using AsmResolver.DotNet.Code.Cil;
using AsmResolver.PE.DotNet.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.Bitwise;

public class CilOrHandler : BitwiseHandlerBase {
    public override VmpCode Translates => VmpCode.CilOrCode;

    public override bool Resolve(CilInstructionCollection instructions) {
        return IsSignature(instructions, CilCode.Or);
    }
}