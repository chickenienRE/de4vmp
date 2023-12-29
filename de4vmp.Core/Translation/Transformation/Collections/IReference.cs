using AsmResolver.PE.DotNet.Cil;

namespace de4vmp.Core.Translation.Transformation.Collections; 

public interface IReference {
    public void Apply(CilInstruction instruction, IlInstructionCollection instructions);
}