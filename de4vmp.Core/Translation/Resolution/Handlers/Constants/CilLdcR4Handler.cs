using AsmResolver.DotNet.Code.Cil;
using AsmResolver.IO;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.Constants; 

public class CilLdcR4Handler : ConstantHandlerBase {
    public override VmpCode Translates => VmpCode.CilLdcR4Code;
    
    public override bool Resolve(CilInstructionCollection instructions) {
        return IsConstantSignature(instructions, typeof(float));
    }

    public override VmpTranslatorState Translate(VmpTranslator translator, ref BinaryStreamReader reader,
        VmpInstruction instruction, VmpTranslatorContext context) {
        instruction.Operand = reader.ReadSingle();
        return VmpTranslatorState.Next;
    }
}