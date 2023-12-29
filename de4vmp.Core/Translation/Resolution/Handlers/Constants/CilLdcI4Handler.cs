using AsmResolver.DotNet.Code.Cil;
using AsmResolver.IO;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.Constants;

public class CilLdcI4Handler : ConstantHandlerBase {
    public override VmpCode Translates => VmpCode.CilLdcI4Code;

    public override bool Resolve(CilInstructionCollection instructions) {
        return IsConstantSignature(instructions, typeof(int));
    }

    public override VmpTranslatorState Translate(VmpTranslator translator, ref BinaryStreamReader reader,
        VmpInstruction instruction, VmpTranslatorContext context) {
        int value = reader.ReadInt32();
        context.DataStack.Push(value);
        instruction.Operand = value;
        return VmpTranslatorState.Next;
    }
}