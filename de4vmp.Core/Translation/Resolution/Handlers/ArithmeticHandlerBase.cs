using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.PE.DotNet.Cil;

namespace de4vmp.Core.Translation.Resolution.Handlers;

public abstract class ArithmeticHandlerBase : HandlerBase {
    private readonly IEnumerable<CilCode> _arithmeticSignature = new List<CilCode> {
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
        CilCode.Ldc_I4,
        CilCode.Ldc_I4,
        CilCode.Call,
        CilCode.Call,
        CilCode.Ret
    };

    private readonly IEnumerable<CilCode> _simpleArithmeticSignature = new List<CilCode> {
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
        CilCode.Ldc_I4,
        CilCode.Call,
        CilCode.Call,
        CilCode.Ret
    };

    private bool IsSimpleSignature(CilInstructionCollection instructions, CilCode code, bool unsigned) {
        return instructions.AreSignatureEqual(_simpleArithmeticSignature) &&
               instructions[10].GetLdcI4Constant() == (unsigned ? 1 : 0) &&
               instructions.GetOperandAs<MethodDefinition>(11).HasCode(code);
    }

    private bool IsSignature(CilInstructionCollection instructions, CilCode code, bool overflow, bool unsigned) {
        return instructions.AreSignatureEqual(_arithmeticSignature) &&
               instructions[10].GetLdcI4Constant() == (overflow ? 1 : 0) &&
               instructions[11].GetLdcI4Constant() == (unsigned ? 1 : 0) &&
               instructions.GetOperandAs<MethodDefinition>(12).GetCodeCount(code) > 6;
    }

    protected bool IsAddSignature(CilInstructionCollection instructions, bool overflow, bool unsigned) {
        return IsSignature(instructions, CilCode.Add, overflow, unsigned);
    }

    protected bool IsSubSignature(CilInstructionCollection instructions, bool overflow, bool unsigned) {
        return IsSignature(instructions, CilCode.Sub, overflow, unsigned);
    }

    protected bool IsMulSignature(CilInstructionCollection instructions, bool overflow, bool unsigned) {
        return IsSignature(instructions, CilCode.Mul, overflow, unsigned);
    }

    protected bool IsDivSignature(CilInstructionCollection instructions, bool unsigned) {
        return IsSimpleSignature(instructions, CilCode.Div, unsigned);
    }

    protected bool IsRemSignature(CilInstructionCollection instructions, bool unsigned) {
        return IsSimpleSignature(instructions, CilCode.Rem, unsigned);
    }

    protected bool IsShrSignature(CilInstructionCollection instructions, bool unsigned) {
        return IsSimpleSignature(instructions, CilCode.Shr, unsigned);
    }
}