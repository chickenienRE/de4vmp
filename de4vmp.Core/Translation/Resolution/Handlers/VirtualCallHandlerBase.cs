using AsmResolver.DotNet.Code.Cil;
using AsmResolver.IO;
using AsmResolver.PE.DotNet.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers;

public abstract class VirtualCallHandlerBase : HandlerBase {
    private readonly IEnumerable<CilCode> _signature = new List<CilCode> {
        CilCode.Ldarg,
        CilCode.Ldarg,
        CilCode.Call,
        CilCode.Callvirt,
        CilCode.Ldc_I4,
        CilCode.Call,
        CilCode.Stloc,
        CilCode.Ldloc,
        CilCode.Brfalse,
        CilCode.Ldarg,
        CilCode.Ldloc,
        CilCode.Call,
        CilCode.Ret
    };

    public abstract bool IsStatic { get; }

    public override bool Resolve(CilInstructionCollection instructions) {
        return IsTypeSystemSignature(instructions, IsStatic);
    }

    public override VmpTranslatorState Translate(VmpTranslator translator, ref BinaryStreamReader reader,
        VmpInstruction instruction, VmpTranslatorContext context) {
        var function = translator.GetOrCreateFunction(IsStatic, (uint)context.DataStack.Pop());
        instruction.Operand = function.Parent;
        return VmpTranslatorState.Next;
    }

    private bool IsTypeSystemSignature(CilInstructionCollection instructions, bool isStatic) {
        return instructions.AreSignatureEqual(_signature) && instructions[4].GetLdcI4Constant() == (isStatic ? 0 : 1);
    }
}