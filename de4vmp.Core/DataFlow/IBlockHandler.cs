using de4vmp.Core.Architecture;

namespace de4vmp.Core.DataFlow; 

public interface IBlockHandler {
    public BlockExplorerState Explore(BlockExplorer explorer, BlockExplorerContext context, VmpInstruction instruction);
}