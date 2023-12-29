using de4vmp.Core.Architecture;
using de4vmp.Core.Architecture.Annotations.ControlFlow;
using de4vmp.Core.Services;

namespace de4vmp.Core.DataFlow.BlockHandlers; 

public class BrBlockHandler : IBlockHandler {
    public BlockExplorerState Explore(BlockExplorer explorer, BlockExplorerContext context, VmpInstruction instruction) {
        switch (instruction.Annotation) {
            case SingleJumpAnnotation annotation:
                if (annotation.WillReturn)
                    break;
                
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
            case DoubleJumpAnnotation annotation:
                uint firstAddress = annotation.Type ? annotation.SecondAddress : annotation.FirstAddress;
                uint secondAddress = annotation.Type ? annotation.FirstAddress : annotation.SecondAddress;
                var firstBlock = explorer.GetBlockByAddress(firstAddress);
                var secondBlock = explorer.GetBlockByAddress(secondAddress);
                
                switch (context.BlockExplorerFlags) {
                    case BlockFlags.ExploreTry:
                        context.HandlerBlock.AddTryBlock(firstBlock);
                        context.HandlerBlock.AddTryBlock(secondBlock);
                        break;
                    case BlockFlags.ExploreHandler:
                        context.HandlerBlock.AddHandlerBlock(firstBlock);
                        context.HandlerBlock.AddHandlerBlock(secondBlock);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(context));
                }
                
                context.Enqueue(new Block(firstBlock, firstAddress));
                context.Enqueue(new Block(secondBlock, secondAddress));
                return BlockExplorerState.Next;
            case MultipleJumpAnnotation annotation:
                break;
            default:
                throw ExceptionService.ThrowInvalidAnnotation(instruction);
        }

        return BlockExplorerState.Break;
    }
}