using AsmResolver.DotNet;
using AsmResolver.DotNet.Signatures.Types;

namespace de4vmp.Core.Architecture.Annotations.Variables.Variants;

public class VariableTypeLocal : IVariable {
    public VariableTypeLocal(ITypeDescriptor typeDescriptor) {
        VariableType = typeDescriptor;
    }

    public ITypeDescriptor VariableType { get; }

    public TypeSignature Signature => VariableType.ToTypeSignature();
    public bool IsArgument => false;

    public override string ToString() {
        return $"var_{VariableType}";
    }
}