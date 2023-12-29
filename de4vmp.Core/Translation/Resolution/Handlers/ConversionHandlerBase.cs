using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.IO;
using AsmResolver.PE.DotNet.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers;

public abstract class ConversionHandlerBase : HandlerBase {
    private readonly IEnumerable<CilCode> _signature = new List<CilCode> {
        CilCode.Ldarg,
        CilCode.Ldarg,
        CilCode.Call,
        CilCode.Callvirt,
        CilCode.Call,
        CilCode.Stloc,
        CilCode.Ldarg,
        CilCode.Ldarg,
        CilCode.Ldarg,
        CilCode.Call,
        CilCode.Ldloc,
        CilCode.Ldc_I4,
        CilCode.Callvirt,
        CilCode.Ldloc,
        CilCode.Call,
        CilCode.Call,
        CilCode.Ret
    };

    protected abstract bool UnSigned { get; }

    public override VmpTranslatorState Translate(VmpTranslator translator, ref BinaryStreamReader reader,
        VmpInstruction instruction, VmpTranslatorContext context) {
        instruction.Operand = translator.ResolveMember<ITypeDescriptor>(context.DataStack.Pop());
        return VmpTranslatorState.Next;
    }

    protected bool IsConversionSignature(CilInstructionCollection instructions) {
        return instructions.AreSignatureEqual(_signature) && instructions[11].GetLdcI4Constant() == (UnSigned ? 1 : 0);
    }
}