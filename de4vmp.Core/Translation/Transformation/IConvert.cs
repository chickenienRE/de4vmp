using AsmResolver.PE.DotNet.Cil;

namespace de4vmp.Core.Translation.Transformation; 

public interface IConvert {
    public CilOpCode Resolve(string name);
    public bool TryResolve(string name, out CilOpCode opCode);
}