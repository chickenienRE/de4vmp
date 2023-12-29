using de4vmp.Core.Architecture;

namespace de4vmp.Core.DataFlow.Blocks; 

public abstract class BaseBlock : List<VmpInstruction> {
    public abstract void MergeWith(IEnumerable<VmpInstruction> instructions);

    public bool IsStateKnown(uint state) {
        return this.Any(ins => ins.Address == state);
    }
    
    public abstract VmpInstruction? Footer { get; }
    public abstract VmpInstruction? Header { get; }
}