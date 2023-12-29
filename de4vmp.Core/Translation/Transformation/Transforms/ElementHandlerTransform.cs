using AsmResolver.DotNet;
using AsmResolver.PE.DotNet.Cil;
using de4vmp.Core.Architecture;
using de4vmp.Core.Services;
using de4vmp.Core.Translation.Transformation.Converters;

namespace de4vmp.Core.Translation.Transformation.Transforms;

public class ElementHandlerTransform : ITransform {
    public IEnumerable<VmpCode> Accepts {
        get {
            yield return VmpCode.CilLdelemCode;
            yield return VmpCode.CilStelemCode;
        }
    }

    public void Transform(VmpRecompiler recompiler, VmpInstruction instruction) {
        if (instruction.Operand is not ITypeDescriptor typeDescriptor)
            throw ExceptionService.ThrowInvalidOperand<VmpInstruction, ITypeDescriptor>(instruction);

        recompiler.AddInstruction(new CilInstruction(CilOpCodes.Pop));
        recompiler.AddInstruction(instruction.Address, new CilInstruction(CilOpCodes.Nop));

        switch (instruction.Code) {
            case VmpCode.CilLdelemCode:
                recompiler.AddInstruction(
                    new CilInstruction(recompiler.LookupByName<LdelemConvert>(typeDescriptor.Name)));
                break;
            case VmpCode.CilStelemCode:
                recompiler.AddInstruction(
                    new CilInstruction(recompiler.LookupByName<StelemConvert>(typeDescriptor.Name)));
                break;
        }
    }
}