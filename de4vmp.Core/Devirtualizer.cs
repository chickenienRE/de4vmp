using AsmResolver;
using AsmResolver.DotNet;
using AsmResolver.DotNet.Builder;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.PE.DotNet.Builder;
using de4vmp.Core.Pipeline;
using de4vmp.Core.Pipeline.Phases;

namespace de4vmp.Core; 

public class Devirtualizer {
    private readonly ILogger _logger;
    private readonly IEnumerable<IPhase> _phases;

    public Devirtualizer(ILogger logger) {
         _logger = logger ?? throw new ArgumentNullException(nameof(logger));
         _phases = new List<IPhase> {
             new FunctionResolutionPhase(),
             new HandlerResolutionPhase(),
             new FunctionTransformPhase()
         };
    }
    
    public void Devirtualize(DevirtualizationContext context) {
        _logger.Debug(this, $"Imported target {context.Module}");

        foreach (var phase in _phases) {
            phase.Run(_logger, context);
        }

        SaveModule(context.Module);
    }

    private void SaveModule(ModuleDefinition moduleDefinition) {
        _logger.Information(this, "Attempting to rebuild module...");

        var builder = new ManagedPEImageBuilder {
            DotNetDirectoryFactory = new DotNetDirectoryFactory {
                MetadataBuilderFlags = MetadataBuilderFlags.PreserveAll,
                MethodBodySerializer = new CilMethodBodySerializer {
                    ComputeMaxStackOnBuildOverride = false,
                    VerifyLabelsOnBuildOverride = false
                }
            }
        };

        var builderResult = builder.CreateImage(moduleDefinition);
        if (builderResult.ErrorListener is DiagnosticBag diagnosticBag) {
            foreach (var exception in diagnosticBag.Exceptions) {
                _logger.Warning(this, exception.Message);
            }   
        }
        
        if (builderResult.HasFailed)
            throw new DevirtualizationException("Unable to rebuild image.", 
                new BadImageFormatException(moduleDefinition.Name));

        string? path = moduleDefinition.FilePath;
        string filePath = Path.Combine(Path.GetDirectoryName(path) ?? string.Empty,
            $"{Path.GetFileNameWithoutExtension(path)}_devirtualized{Path.GetExtension(path)}");

        var fileBuilder = new ManagedPEFileBuilder();
        var file = fileBuilder.CreateFile(builderResult.ConstructedImage);
        file.Write(filePath);
            
        _logger.Debug(this, $"Wrote result at {filePath}");
    }
}