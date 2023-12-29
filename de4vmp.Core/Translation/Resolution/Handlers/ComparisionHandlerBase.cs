using AsmResolver.DotNet.Code.Cil;
using AsmResolver.IO;
using AsmResolver.PE.DotNet.Cil;
using de4vmp.Core.Architecture;
using de4vmp.Core.Architecture.Annotations.Comparision;

namespace de4vmp.Core.Translation.Resolution.Handlers;

public abstract class ComparisionHandlerBase : HandlerBase {
    private readonly IEnumerable<CilCode> _signature = new List<CilCode> {
        CilCode.Ldarg,
        CilCode.Call,
        CilCode.Callvirt,
        CilCode.Stloc,
        CilCode.Ldarg,
        CilCode.Call,
        CilCode.Stloc,
        CilCode.Ldarg,
        CilCode.Call,
        CilCode.Stloc,
        CilCode.Ldarg,
        CilCode.Ldarg,
        CilCode.Ldloc,
        CilCode.Ldloc,
        CilCode.Ldc_I4,
        CilCode.Ldloc,
        CilCode.Call,
        CilCode.Newobj,
        CilCode.Call,
        CilCode.Ret
    };

    protected abstract bool UnSigned { get; }

    public override VmpTranslatorState Translate(VmpTranslator translator, ref BinaryStreamReader reader,
        VmpInstruction instruction, VmpTranslatorContext context) {
        if (context.CmpStack.Count != 0)
            throw new VmpTranslatorException("Comparer stack is not empty.");

        if (context.State is not VmpState.NormalState)
            throw new VmpTranslatorException("Invalid state in comparer translation.");

        context.CmpStack.Push(new VmpCmp(context.DataStack.Pop(), UnSigned));
        context.State = VmpState.ComparisionState;
        return VmpTranslatorState.Next;
    }

    protected bool IsComparisionSignature(CilInstructionCollection instructions) {
        return instructions.AreSignatureEqual(_signature) && instructions[14].GetLdcI4Constant() == (UnSigned ? 1 : 0);
    }
}