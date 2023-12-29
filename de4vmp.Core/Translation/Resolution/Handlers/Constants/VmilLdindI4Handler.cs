using System.Runtime.InteropServices;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.IO;
using de4vmp.Core.Architecture;
using de4vmp.Core.Architecture.Annotations;

namespace de4vmp.Core.Translation.Resolution.Handlers.Constants;

public class VmilLdindI4Handler : HandlerBase {
    public override VmpCode Translates => VmpCode.VmilLdindI4Code;

    public override bool Resolve(CilInstructionCollection instructions) {
        return instructions.HasMethodReference(nameof(Marshal.ReadInt32));
    }

    public override VmpTranslatorState Translate(VmpTranslator translator, ref BinaryStreamReader reader,
        VmpInstruction instruction,
        VmpTranslatorContext context) {
        if (context.DataStack.TryPeek(out int table) && translator.CanCreateReader((uint)table)) {
            context.State = VmpState.MultipleJumpState;
            instruction.Annotation = new NopAnnotation();
        }

        return VmpTranslatorState.Next;
    }
}