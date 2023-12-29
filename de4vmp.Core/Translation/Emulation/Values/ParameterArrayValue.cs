using AsmResolver.DotNet.Collections;

namespace de4vmp.Core.Translation.Emulation.Values; 

public class ParameterArrayValue : IValue {
    public ParameterArrayValue(ParameterCollection parameters) {
        var thisParameter = parameters.ThisParameter;
        if (thisParameter is not null)
            Parameters.Add(thisParameter);
        
        foreach (var parameter in parameters) {
            Parameters.Add(parameter);
        }
    }

    public IList<Parameter> Parameters { get; } = new List<Parameter>();
}