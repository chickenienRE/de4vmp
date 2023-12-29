using System.Reflection;
using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.PE.DotNet.Cil;

namespace de4vmp.Core.Translation.Resolution;

public class HandlerResolver {
    private readonly IEnumerable<HandlerBase> _handlers;

    private readonly ILogger _logger;

    private readonly IList<CilCode> _signature = new List<CilCode> {
        CilCode.Ldarg,
        CilCode.Ldfld,
        CilCode.Ldc_I4,
        CilCode.Ldarg,
        CilCode.Ldftn,
        CilCode.Newobj,
        CilCode.Callvirt
    };

    public HandlerResolver(ILogger logger) {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _handlers = CollectHandlers(Assembly.GetExecutingAssembly().GetTypes());
    }

    public IDictionary<byte, HandlerBase> Resolve(IEnumerable<TypeDefinition> types) {
        if (!TryResolveContainer(types, out var instructions))
            throw new HandlerResolverException("Unable to resolve virtual opcode container!");

        return Resolve(instructions);
    }

    private IDictionary<byte, HandlerBase> Resolve(CilInstructionCollection instructions) {
        instructions.ExpandMacros();

        _logger.Information(this, "Resolving all virtual handlers...");
        var subsequences = GetPatterns(instructions, _signature);
        if (subsequences.Count != 256)
            throw new DevirtualizationException("Couldn't find all subsequences",
                new ArgumentOutOfRangeException($"Subsequences count: {subsequences.Count}"));

        var result = new Dictionary<byte, HandlerBase>();
        foreach (var subsequence in subsequences) {
            byte handlerOffset = (byte)subsequence[2].GetLdcI4Constant();

            if (subsequence[4].Operand is not MethodDefinition { CilMethodBody: { } cilMethodBody })
                continue;

            var collection = cilMethodBody.Instructions;
            collection.ExpandMacros();

            if (TryIdentify(collection, out var handler)) {
                result.Add(handlerOffset, handler);
            }
        }

        ResolveInvalidHandlers(result, subsequences.Count);
        return result;
    }

    private void ResolveInvalidHandlers(IDictionary<byte, HandlerBase> handlers, int count) {
        foreach (var handler in _handlers.Where(handler => handlers.Values.Contains(handler)))
            _logger.Warning(this, $"Handler: {handler.Translates} could not be resolved!");

        _logger.Debug(this, $"Resolved {handlers.Count} out of {count} embedded handlers");
    }

    private bool TryIdentify(CilInstructionCollection instructions, out HandlerBase result) {
        var handlers = _handlers.Where(handler => handler.Resolve(instructions)).ToArray();
        result = handlers.FirstOrDefault()!;

        if (handlers.Length <= 1)
            return handlers.Length != 0;

        foreach (var handler in handlers)
            _logger.Warning(this, $"Trace: {handler.GetType().Namespace} Translates: {handler.Translates}");

        throw new HandlerResolverException($"Multiple handlers match same virtual opcode! Count: {handlers.Length}");
    }

    private static IList<List<CilInstruction>> GetPatterns(CilInstructionCollection instructions,
        IList<CilCode> signature) {
        var result = new List<List<CilInstruction>>();
        for (int i = 0; i < instructions.Count; i++) {
            var current = new List<CilInstruction>();

            for (int j = i, k = 0; j < instructions.Count && k < signature.Count; j++, k++) {
                if (instructions[j].OpCode.Code != signature[k])
                    break;

                current.Add(instructions[j]);
            }

            if (current.Count != signature.Count)
                continue;

            result.Add(current);
        }

        return result;
    }

    private static bool TryResolveContainer(IEnumerable<TypeDefinition> types,
        out CilInstructionCollection instructions) {
        var methods = from typeDefinition in types
            from methodDefinition in typeDefinition.Methods
            where !methodDefinition.IsStatic && methodDefinition.IsConstructor
            select methodDefinition;

        instructions = default!;
        foreach (var method in methods) {
            if (method.CilMethodBody is not { } methodBody)
                continue;

            instructions = methodBody.Instructions;
            if (instructions.Count > 1800) return true;
        }

        return false;
    }

    private static IEnumerable<HandlerBase> CollectHandlers(IEnumerable<Type> types) =>
        from type in types.Where(type => !type.IsAbstract)
        where type.IsAssignableTo(typeof(HandlerBase))
        select CreateInstanceOfType<HandlerBase>(type);

    private static T CreateInstanceOfType<T>(Type type) where T : class =>
        Activator.CreateInstance(type) as T ??
        throw new DevirtualizationException($"Unable to build handler: {type.Name}");
}