using AsmResolver.DotNet.Code.Cil;
using AsmResolver.IO;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution; 

public abstract class HandlerBase {
    public abstract VmpCode Translates { get; }
    public abstract bool Resolve(CilInstructionCollection instructions);
    public virtual VmpTranslatorState Translate(VmpTranslator translator, ref BinaryStreamReader reader,
        VmpInstruction instruction, VmpTranslatorContext context) {
        return VmpTranslatorState.Next;
    }
}