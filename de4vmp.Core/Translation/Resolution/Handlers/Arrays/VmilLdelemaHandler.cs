using AsmResolver.DotNet.Code.Cil;
using AsmResolver.IO;
using AsmResolver.PE.DotNet.Cil;
using de4vmp.Core.Architecture;
using de4vmp.Core.Architecture.Annotations;
using de4vmp.Core.Translation.Emulation;
using de4vmp.Core.Translation.Emulation.Values;

namespace de4vmp.Core.Translation.Resolution.Handlers.Arrays;

public class VmilLdelemaHandler : HandlerBase {
    private readonly IEnumerable<CilCode> _signature = new List<CilCode> {
        CilCode.Ldarg,
        CilCode.Call,
        CilCode.Stloc,
        CilCode.Ldarg,
        CilCode.Call,
        CilCode.Callvirt,
        CilCode.Isinst,
        CilCode.Stloc,
        CilCode.Ldloc,
        CilCode.Brtrue,
        CilCode.Newobj,
        CilCode.Throw,
        CilCode.Ldarg,
        CilCode.Ldloc,
        CilCode.Ldloc,
        CilCode.Callvirt,
        CilCode.Newobj,
        CilCode.Call,
        CilCode.Ret
    };

    public override VmpCode Translates => VmpCode.VmilLdelemaCode;

    public override bool Resolve(CilInstructionCollection instructions) {
        return instructions.AreSignatureEqual(_signature);
    }

    public override VmpTranslatorState Translate(VmpTranslator translator, ref BinaryStreamReader reader,
        VmpInstruction instruction,
        VmpTranslatorContext context) {
        if (context.ValueStack.TryPeek<ParameterArrayValue, IValue>(out var value)) {
            if (!context.DataStack.TryPop(out int index))
                throw new VmpTranslatorException("Integer stack does not contain variable index.");

            context.ValueStack.Push(new ParameterValue(value.Parameters[index]));
            instruction.Annotation = new NopAnnotation();
        }

        return VmpTranslatorState.Next;
    }
}