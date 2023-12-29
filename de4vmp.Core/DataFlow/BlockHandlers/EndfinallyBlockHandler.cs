using de4vmp.Core.Architecture;
using de4vmp.Core.Architecture.ExceptionHandlers;

namespace de4vmp.Core.DataFlow.BlockHandlers; 

public class EndfinallyBlockHandler : IBlockHandler {
    public BlockExplorerState Explore(BlockExplorer explorer, BlockExplorerContext context, VmpInstruction instruction) {
        if (context.BlockExplorerFlags is not BlockFlags.ExploreHandler)
            return BlockExplorerState.Next;

        if (context.ExceptionHandler.Type is not (VmpExceptionHandlerType.Finally or VmpExceptionHandlerType.Fault))
            return BlockExplorerState.Next;

        if (context.CurrentDimension != 1)
            return BlockExplorerState.Next;
        
        context.ExceptionHandler.HandlerEnd = instruction.Address;
        return BlockExplorerState.Break;
    }
}