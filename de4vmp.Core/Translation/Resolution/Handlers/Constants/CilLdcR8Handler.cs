using AsmResolver.DotNet.Code.Cil;
using AsmResolver.IO;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.Constants;

public class CilLdcR8Handler : ConstantHandlerBase {
    public override VmpCode Translates => VmpCode.CilLdcR8Code;

    public override bool Resolve(CilInstructionCollection instructions) {
        return IsConstantSignature(instructions, typeof(double));
    }

    public override VmpTranslatorState Translate(VmpTranslator translator, ref BinaryStreamReader reader,
        VmpInstruction instruction, VmpTranslatorContext context) {
        instruction.Operand = reader.ReadDouble();
        return VmpTranslatorState.Next;
    }
}