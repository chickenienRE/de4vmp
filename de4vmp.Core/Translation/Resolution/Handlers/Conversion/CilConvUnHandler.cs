using AsmResolver.DotNet.Code.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.Conversion;

public class CilConvUnHandler : ConversionHandlerBase {
    public override VmpCode Translates => VmpCode.CilConvUnsignedCode;

    protected override bool UnSigned => true;

    public override bool Resolve(CilInstructionCollection instructions) {
        return IsConversionSignature(instructions);
    }
}