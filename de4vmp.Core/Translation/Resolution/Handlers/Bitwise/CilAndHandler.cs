using AsmResolver.DotNet.Code.Cil;
using AsmResolver.IO;
using AsmResolver.PE.DotNet.Cil;
using de4vmp.Core.Architecture;
using de4vmp.Core.Services;

namespace de4vmp.Core.Translation.Resolution.Handlers.Bitwise;

public class CilAndHandler : BitwiseHandlerBase {
    public override VmpCode Translates => VmpCode.CilAndCode;

    public override bool Resolve(CilInstructionCollection instructions) {
        return IsSignature(instructions, CilCode.And) && !IsSignature(instructions, CilCode.Shl);
    }

    public override VmpTranslatorState Translate(VmpTranslator translator, ref BinaryStreamReader reader,
        VmpInstruction instruction,
        VmpTranslatorContext context) {
        if (context.State is VmpState.ComparisionState) {
            context.DataStack.TryPop(out _);

            if (!context.CmpStack.TryPop(out var value))
                throw ExceptionService.ComparerNullException();

            if (value.OpCode.FlowControl is CilFlowControl.ConditionalBranch)
                context.CmpStack.Push(value);
            else
                instruction.Annotation = value;

            context.State = VmpState.NormalState;
        }

        return VmpTranslatorState.Next;
    }
}