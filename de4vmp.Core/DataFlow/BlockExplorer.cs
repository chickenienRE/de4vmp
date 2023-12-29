using System.Collections.ObjectModel;
using de4vmp.Core.Architecture;
using de4vmp.Core.Architecture.ExceptionHandlers;
using de4vmp.Core.Architecture.ExceptionHandlers.Variants;
using de4vmp.Core.DataFlow.BlockHandlers;
using de4vmp.Core.DataFlow.Blocks;

namespace de4vmp.Core.DataFlow; 

public class BlockExplorer {
    private readonly IDictionary<VmpCode, IBlockHandler> _blockHandlers = new Dictionary<VmpCode, IBlockHandler> {
        [VmpCode.CilLeaveCode] = new LeaveBlockHandler(),
        [VmpCode.VmilPushEhCode] = new PushBlockHandler(),
        [VmpCode.CilEndfinallyCode] = new EndfinallyBlockHandler(),
        [VmpCode.VmilBrCode] = new BrBlockHandler()
    };
    
    private IList<VmpExceptionHandlerBase> _exceptionHandlers;
    private ObservableCollection<BaseBlock> _blocks;

    public void Resolve(VmpFunction function) {
        _blocks = new ObservableCollection<BaseBlock>(function.Blocks);
        _exceptionHandlers = new List<VmpExceptionHandlerBase>(function.Handlers);
        
        foreach (var handler in _exceptionHandlers) {
            try {
                var tryBlock = GetBlockByAddress(handler.TryStart);
                var handlerBlock = GetBlockByAddress(handler.HandlerStart);
                var context = new BlockExplorerContext(tryBlock, handlerBlock, handler);
            
                context.BlockExplorerFlags = BlockFlags.ExploreTry;
                context.Enqueue(new Block(tryBlock, handler.TryStart));
                RunThroughBlock(context);

                context.BlockExplorerFlags = BlockFlags.ExploreHandler;
                context.Enqueue(new Block(handlerBlock, handler.HandlerStart));
                RunThroughBlock(context);
            
                int globalIndex = _blocks.IndexOf(tryBlock);
            
                MoveBlocks(OrderTryBlocks(context), ref globalIndex);
                MoveBlocks(OrderHandlerBlocks(context), ref globalIndex);
            }
            catch (Exception e) {
                
            }
        }
    
        function.Blocks = _blocks;
    }
    
    private IEnumerable<BaseBlock> OrderTryBlocks(BlockExplorerContext context) {
        var tryStartBlock = context.HandlerBlock.TryStartBlock;
        
        if (context.HandlerBlock.TryBlocks.Count == 1) {
            yield return tryStartBlock;
            yield break;
        }
        
        var tryEndBlock =
            context.HandlerBlock.TryBlocks.First(
                block => block.IsStateKnown(context.ExceptionHandler.TryEnd));

        yield return tryStartBlock;
        
        foreach (var block in context.HandlerBlock.TryBlocks) {
            if (block != tryStartBlock && block != tryEndBlock) {
                yield return block;
            }
        }

        yield return tryEndBlock;
    }
    
    private IEnumerable<BaseBlock> OrderHandlerBlocks(BlockExplorerContext context) {
        var handlerStartBlock = context.HandlerBlock.HandlerStartBlock;
        
        if (context.HandlerBlock.HandlerBlocks.Count == 1) {
            yield return handlerStartBlock;
            yield break;
        }
        
        var handlerEndBlock =
            context.HandlerBlock.HandlerBlocks.First(
                block => block.IsStateKnown(context.ExceptionHandler.HandlerEnd));

        yield return handlerStartBlock;
        
        foreach (var block in context.HandlerBlock.HandlerBlocks) {
            if (block != handlerStartBlock && block != handlerEndBlock) {
                yield return block;
            }
        }

        yield return handlerEndBlock;
    }

    private void MoveBlocks(IEnumerable<BaseBlock> blocks, ref int globalIndex) {
        foreach (var baseBlock in blocks) {
            int blockIndex = _blocks.IndexOf(baseBlock);
            if (blockIndex == globalIndex) {
                continue;
            }
                
            if (globalIndex >= _blocks.Count - 1) {
                _blocks.Move(blockIndex, globalIndex);
                continue;
            }

            _blocks.Move(blockIndex, ++globalIndex);
        }
    }
    
    private void RunThroughBlock(BlockExplorerContext context) {
        while (context.TryDequeue(out var block)) {
            var state = BlockExplorerState.Next;

            foreach (var instruction in block.BaseBlock.Where(instruction => instruction.Address >= block.Address)) {
                if (_blockHandlers.TryGetValue(instruction.Code, out var result)) {
                    state = result.Explore(this, context, instruction);
                }

                if (state is not BlockExplorerState.Break) 
                    continue;
                
                if (context.CanDequeue())
                    break;

                return;
            }
        }
    }
    
    public BaseBlock GetBlockByAddress(uint address) {
        return _blocks.First(block => block.IsStateKnown(address));
    }
}