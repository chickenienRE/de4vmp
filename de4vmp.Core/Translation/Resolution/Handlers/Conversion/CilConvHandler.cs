using AsmResolver.DotNet.Code.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.Conversion;

public class CilConvHandler : ConversionHandlerBase {
    public override VmpCode Translates => VmpCode.CilConvSignedCode;

    protected override bool UnSigned => false;

    public override bool Resolve(CilInstructionCollection instructions) {
        return IsConversionSignature(instructions);
    }
}