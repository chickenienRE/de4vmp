using AsmResolver.DotNet.Code.Cil;
using AsmResolver.IO;
using AsmResolver.PE.DotNet.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.Bitwise;

public class CilNotHandler : BitwiseHandlerBase {
    public override VmpCode Translates => VmpCode.CilNotCode;

    public override bool Resolve(CilInstructionCollection instructions) {
        return IsSimpleSignature(instructions, CilCode.Not);
    }

    public override VmpTranslatorState Translate(VmpTranslator translator, ref BinaryStreamReader reader,
        VmpInstruction instruction,
        VmpTranslatorContext context) {
        if (context.State is VmpState.DoubleJumpState) context.FlagsStack.Push(VmpFlags.NotFlag);

        return VmpTranslatorState.Next;
    }
}