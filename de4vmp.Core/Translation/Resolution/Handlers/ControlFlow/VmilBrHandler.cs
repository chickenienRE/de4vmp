using AsmResolver.DotNet.Code.Cil;
using AsmResolver.IO;
using AsmResolver.PE.DotNet.Cil;
using de4vmp.Core.Architecture;
using de4vmp.Core.Architecture.Annotations.ControlFlow;

namespace de4vmp.Core.Translation.Resolution.Handlers.ControlFlow;

public class VmilBrHandler : HandlerBase {
    private readonly IEnumerable<CilCode> _signature = new List<CilCode> {
        CilCode.Ldarg,
        CilCode.Ldarg,
        CilCode.Call,
        CilCode.Callvirt,
        CilCode.Stfld,
        CilCode.Ret
    };

    public override VmpCode Translates => VmpCode.VmilBrCode;

    public override bool Resolve(CilInstructionCollection instructions) {
        return instructions.AreSignatureEqual(_signature);
    }

    public override VmpTranslatorState Translate(VmpTranslator translator, ref BinaryStreamReader reader,
        VmpInstruction instruction, VmpTranslatorContext context) {
        switch (context.State) {
            case VmpState.NormalState:
                TranslateSingleJump(translator, instruction, context);
                break;
            case VmpState.DoubleJumpState:
                TranslateDoubleJump(translator, instruction, context);
                break;
            case VmpState.MultipleJumpState:
                TranslateMultipleJump(translator, instruction, context);
                break;
            default:
                throw new InvalidOperationException($"invalid state_{context.State}");
        }

        context.State = VmpState.NormalState;
        return VmpTranslatorState.Break;
    }

    private static void TranslateSingleJump(VmpTranslator translator, VmpInstruction instruction,
        VmpTranslatorContext translatorContext) {
        var annotation = new SingleJumpAnnotation((uint)translatorContext.DataStack.Pop());
        instruction.Annotation = annotation;

        if (annotation.WillReturn)
            return;

        translator.AddState(annotation.Address);
    }

    private static void TranslateDoubleJump(VmpTranslator translator, VmpInstruction instruction,
        VmpTranslatorContext translatorContext) {
        var annotation = new DoubleJumpAnnotation((uint)translatorContext.DataStack.Pop(), (uint)translatorContext.DataStack.Pop());

        while (translatorContext.FlagsStack.TryPop(out var flag))
            if (flag is VmpFlags.NotFlag)
                annotation.Type = !annotation.Type;

        if (translatorContext.CmpStack.TryPop(out var value))
            annotation.Comparision = value;

        translator.AddState(annotation.SecondAddress);
        translator.AddState(annotation.FirstAddress);
        instruction.Annotation = annotation;
    }

    private static void TranslateMultipleJump(VmpTranslator translator, VmpInstruction instruction,
        VmpTranslatorContext translatorContext) {
        uint baseAddress = (uint)translatorContext.DataStack.Pop();
        translatorContext.DataStack.Pop();
        int count = translatorContext.DataStack.Pop();

        var addresses = new List<uint>();
        for (int i = 0; i < count; i++) {
            var reader = translator.CreateReader(baseAddress);
            uint address = (uint)reader.ReadInt32();

            if (translator.CanCreateReader(address)) addresses.Add(address);

            baseAddress += sizeof(int);
        }

        foreach (uint address in addresses) translator.AddState(address);

        instruction.Annotation = new MultipleJumpAnnotation(addresses);
    }
}