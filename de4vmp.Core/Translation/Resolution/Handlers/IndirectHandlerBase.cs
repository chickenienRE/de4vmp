using System.Reflection;
using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.IO;
using AsmResolver.PE.DotNet.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers;

public abstract class IndirectHandlerBase : HandlerBase {
    private readonly IEnumerable<CilCode> _pushSignature = new List<CilCode> {
        CilCode.Ldarg,
        CilCode.Ldfld,
        CilCode.Ldarg,
        CilCode.Callvirt,
        CilCode.Callvirt,
        CilCode.Ret
    };

    protected abstract bool HasPush { get; }

    public override bool Resolve(CilInstructionCollection instructions) {
        return instructions.HasMethodReference(nameof(Pointer.Unbox)) && IsIndirectSignature(instructions);
    }

    public override VmpTranslatorState Translate(VmpTranslator translator, ref BinaryStreamReader reader,
        VmpInstruction instruction, VmpTranslatorContext context) {
        instruction.Operand = translator.ResolveMember<IMetadataMember>(context.DataStack.Pop());
        return VmpTranslatorState.Next;
    }

    private bool IsIndirectSignature(CilInstructionCollection instructions) {
        return instructions.GetAllTFromCollection<IMethodDescriptor>(CilOperandType.InlineMethod)
            .OfType<MethodDefinition>()
            .Any(methodDefinition => methodDefinition.IsSignatureEqual(_pushSignature)) == HasPush;
    }
}