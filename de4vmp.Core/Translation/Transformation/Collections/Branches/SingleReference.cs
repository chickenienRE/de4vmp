using AsmResolver.PE.DotNet.Cil;

namespace de4vmp.Core.Translation.Transformation.Collections.Branches; 

public class SingleReference : IReference {
    public SingleReference(uint address) {
        Address = address;
    }

    private uint Address { get; }
    
    public void Apply(CilInstruction instruction, IlInstructionCollection instructions) {
        instruction.Operand = instructions.GetLabelByAddress(Address);
    }
}