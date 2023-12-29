using AsmResolver.DotNet.Collections;

namespace de4vmp.Core.Translation.Emulation.Values; 

public class ParameterValue : IValue {
    public ParameterValue(Parameter parameter) {
        Parameter = parameter;
    }
    
    public Parameter Parameter { get; }
}