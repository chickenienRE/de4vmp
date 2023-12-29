using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.PE.DotNet.Cil;

namespace de4vmp.Core.Translation.Resolution.Handlers;

public abstract class BitwiseHandlerBase : HandlerBase {
    private readonly IEnumerable<CilCode> _bitwiseSignature = new List<CilCode> {
        CilCode.Ldarg,
        CilCode.Call,
        CilCode.Stloc,
        CilCode.Ldarg,
        CilCode.Call,
        CilCode.Stloc,
        CilCode.Ldarg,
        CilCode.Ldarg,
        CilCode.Ldloc,
        CilCode.Ldloc,
        CilCode.Call,
        CilCode.Call,
        CilCode.Ret
    };

    private readonly IEnumerable<CilCode> _simpleBitwiseSignature = new List<CilCode> {
        CilCode.Ldarg,
        CilCode.Call,
        CilCode.Stloc,
        CilCode.Ldarg,
        CilCode.Ldarg,
        CilCode.Ldloc,
        CilCode.Call,
        CilCode.Call,
        CilCode.Ret
    };

    protected bool IsSignature(CilInstructionCollection instructions, CilCode code) {
        return instructions.AreSignatureEqual(_bitwiseSignature) &&
               instructions.GetOperandAs<MethodDefinition>(10).HasCode(code);
    }

    protected bool IsSimpleSignature(CilInstructionCollection instructions, CilCode code) {
        return instructions.AreSignatureEqual(_simpleBitwiseSignature) &&
               instructions.GetOperandAs<MethodDefinition>(6).HasCode(code);
    }
}