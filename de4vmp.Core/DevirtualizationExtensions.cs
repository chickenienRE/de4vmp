using System.Diagnostics.CodeAnalysis;
using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;

namespace de4vmp.Core;

public static class DevirtualizationExtensions {
    public static bool IsFullnameType(this TypeSignature signature, Type type) {
        return signature.FullName.Equals(type.FullName);
    }

    public static bool HasReturnTypeOf(this MethodDefinition methodDefinition, Type type) {
        return methodDefinition.Signature is { } signature && signature.ReturnType.FullName.Equals(type.FullName);
    }

    public static bool AreSignatureEqual(this CilInstructionCollection instructions, IEnumerable<CilCode> signature) {
        return !signature.Where((code, i) => instructions[i].OpCode.Code != code).Any();
    }

    public static bool IsSignatureEqual(this MethodDefinition methodDefinition, IEnumerable<CilCode> signature) {
        if (methodDefinition.CilMethodBody is null)
            return false;

        var instructions = methodDefinition.CilMethodBody.Instructions;
        instructions.ExpandMacros();

        return AreSignatureEqual(instructions, signature);
    }

    public static bool HasCode(this MethodDefinition methodDefinition, CilCode code) {
        var body = methodDefinition.CilMethodBody;
        return body is not null && HasCode(body.Instructions, code);
    }

    public static bool HasCode(this CilInstructionCollection instructions, CilCode code) {
        return instructions.Any(instruction => instruction.OpCode.Code == code);
    }

    public static int GetCodeCount(this MethodDefinition methodDefinition, CilCode code) {
        var body = methodDefinition.CilMethodBody;
        if (body is null)
            throw new ArgumentException(nameof(body));

        return GetCodeCount(body.Instructions, code);
    }

    public static int GetCodeCount(this CilInstructionCollection instructions, CilCode code) {
        return instructions.Count(instruction => instruction.OpCode.Code == code);
    }

    public static T GetOperandAs<T>(this CilInstructionCollection instructions, int index) where T : class {
        object? value = instructions[index].Operand;

        if (value is T result) return result;

        throw new InvalidOperationException($"{value} != {typeof(T)}");
    }

    public static bool HasMethodReference(this CilInstructionCollection instructions, string name) {
        return instructions.GetAllTFromCollection<IMethodDescriptor>(CilOperandType.InlineMethod)
            .Any(methodDescriptor => methodDescriptor.Name == name);
    }

    public static bool HasTypeReference(this CilInstructionCollection instructions, string name) {
        return instructions.GetAllTFromCollection<ITypeDescriptor>(CilOperandType.InlineType)
            .Any(typeDescriptor => typeDescriptor.Name == name);
    }

    public static bool HasExceptionReference(this CilInstructionCollection instructions, string name) {
        return instructions.GetAllTFromCollection<IMethodDescriptor>(CilOperandType.InlineMethod)
            .Any(methodDescriptor => methodDescriptor.DeclaringType?.Name == name);
    }

    public static IEnumerable<T> GetAllTFromCollection<T>(this CilInstructionCollection instructions,
        CilOperandType cilOperandType) where T : class {
        return from instruction in instructions
            where instruction.OpCode.OperandType == cilOperandType
            select instruction.Operand as T;
    }
    
    public static bool TryPop<T, TType>(this Stack<TType> stack, [MaybeNullWhen(false)] out T result) where T : class {
        if (stack.Any()) {
            var symbolicValue = stack.Pop();
            if (symbolicValue is T value) {
                result = value;
                return true;
            }
            
            stack.Push(symbolicValue);
        }

        result = default;
        return false;
    }
    
    public static bool TryPeek<T, TType>(this Stack<TType> stack, [MaybeNullWhen(false)] out T result) where T : class {
        if (stack.Any()) {
            var symbolicValue = stack.Peek();
            if (symbolicValue is T value) {
                result = value;
                return true;
            }
        }

        result = default;
        return false;
    }
}