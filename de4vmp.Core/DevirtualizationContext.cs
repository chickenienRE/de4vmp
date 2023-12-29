using AsmResolver;
using AsmResolver.DotNet;
using AsmResolver.DotNet.Serialized;
using AsmResolver.PE.File;
using AsmResolver.PE.File.Headers;
using de4vmp.Core.Architecture;
using de4vmp.Core.Services;
using de4vmp.Core.Translation.Resolution;

namespace de4vmp.Core;

public class DevirtualizationContext {
    private readonly IDictionary<uint, VmpFunction> _functions = new Dictionary<uint, VmpFunction>();
    
    private readonly IDictionary<byte, HandlerBase> _handlers = new Dictionary<byte, HandlerBase>();

    public DevirtualizationContext(IReadOnlyList<string> args) {
        if (args.Count != 1)
            throw new ArgumentException("Invalid arguments, please specify path to target.");

        string filePath = Path.GetFullPath(args[0]);
        if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            throw new ArgumentException("Invalid path, please make sure your input is correct.");

        PeFile = PEFile.FromFile(args[0]);
        var dataDirectory = PeFile.OptionalHeader.DataDirectories[(short)DataDirectoryIndex.ClrDirectory];
        if (!dataDirectory.IsPresentInPE)
            throw new ArgumentException("PEFile does not have dotnet metadata! This devirtualizer is for .NET!");

        string? workingDirectory = Path.GetDirectoryName(filePath);
        if (string.IsNullOrEmpty(workingDirectory))
            workingDirectory = Directory.GetCurrentDirectory();

        var readerParameters = new ModuleReaderParameters(workingDirectory, EmptyErrorListener.Instance);
        Module = ModuleDefinition.FromFile(PeFile, readerParameters);
    }
    
    public void ImportFunction(VmpFunction function) {
        if (_functions.TryAdd(function.Rva, function))
            return;

        throw ExceptionService.ThrowInferredException(function, nameof(function));
    }
    
    public bool TryLookupFunction(uint address, out VmpFunction function) {
        return _functions.TryGetValue(address, out function!);
    }
    
    public void ImportHandler(byte opCodeValue, HandlerBase handler) {
        if (_handlers.TryAdd(opCodeValue, handler))
            return;
        
        throw ExceptionService.ThrowInferredException(handler, nameof(handler));
    }
    
    public bool TryLookupHandler(byte opCodeValue, out HandlerBase handler) {
        return _handlers.TryGetValue(opCodeValue, out handler!);
    }

    public PEFile PeFile { get; }

    public ModuleDefinition Module { get; }

    public IEnumerable<VmpFunction> Functions => _functions.Values;
}