using AsmResolver.IO;
using de4vmp.Core.Architecture;
using de4vmp.Core.Architecture.Annotations.Variables;

namespace de4vmp.Core.Translation.Resolution.Handlers; 

public abstract class VariableHandlerBase : HandlerBase {
    public override VmpTranslatorState Translate(VmpTranslator translator, ref BinaryStreamReader reader, VmpInstruction instruction,
        VmpTranslatorContext context) {
        short index = reader.ReadInt16();
        var variable = translator.Function.Variables[index];
        instruction.Annotation = new VariableAnnotation(index, variable);
        return VmpTranslatorState.Next;
    }
}