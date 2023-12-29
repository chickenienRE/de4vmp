using AsmResolver.DotNet;
using AsmResolver.PE.DotNet.Cil;
using de4vmp.Core.Architecture;
using de4vmp.Core.Services;
using de4vmp.Core.Translation.Transformation.Converters;

namespace de4vmp.Core.Translation.Transformation.Transforms;

public class ConversionHandlerTransform : ITransform {
    public IEnumerable<VmpCode> Accepts {
        get {
            yield return VmpCode.VmilConvCode;
            yield return VmpCode.CilConvSignedCode;
            yield return VmpCode.CilConvUnsignedCode;
        }
    }

    public void Transform(VmpRecompiler recompiler, VmpInstruction instruction) {
        if (instruction.Operand is not ITypeDescriptor typeDescriptor)
            throw ExceptionService.ThrowInvalidOperand<VmpInstruction, ITypeDescriptor>(instruction);

        if (typeDescriptor.FullName.Equals(typeof(bool).FullName)) {
            recompiler.AddInstruction(instruction.Address, new CilInstruction(CilOpCodes.Nop));
            return;
        }

        recompiler.AddInstruction(new CilInstruction(CilOpCodes.Pop));
        recompiler.AddInstruction(instruction.Address,
            recompiler.TryLookupByName<ConvConvert>(typeDescriptor.Name, out var convOpCode)
                ? new CilInstruction(convOpCode)
                : new CilInstruction(CilOpCodes.Box, typeDescriptor));
    }
}