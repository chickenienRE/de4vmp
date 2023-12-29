using System.Collections;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.IO;
using de4vmp.Core.Architecture;
using de4vmp.Core.Architecture.ExceptionHandlers.Variants;

namespace de4vmp.Core.Translation.Resolution.Handlers.ExceptionHandlers;

public class VmilPushEhHandler : HandlerBase {
    public override VmpCode Translates => VmpCode.VmilPushEhCode;

    public override bool Resolve(CilInstructionCollection instructions) {
        return instructions.HasMethodReference(nameof(Stack.Push)) &&
               !instructions.HasMethodReference(nameof(Stack.Pop));
    }

    public override VmpTranslatorState Translate(VmpTranslator translator, ref BinaryStreamReader reader,
        VmpInstruction instruction,
        VmpTranslatorContext context) {
        int scopeStart = context.DataStack.Pop();
        var handlers = context.Scopes
            .Where(handlerScope => handlerScope.ScopeStart == scopeStart)
            .SelectMany(scope => scope.Handlers)
            .Reverse();

        foreach (var handler in handlers) {
            handler.TryStart = instruction.Address;
            translator.Function.Handlers.Add(handler);

            translator.AddState(handler.HandlerStart);
            if (handler is VmpFilterExceptionHandler filter) 
                translator.AddState(filter.FilterStart);
        }

        return VmpTranslatorState.Next;
    }
}