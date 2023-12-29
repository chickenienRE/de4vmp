using de4vmp.Core.Architecture;

namespace de4vmp.Core.DataFlow.BlockHandlers; 

public class PushBlockHandler : IBlockHandler {
    public BlockExplorerState Explore(BlockExplorer explorer, BlockExplorerContext context, VmpInstruction instruction) {
        context.CurrentDimension++;
        return BlockExplorerState.Next;
    }
}