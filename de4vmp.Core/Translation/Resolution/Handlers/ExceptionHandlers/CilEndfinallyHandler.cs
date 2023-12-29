using System.Collections;
using AsmResolver.DotNet.Code.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.ExceptionHandlers;

public class CilEndfinallyHandler : HandlerBase {
    public override VmpCode Translates => VmpCode.CilEndfinallyCode;

    public override bool Resolve(CilInstructionCollection instructions) {
        return instructions.HasMethodReference(nameof(Stack.Pop)) &&
               !instructions.HasMethodReference(nameof(Stack.Push)) &&
               !instructions.HasMethodReference(nameof(Stack.Clear));
    }
}