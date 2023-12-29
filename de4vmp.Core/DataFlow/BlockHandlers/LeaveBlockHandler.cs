using de4vmp.Core.Architecture;
using de4vmp.Core.Architecture.Annotations.ControlFlow;
using de4vmp.Core.Services;

namespace de4vmp.Core.DataFlow.BlockHandlers; 

public class LeaveBlockHandler : IBlockHandler {
    public BlockExplorerState Explore(BlockExplorer explorer, BlockExplorerContext context, VmpInstruction instruction) {
        switch (context.CurrentDimension) {
            case > 1:
                context.CurrentDimension--;

                if (instruction.Annotation is not SingleJumpAnnotation annotation)
                    throw ExceptionService.ThrowInvalidAnnotation(instruction);
                
                var block = explorer.GetBlockByAddress(annotation.Address);
                
                switch (context.BlockExplorerFlags) {
                    case BlockFlags.ExploreTry:
                        context.HandlerBlock.AddTryBlock(block);
                        break;
                    case BlockFlags.ExploreHandler:
                        context.HandlerBlock.AddHandlerBlock(block);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(context));
                }
                
                context.Enqueue(new Block(block, annotation.Address));
                return BlockExplorerState.Next;
            case 1:
                switch (context.BlockExplorerFlags) {
                    case BlockFlags.ExploreTry:
                        context.ExceptionHandler.TryEnd = instruction.Address;
                        break;
                    case BlockFlags.ExploreHandler:
                        context.ExceptionHandler.HandlerEnd = instruction.Address;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(context.BlockExplorerFlags));
                }
                
                return BlockExplorerState.Break;
            case < 1:
                throw new ArgumentOutOfRangeException(nameof(context.CurrentDimension));
        }
    }
}