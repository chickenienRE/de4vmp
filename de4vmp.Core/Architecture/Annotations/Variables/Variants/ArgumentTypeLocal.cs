using AsmResolver.DotNet.Collections;
using AsmResolver.DotNet.Signatures.Types;

namespace de4vmp.Core.Architecture.Annotations.Variables.Variants;

public class ArgumentTypeLocal : IVariable {
    public ArgumentTypeLocal(Parameter definition) {
        Definition = definition;
    }

    public Parameter Definition { get; }

    public TypeSignature Signature => Definition.ParameterType;
    public bool IsArgument => true;

    public override string ToString() {
        return $"arg_{Definition}";
    }
}