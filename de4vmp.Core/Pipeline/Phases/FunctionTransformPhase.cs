using de4vmp.Core.Translation;
using de4vmp.Core.Translation.Transformation;

namespace de4vmp.Core.Pipeline.Phases; 

public class FunctionTransformPhase : IPhase {
    public void Run(ILogger logger, DevirtualizationContext context) {
        logger.Information(this, "Translating functions...");
        
        var translator = new VmpTranslator(context);
        while (translator.TryGetNextFunction(out var function)) {
            logger.Debug(this, $"Translating function_{function.Rva:X4}");
            translator.TranslateFunction(function);
        }
        
        logger.Information(this, "Recompiling functions...");
        
        var recompiler = new VmpRecompiler(context);
        foreach (var function in context.Functions) {
            logger.Debug(this, $"Transforming function_{function.Rva:X4}");
            function.Parent.CilMethodBody = recompiler.Recompile(function, logger);
        }
    }
}