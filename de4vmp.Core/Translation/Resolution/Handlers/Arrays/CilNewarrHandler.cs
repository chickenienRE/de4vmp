using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.IO;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.Arrays;

public class CilNewarrHandler : HandlerBase {
    public override VmpCode Translates => VmpCode.CilNewarrCode;

    public override bool Resolve(CilInstructionCollection instructions) {
        return instructions.HasMethodReference(nameof(Array.CreateInstance)) &&
               !instructions.HasMethodReference(nameof(Type.GetGenericTypeDefinition));
    }

    public override VmpTranslatorState Translate(VmpTranslator translator, ref BinaryStreamReader reader,
        VmpInstruction instruction, VmpTranslatorContext context) {
        instruction.Operand = translator.ResolveMember<ITypeDescriptor>(context.DataStack.Pop());
        return VmpTranslatorState.Next;
    }
}