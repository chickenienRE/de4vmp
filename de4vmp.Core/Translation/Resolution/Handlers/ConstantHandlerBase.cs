using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.PE.DotNet.Cil;

namespace de4vmp.Core.Translation.Resolution.Handlers;

public abstract class ConstantHandlerBase : HandlerBase {
    private readonly IEnumerable<CilCode> _signature = new List<CilCode> {
        CilCode.Ldarg,
        CilCode.Ldarg,
        CilCode.Call,
        CilCode.Newobj,
        CilCode.Call,
        CilCode.Ret
    };

    protected bool IsConstantSignature(CilInstructionCollection instructions, Type type) {
        return instructions.AreSignatureEqual(_signature) &&
               instructions.GetOperandAs<MethodDefinition>(2).HasReturnTypeOf(type);
    }
}