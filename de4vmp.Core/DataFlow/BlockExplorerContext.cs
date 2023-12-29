using de4vmp.Core.Architecture.ExceptionHandlers;
using de4vmp.Core.DataFlow.Blocks;

namespace de4vmp.Core.DataFlow; 

public class BlockExplorerContext {
    private readonly IList<Block> _blockCache = new List<Block>();
    
    private readonly Queue<Block> _blockQueue = new();
    
    public BlockExplorerContext(BaseBlock tryBlock, BaseBlock handlerBlock, VmpExceptionHandlerBase exceptionHandler) {
        HandlerBlock = new HandlerBlock(tryBlock, handlerBlock);
        ExceptionHandler = exceptionHandler;
    }

    public void Enqueue(Block item) {
        if (_blockCache.Contains(item))
            return;
        
        _blockQueue.Enqueue(item);
        _blockCache.Add(item);
    }

    public bool TryDequeue(out Block item) {
        return _blockQueue.TryDequeue(out item!);
    }
    
    public bool CanDequeue() {
        return _blockQueue.Count != 0;
    }

    public HandlerBlock HandlerBlock { get; }
    
    public BlockFlags BlockExplorerFlags { get; set; }
    
    public VmpExceptionHandlerBase ExceptionHandler { get; }

    public int CurrentDimension { get; set; }
}