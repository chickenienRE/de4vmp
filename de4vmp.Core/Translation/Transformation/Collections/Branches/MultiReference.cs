using AsmResolver.PE.DotNet.Cil;

namespace de4vmp.Core.Translation.Transformation.Collections.Branches; 

public class MultiReference : IReference {
    public MultiReference(IList<uint> addresses) {
        Addresses = addresses;
    }

    private IList<uint> Addresses { get; }
    
    public void Apply(CilInstruction instruction, IlInstructionCollection instructions) {
        instruction.Operand = Addresses.Select(instructions.GetLabelByAddress).ToList();
    }
}