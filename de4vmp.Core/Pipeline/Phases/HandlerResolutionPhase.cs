using de4vmp.Core.Translation.Resolution;

namespace de4vmp.Core.Pipeline.Phases; 

public class HandlerResolutionPhase : IPhase {
    public void Run(ILogger logger, DevirtualizationContext context) {
        var resolver = new HandlerResolver(logger);
        var handlers = resolver.Resolve(context.Module.TopLevelTypes);

        foreach ((byte key, var value) in handlers) {
            context.ImportHandler(key, value);
        }
    }
}