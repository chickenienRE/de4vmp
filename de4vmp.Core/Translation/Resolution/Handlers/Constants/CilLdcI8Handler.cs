using AsmResolver.DotNet.Code.Cil;
using AsmResolver.IO;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.Constants;

public class CilLdcI8Handler : ConstantHandlerBase {
    public override VmpCode Translates => VmpCode.CilLdcI8Code;

    public override bool Resolve(CilInstructionCollection instructions) {
        return IsConstantSignature(instructions, typeof(long));
    }

    public override VmpTranslatorState Translate(VmpTranslator translator, ref BinaryStreamReader reader,
        VmpInstruction instruction, VmpTranslatorContext context) {
        instruction.Operand = reader.ReadInt64();
        return VmpTranslatorState.Next;
    }
}