namespace de4vmp.Core.Architecture.Annotations.Variables;

public class VariableAnnotation : IAnnotation {
    public VariableAnnotation(short index, IVariable variable) {
        Index = index;
        Variable = variable;
        IsArgument = variable.IsArgument;
    }

    public short Index { get; }
    public bool IsArgument { get; }
    public IVariable Variable { get; }

    public override string ToString() {
        return $"{(IsArgument ? "arg" : "var")}_{Index}";
    }
}