using AsmResolver.DotNet;
using AsmResolver.IO;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers;

public abstract class TypeSystemHandlerBase : HandlerBase {
    public override VmpTranslatorState Translate(VmpTranslator translator, ref BinaryStreamReader reader,
        VmpInstruction instruction, VmpTranslatorContext context) {
        instruction.Operand = translator.ResolveMember<IMetadataMember>(context.DataStack.Pop());
        return VmpTranslatorState.Next;
    }
}