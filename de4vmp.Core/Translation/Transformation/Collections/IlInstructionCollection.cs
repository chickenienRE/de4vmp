using AsmResolver.PE.DotNet.Cil;

namespace de4vmp.Core.Translation.Transformation.Collections; 

public class IlInstructionCollection : List<IlInstruction> {
    public ICilLabel GetLabelByAddress(uint address) {
        return GetInstructionByAddress(address).Instruction.CreateLabel();
    }

    private IlInstruction GetInstructionByAddress(uint address) {
        foreach (var ilInstruction in this.Where(ilInstruction => ilInstruction.Rva == address)) {
            return ilInstruction;
        }

        throw new ArgumentOutOfRangeException(nameof(address));
    }
    
    public ICilLabel GetLabelAfterInstruction(uint address) {
        var instruction = GetInstructionByAddress(address);
        var result = this[IndexOf(instruction) + 1];
        return result.Instruction.CreateLabel();
    }
}