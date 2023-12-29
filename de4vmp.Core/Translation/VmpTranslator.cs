using AsmResolver.DotNet;
using AsmResolver.DotNet.Signatures;
using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.IO;
using AsmResolver.PE.DotNet.Metadata.Tables;
using AsmResolver.PE.DotNet.Metadata.Tables.Rows;
using de4vmp.Core.Architecture;
using de4vmp.Core.DataFlow;
using de4vmp.Core.DataFlow.Blocks;
using de4vmp.Core.Translation.Emulation.Values;

namespace de4vmp.Core.Translation;

public class VmpTranslator {
    private readonly DevirtualizationContext _context;
    private readonly Stack<VmpFunction> _functions;
    private readonly BlockExplorer _blockExplorer;
    private readonly Stack<uint> _states;

    private IList<VmpInstruction> _instructions;
    private VmpTranslatorContext _translator;

    private IList<BaseBlock> Blocks => Function.Blocks;

    public VmpTranslator(DevirtualizationContext context) {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _functions = new Stack<VmpFunction>(context.Functions);
        _blockExplorer = new BlockExplorer();
        _states = new Stack<uint>();
    }

    public VmpFunction Function { get; private set; }

    public bool TryGetNextFunction(out VmpFunction function) {
        return _functions.TryPop(out function!);
    }

    public VmpFunction GetOrCreateFunction(bool isStatic, uint address) {
        var binaryReader = CreateReader(address);
        var signatures = new TypeSignature[binaryReader.ReadInt16()];
        for (int i = signatures.Length - 1; i >= 0; i--)
            signatures[i] = ResolveMember<ITypeDescriptor>(binaryReader.ReadInt32())
                .ToTypeSignature();

        int returnTypeToken = binaryReader.ReadInt32();
        uint functionAddress = binaryReader.Rva;

        if (_context.TryLookupFunction(functionAddress, out var function))
            return function;

        var returnType = returnTypeToken is 0
            ? _context.Module.CorLibTypeFactory.Void
            : ResolveMember<ITypeDescriptor>(returnTypeToken).ToTypeSignature();

        string methodName = $"EXPORT_{(isStatic ? "STATIC" : "INSTANCE")}_{functionAddress:X}";
        var methodSignature = new MethodSignature(CallingConventionAttributes.Default, returnType, signatures);
        var methodDefinition = new MethodDefinition(methodName,
            MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Static, methodSignature);
        var virtualFunction = new VmpFunction(methodDefinition, functionAddress);

        _context.Module.GetOrCreateModuleType().Methods.Add(methodDefinition);
        _context.ImportFunction(virtualFunction);
        _functions.Push(virtualFunction);
        return virtualFunction;
    }

    public T ResolveMember<T>(MetadataToken metadataToken) where T : class {
        if (_context.Module.LookupMember(metadataToken) is T result) return result;

        throw new VmpTranslatorException($"MDToken {metadataToken} is not {typeof(T)}!");
    }

    public string ResolveString(MetadataToken metadataToken) {
        return _context.Module.LookupString(metadataToken);
    }

    public bool CanCreateReader(uint address) {
        return TryCreateReader(address, out _);
    }

    public BinaryStreamReader CreateReader(uint address) {
        if (TryCreateReader(address, out var binaryStreamReader)) return binaryStreamReader;

        throw new VmpTranslatorException($"Couldn't create reader at address: {address}");
    }

    private bool TryCreateReader(uint address, out BinaryStreamReader reader) {
        return _context.PeFile.TryCreateReaderAtRva(address, out reader);
    }

    public void AddState(uint state) {
        if (IsStateKnown(state)) 
            return;

        if (_states.Contains(state)) 
            return;

        _states.Push(state);
    }

    private bool IsStateKnown(uint state) {
        return Blocks.Any(block => block.IsStateKnown(state));
    }

    public void TranslateFunction(VmpFunction function) {
        Function = function;

        _translator = new VmpTranslatorContext();
        var signature = function.Parent.Signature;
        if (signature is not null && signature.GetTotalParameterCount() > 0)
            _translator.ValueStack.Push(new ParameterArrayValue(function.Parent.Parameters));

        _states.Push(function.Rva);

        while (_states.TryPop(out uint state)) {
            if (IsStateKnown(state))
                continue;

            var block = ReadCodeBlock(state);
            if (!Blocks.Contains(block))
                Blocks.Add(block);
        }
        
        _blockExplorer.Resolve(function);
    }

    private BaseBlock ReadCodeBlock(uint blockStart) {
        _instructions = new List<VmpInstruction>();
        var reader = CreateReader(blockStart);

        var state = VmpTranslatorState.Next;
        while (state is VmpTranslatorState.Next) {
            uint address = reader.Rva;

            foreach (var block in Blocks) {
                var header = block.Header;
                if (header is null)
                    continue;

                if (header.Address != address)
                    continue;

                block.MergeWith(_instructions);
                return block;
            }

            byte opCodeValue = reader.ReadByte();
            if (!_context.TryLookupHandler(opCodeValue, out var handler))
                throw new DevirtualizationException(
                    $"Discovered UnSupported OpCode: {opCodeValue} at address: {address}");

            var instruction = new VmpInstruction(address, handler.Translates);
            state = handler.Translate(this, ref reader, instruction, _translator);
            _instructions.Add(instruction);
        }

        return new BasicBlock(_instructions);
    }
}