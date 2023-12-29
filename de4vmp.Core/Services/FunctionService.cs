using AsmResolver.DotNet;
using AsmResolver.PE.DotNet.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Services; 

public static class FunctionService {
    public static IEnumerable<VmpFunction> ResolveFunctions(IEnumerable<MethodDefinition> methods) {
        foreach (var methodDefinition in methods) {
            var cilMethodBody = methodDefinition.CilMethodBody;
            if (cilMethodBody is null)
                continue;

            var instructions = cilMethodBody.Instructions;
            for (int i = 0; i < instructions.Count; i++) {
                var instruction = instructions[i];
                if (instruction.OpCode.Code != CilCode.Call || !ResolveVirtualMachineEntry(instruction))
                    continue;

                var constantInstruction = instructions[i - 1];
                if (constantInstruction.OpCode.Code != CilCode.Ldc_I4)
                    continue;

                uint rva = (uint) constantInstruction.GetLdcI4Constant();
                yield return new VmpFunction(methodDefinition, rva);
            }
        }
    }
    
    private static bool ResolveVirtualMachineEntry(CilInstruction instruction) {
        if (instruction.Operand is not MethodDefinition methodDefinition)
            return false;

        var parameters = methodDefinition.Parameters;
        
        if (!methodDefinition.IsPublic || methodDefinition.IsStatic || parameters.Count != 2)
            return false;

        if (methodDefinition.Signature is { } signature && !signature.ReturnType.IsFullnameType(typeof(object)))
            return false;

        return parameters[0].ParameterType.IsFullnameType(typeof(object[])) &&
               parameters[1].ParameterType.IsFullnameType(typeof(int));
    }
}