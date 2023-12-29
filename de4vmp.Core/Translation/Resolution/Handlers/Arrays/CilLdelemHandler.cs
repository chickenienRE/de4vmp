using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.IO;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.Arrays;

public class CilLdelemHandler : HandlerBase {
    public override VmpCode Translates => VmpCode.CilLdelemCode;

    public override bool Resolve(CilInstructionCollection instructions) {
        return instructions.HasTypeReference(nameof(Array)) &&
               instructions.HasMethodReference(nameof(Array.GetValue));
    }

    public override VmpTranslatorState Translate(VmpTranslator translator, ref BinaryStreamReader reader,
        VmpInstruction instruction, VmpTranslatorContext context) {
        instruction.Operand = translator.ResolveMember<ITypeDescriptor>(context.DataStack.Pop());
        return VmpTranslatorState.Next;
    }
}