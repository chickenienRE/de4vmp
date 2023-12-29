using AsmResolver.DotNet.Code.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.Comparision; 

public class VmilCmpUnHandler : ComparisionHandlerBase {
    public override VmpCode Translates => VmpCode.VmilCmpUnsignedCode;
    
    public override bool Resolve(CilInstructionCollection instructions) {
        return IsComparisionSignature(instructions);
    }

    protected override bool UnSigned => true;
}