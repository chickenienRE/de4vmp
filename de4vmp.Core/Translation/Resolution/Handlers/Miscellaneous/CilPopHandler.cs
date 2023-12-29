using AsmResolver.DotNet.Code.Cil;
using AsmResolver.IO;
using AsmResolver.PE.DotNet.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.Miscellaneous;

public class CilPopHandler : HandlerBase {
    private readonly IEnumerable<CilCode> _signature = new List<CilCode> {
        CilCode.Ldarg,
        CilCode.Call,
        CilCode.Pop,
        CilCode.Ret
    };

    public override VmpCode Translates => VmpCode.CilPopCode;

    public override bool Resolve(CilInstructionCollection instructions) {
        return instructions.AreSignatureEqual(_signature);
    }

    public override VmpTranslatorState Translate(VmpTranslator translator, ref BinaryStreamReader reader,
        VmpInstruction instruction,
        VmpTranslatorContext context) {
        if (translator.Function.Variables.Count > 0)
            if (context.ValueStack.Count > 0)
                context.ValueStack.Clear();

        return VmpTranslatorState.Next;
    }
}