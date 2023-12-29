using de4vmp.Core.Architecture;

namespace de4vmp.Core.DataFlow.Blocks; 

public class BasicBlock : BaseBlock {
    public BasicBlock(IList<VmpInstruction> instructions) {
        AddRange(instructions);
    }

    public override void MergeWith(IEnumerable<VmpInstruction> instructions) {
        var result = new List<VmpInstruction>(instructions);
        result.AddRange(this);

        Clear();
        foreach (var instruction in result) {
            Add(instruction);
        }
    }

    public override VmpInstruction? Footer => this.LastOrDefault();
    public override VmpInstruction? Header => this.FirstOrDefault();
}