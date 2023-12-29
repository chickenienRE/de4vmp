using AsmResolver.DotNet.Code.Cil;
using AsmResolver.IO;
using de4vmp.Core.Architecture;
using de4vmp.Core.Architecture.Annotations.Comparision;
using de4vmp.Core.Services;

namespace de4vmp.Core.Translation.Resolution.Handlers.Arithmetic;

public class CilAddHandler : ArithmeticHandlerBase {
    public override VmpCode Translates => VmpCode.CilAddCode;

    public override bool Resolve(CilInstructionCollection instructions) {
        return IsAddSignature(instructions, false, false);
    }

    public override VmpTranslatorState Translate(VmpTranslator translator, ref BinaryStreamReader reader,
        VmpInstruction instruction,
        VmpTranslatorContext context) {
        if (context.State is VmpState.ComparisionState) {
            if (!context.CmpStack.TryPeek(out var value))
                throw ExceptionService.ComparerNullException();

            value.Flags |= VmpCmpFlags.AddFlag;
        }

        return VmpTranslatorState.Next;
    }
}