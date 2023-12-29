using AsmResolver.DotNet;
using AsmResolver.PE.DotNet.Cil;
using de4vmp.Core.Architecture;
using de4vmp.Core.Services;
using de4vmp.Core.Translation.Transformation.Converters;

namespace de4vmp.Core.Translation.Transformation.Transforms;

public class BoxHandlerTransform : ITransform {
    public IEnumerable<VmpCode> Accepts {
        get { yield return VmpCode.CilBoxCode; }
    }

    public void Transform(VmpRecompiler recompiler, VmpInstruction instruction) {
        if (instruction.Operand is not ITypeDescriptor typeDescriptor)
            throw ExceptionService.ThrowInvalidOperand<VmpInstruction, ITypeDescriptor>(instruction);

        recompiler.AddInstruction(new CilInstruction(CilOpCodes.Pop));
        
        if (recompiler.ReturnType.IsFullnameType(typeof(bool))) {
            recompiler.AddInstruction(instruction.Address, new CilInstruction(CilOpCodes.Nop));
            return;
        }

        recompiler.AddInstruction(instruction.Address,
            recompiler.TryLookupByName<ConvConvert>(typeDescriptor.Name, out var convOpCode)
                ? new CilInstruction(convOpCode)
                : new CilInstruction(CilOpCodes.Box, typeDescriptor));
    }
}