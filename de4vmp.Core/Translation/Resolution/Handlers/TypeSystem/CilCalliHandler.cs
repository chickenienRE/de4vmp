using System.Reflection;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.IO;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.TypeSystem;

public class CilCalliHandler : TypeSystemHandlerBase {
    public override VmpCode Translates => VmpCode.CilCalliCode;

    public override bool Resolve(CilInstructionCollection instructions) {
        return instructions.HasTypeReference(nameof(MethodBase));
    }

    public override VmpTranslatorState Translate(VmpTranslator translator, ref BinaryStreamReader reader,
        VmpInstruction instruction, VmpTranslatorContext context) {
        throw new NotSupportedException(nameof(CilCalliHandler));
    }
}