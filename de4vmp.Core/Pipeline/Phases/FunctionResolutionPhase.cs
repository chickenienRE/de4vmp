using de4vmp.Core.Services;

namespace de4vmp.Core.Pipeline.Phases; 

public class FunctionResolutionPhase : IPhase {
    public void Run(ILogger logger, DevirtualizationContext context) {
        logger.Information(this, "Searching for functions...");
        
        var functions =
            FunctionService.ResolveFunctions(context.Module
                .GetAllTypes()
                .SelectMany(typeDefinition => typeDefinition.Methods));

        foreach (var function in functions) {
            context.ImportFunction(function);
            logger.Debug(this, $"Function: {function.Parent}, RVA: {function.Rva}");
        }
    }
}