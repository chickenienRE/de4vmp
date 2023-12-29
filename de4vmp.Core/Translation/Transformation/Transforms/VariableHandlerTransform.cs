using AsmResolver.PE.DotNet.Cil;
using de4vmp.Core.Architecture;
using de4vmp.Core.Architecture.Annotations.Variables;
using de4vmp.Core.Architecture.Annotations.Variables.Variants;
using de4vmp.Core.Services;

namespace de4vmp.Core.Translation.Transformation.Transforms;

public class VariableHandlerTransform : ITransform {
    private readonly IDictionary<VmpCode, CilOpCode> _argumentMapping = new Dictionary<VmpCode, CilOpCode> {
        [VmpCode.VmilStoreVarCode] = CilOpCodes.Starg,
        [VmpCode.VmilLoadVarCode] = CilOpCodes.Ldarg,
        [VmpCode.VmilLoadVarACode] = CilOpCodes.Ldarga
    };

    private readonly IDictionary<VmpCode, CilOpCode> _variableMapping = new Dictionary<VmpCode, CilOpCode> {
        [VmpCode.VmilStoreVarCode] = CilOpCodes.Stloc,
        [VmpCode.VmilLoadVarCode] = CilOpCodes.Ldloc,
        [VmpCode.VmilLoadVarACode] = CilOpCodes.Ldloca
    };

    public IEnumerable<VmpCode> Accepts {
        get {
            yield return VmpCode.VmilStoreVarCode;
            yield return VmpCode.VmilLoadVarCode;
            yield return VmpCode.VmilLoadVarACode;
        }
    }

    public void Transform(VmpRecompiler recompiler, VmpInstruction instruction) {
        if (instruction.Annotation is not VariableAnnotation annotation)
            throw ExceptionService.ThrowInvalidAnnotation(instruction);

        if (annotation.IsArgument) {
            if (annotation.Variable is not ArgumentTypeLocal argument)
                throw new VmpRecompilerException("Unreachable.");

            recompiler.AddInstruction(instruction.Address,
                new CilInstruction(_argumentMapping[instruction.Code], argument.Definition));
        }
        else {
            var variable = recompiler.GetVariable(annotation.Index);
            recompiler.AddInstruction(instruction.Address,
                new CilInstruction(_variableMapping[instruction.Code], variable));
        }
    }
}