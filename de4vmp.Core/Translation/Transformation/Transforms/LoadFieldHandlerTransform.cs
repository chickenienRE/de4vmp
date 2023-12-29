using AsmResolver.DotNet;
using AsmResolver.PE.DotNet.Cil;
using de4vmp.Core.Architecture;
using de4vmp.Core.Services;

namespace de4vmp.Core.Translation.Transformation.Transforms;

public class LoadFieldHandlerTransform : ITransform {
    private readonly IDictionary<VmpCode, CilOpCode> _instanceMapping = new Dictionary<VmpCode, CilOpCode> {
        [VmpCode.VmilLoadFieldCode] = CilOpCodes.Ldfld,
        [VmpCode.VmilLoadFieldACode] = CilOpCodes.Ldflda
    };

    private readonly IDictionary<VmpCode, CilOpCode> _staticMapping = new Dictionary<VmpCode, CilOpCode> {
        [VmpCode.VmilLoadFieldCode] = CilOpCodes.Ldsfld,
        [VmpCode.VmilLoadFieldACode] = CilOpCodes.Ldsflda
    };

    public IEnumerable<VmpCode> Accepts {
        get {
            yield return VmpCode.VmilLoadFieldCode;
            yield return VmpCode.VmilLoadFieldACode;
        }
    }

    public void Transform(VmpRecompiler recompiler, VmpInstruction instruction) {
        recompiler.AddInstruction(new CilInstruction(CilOpCodes.Pop));

        if (instruction.Operand is not IFieldDescriptor fieldDescriptor)
            throw ExceptionService.ThrowInvalidOperand<VmpInstruction, IFieldDescriptor>(instruction);

        if (fieldDescriptor.Resolve() is not { } fieldDefinition)
            throw new VmpRecompilerException($"Unable to resolve fieldDefinition from {fieldDescriptor}");

        if (fieldDefinition.IsStatic) {
            recompiler.AddInstruction(new CilInstruction(CilOpCodes.Pop));
            recompiler.AddInstruction(new CilInstruction(_staticMapping[instruction.Code], fieldDescriptor));
        }
        else {
            recompiler.AddInstruction(new CilInstruction(_instanceMapping[instruction.Code], fieldDescriptor));
        }
    }
}