using System.Collections;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.IO;
using de4vmp.Core.Architecture;
using de4vmp.Core.Architecture.Annotations.ControlFlow;

namespace de4vmp.Core.Translation.Resolution.Handlers.ExceptionHandlers;

public class CilLeaveHandler : HandlerBase {
    public override VmpCode Translates => VmpCode.CilLeaveCode;

    public override bool Resolve(CilInstructionCollection instructions) {
        return instructions.HasMethodReference(nameof(Stack.Peek)) &&
               instructions.HasMethodReference(nameof(Stack.Clear));
    }

    public override VmpTranslatorState Translate(VmpTranslator translator, ref BinaryStreamReader reader,
        VmpInstruction instruction,
        VmpTranslatorContext context) {
        uint continueAddress = (uint) context.DataStack.Pop();
        translator.AddState(continueAddress);
        instruction.Annotation = new SingleJumpAnnotation(continueAddress);
        return VmpTranslatorState.Break;
    }
}