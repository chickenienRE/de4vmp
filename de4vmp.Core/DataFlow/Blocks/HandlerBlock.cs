namespace de4vmp.Core.DataFlow.Blocks; 

public class HandlerBlock {
    public HandlerBlock(BaseBlock tryStart, BaseBlock handlerStart) {
        TryStartBlock = tryStart;
        HandlerStartBlock = handlerStart;

        TryBlocks.Add(tryStart);
        HandlerBlocks.Add(handlerStart);
    }
    
    public BaseBlock TryStartBlock { get; }
    
    public BaseBlock HandlerStartBlock { get; }

    public void AddTryBlock(BaseBlock block) {
        if (TryBlocks.Contains(block))
            return;

        TryBlocks.Add(block);
    }

    public void AddHandlerBlock(BaseBlock block) {
        if (HandlerBlocks.Contains(block))
            return;

        HandlerBlocks.Add(block);
    }

    public IList<BaseBlock> TryBlocks { get; } = new List<BaseBlock>();

    public IList<BaseBlock> HandlerBlocks { get; } = new List<BaseBlock>();
}