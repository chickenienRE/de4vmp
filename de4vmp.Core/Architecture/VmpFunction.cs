using AsmResolver.DotNet;
using de4vmp.Core.Architecture.Annotations.Variables;
using de4vmp.Core.Architecture.ExceptionHandlers;
using de4vmp.Core.DataFlow.Blocks;

namespace de4vmp.Core.Architecture; 

public class VmpFunction {
    public MethodDefinition Parent { get; }
    public uint Rva { get; }

    public VmpFunction(MethodDefinition parent, uint rva) {
        Parent = parent;
        Rva = rva;
    }
    
    public IList<BaseBlock> Blocks { get; set; } = new List<BaseBlock>();

    public IList<IVariable> Variables { get; } = new List<IVariable>();
    
    public IList<VmpExceptionHandlerBase> Handlers { get; } = new List<VmpExceptionHandlerBase>();
    
    public override string ToString() {
        return $"function:{Parent.Name}_{Rva:X4}";
    }
}