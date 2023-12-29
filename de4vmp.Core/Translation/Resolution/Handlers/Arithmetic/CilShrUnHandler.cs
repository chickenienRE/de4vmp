using AsmResolver.DotNet.Code.Cil;
using AsmResolver.IO;
using de4vmp.Core.Architecture;
using de4vmp.Core.Architecture.Annotations.Comparision;
using de4vmp.Core.Services;

namespace de4vmp.Core.Translation.Resolution.Handlers.Arithmetic;

public class CilShrUnHandler : ArithmeticHandlerBase {
    public override VmpCode Translates => VmpCode.CilShrUnsignedCode;

    public override bool Resolve(CilInstructionCollection instructions) {
        return IsShrSignature(instructions, true);
    }

    public override VmpTranslatorState Translate(VmpTranslator translator, ref BinaryStreamReader reader,
        VmpInstruction instruction,
        VmpTranslatorContext context) {
        if (context.State is VmpState.ComparisionState) {
            if (!context.CmpStack.TryPeek(out var value))
                throw ExceptionService.ComparerNullException();

            value.Flags |= VmpCmpFlags.ShrUnFlag;
            context.DataStack.TryPop(out _);
        }

        return VmpTranslatorState.Next;
    }
}