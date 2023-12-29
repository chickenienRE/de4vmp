using AsmResolver.DotNet.Code.Cil;
using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using de4vmp.Core.Architecture;
using de4vmp.Core.Architecture.ExceptionHandlers;
using de4vmp.Core.Architecture.ExceptionHandlers.Variants;
using de4vmp.Core.Services;
using de4vmp.Core.Translation.Transformation.Collections;
using de4vmp.Core.Translation.Transformation.Converters;

namespace de4vmp.Core.Translation.Transformation;

public class VmpRecompiler {
    private readonly IDictionary<VmpCode, ITransform> _transforms;
    private readonly DevirtualizationContext _context;
    private readonly IList<IConvert> _converters;

    private IlInstructionCollection _ilInstructions;
    private IList<CilLocalVariable> _variables;

    public VmpRecompiler(DevirtualizationContext context) {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _transforms = new Dictionary<VmpCode, ITransform>();
        foreach (var transform in CollectTransforms(typeof(VmpRecompiler)))
        foreach (var code in transform.Accepts)
            _transforms[code] = transform;

        _converters = new List<IConvert> {
            new ConvConvert(),
            new LdelemConvert(),
            new StelemConvert(),
            new LdindConvert(),
            new StindConvert()
        };
    }

    public CorLibTypeFactory TypeFactory => _context.Module.CorLibTypeFactory;

    public TypeSignature ReturnType { get; private set; }

    private static IEnumerable<ITransform> CollectTransforms(Type type) {
        foreach (var typeDefinition in type.Module.GetTypes()
                     .Where(typeDefinition => typeDefinition.GetInterface(nameof(ITransform)) is not null))
            if (Activator.CreateInstance(typeDefinition) is ITransform transform)
                yield return transform;
    }

    public CilOpCode LookupByName<TConverter>(string? name) where TConverter : IConvert {
        foreach (var item in _converters)
            if (item is TConverter converter)
                return converter.Resolve(name!);

        throw ExceptionService.UnknownConverterException<TConverter>();
    }

    public bool TryLookupByName<TConverter>(string? name, out CilOpCode opCode) where TConverter : IConvert {
        foreach (var item in _converters)
            if (item is TConverter converter)
                return converter.TryResolve(name!, out opCode);

        throw ExceptionService.UnknownConverterException<TConverter>();
    }

    public CilMethodBody Recompile(VmpFunction function, ILogger logger) {
        var methodDefinition = function.Parent;
        if (methodDefinition.Signature is not null)
            ReturnType = methodDefinition.Signature.ReturnType;

        var cilMethodBody = new CilMethodBody(methodDefinition);

        _variables = new List<CilLocalVariable>();
        foreach (var variable in function.Variables) 
            _variables.Add(new CilLocalVariable(variable.Signature));

        _ilInstructions = new IlInstructionCollection { new(new CilInstruction(CilOpCodes.Ldnull)) };

        foreach (var block in function.Blocks)
        foreach (var instruction in block) {
            if (!_transforms.TryGetValue(instruction.Code, out var result))
                throw new VmpRecompilerException($"Unable to resolve transforms for: {instruction.Code}");

            result.Transform(this, instruction);
        }

        foreach (var variable in _variables) 
            cilMethodBody.LocalVariables.Add(variable);

        var instructions = ProcessInstructions();
        foreach (var instruction in instructions) 
            cilMethodBody.Instructions.Add(instruction);
        
        var handlers = ComputeHandlers(function.Handlers);
        foreach (var handler in handlers)
            cilMethodBody.ExceptionHandlers.Add(handler);

        cilMethodBody.Instructions.CalculateOffsets();
        cilMethodBody.Instructions.OptimizeMacros();

        try {
            cilMethodBody.VerifyLabels();
            cilMethodBody.MaxStack = cilMethodBody.ComputeMaxStack();
        }
        catch (Exception) {
            logger.Warning(this, $"Failed to compute MaxStack for: {methodDefinition.Name}");
        }

        return cilMethodBody;
    }

    private IEnumerable<CilInstruction> ProcessInstructions() {
        foreach (var ilInstruction in _ilInstructions) {
            var instruction = ilInstruction.Instruction;

            if (instruction.OpCode.OperandType is CilOperandType.InlineBrTarget or CilOperandType.InlineSwitch) {
                if (ilInstruction.Reference is null)
                    throw new VmpRecompilerException($"{instruction} does not have a reference.");
                    
                ilInstruction.Reference.Apply(instruction, _ilInstructions);
            }

            yield return instruction;
        }
    }
    
    private IEnumerable<CilExceptionHandler> ComputeHandlers(IList<VmpExceptionHandlerBase> handlers) {
        return handlers.Select(handler => handler.Type switch {
            VmpExceptionHandlerType.Catch => BuildCatchHandler((VmpCatchExceptionHandler)handler),
            VmpExceptionHandlerType.Filter => BuildFilterHandler((VmpFilterExceptionHandler)handler),
            VmpExceptionHandlerType.Finally => BuildFinallyHandler(handler),
            VmpExceptionHandlerType.Fault => BuildFaultHandler(handler),
            _ => throw new ArgumentOutOfRangeException(nameof(handlers))
        });
    }
    
    private CilExceptionHandler BuildCatchHandler(VmpCatchExceptionHandler handler) {
        return new CilExceptionHandler {
            HandlerType = CilExceptionHandlerType.Exception,
            ExceptionType = handler.CatchType.ToTypeDefOrRef(),
            HandlerStart = _ilInstructions.GetLabelByAddress(handler.HandlerStart),
            HandlerEnd = _ilInstructions.GetLabelAfterInstruction(handler.HandlerEnd),
            TryStart = _ilInstructions.GetLabelByAddress(handler.TryStart),
            TryEnd = _ilInstructions.GetLabelAfterInstruction(handler.TryEnd)
        };
    }
    
    private CilExceptionHandler BuildFilterHandler(VmpFilterExceptionHandler handler) {
        return new CilExceptionHandler {
            HandlerType = CilExceptionHandlerType.Filter,
            FilterStart = _ilInstructions.GetLabelByAddress(handler.FilterStart),
            HandlerStart = _ilInstructions.GetLabelByAddress(handler.HandlerStart),
            HandlerEnd = _ilInstructions.GetLabelAfterInstruction(handler.HandlerEnd),
            TryStart = _ilInstructions.GetLabelByAddress(handler.TryStart),
            TryEnd = _ilInstructions.GetLabelAfterInstruction(handler.TryEnd)
        };
    }
    
    private CilExceptionHandler BuildFinallyHandler(VmpExceptionHandlerBase handler) {
        return new CilExceptionHandler {
            HandlerType = CilExceptionHandlerType.Finally,
            HandlerStart = _ilInstructions.GetLabelByAddress(handler.HandlerStart),
            HandlerEnd = _ilInstructions.GetLabelAfterInstruction(handler.HandlerEnd),
            TryStart = _ilInstructions.GetLabelByAddress(handler.TryStart),
            TryEnd = _ilInstructions.GetLabelAfterInstruction(handler.TryEnd)
        };
    }
    
    private CilExceptionHandler BuildFaultHandler(VmpExceptionHandlerBase handler) {
        return new CilExceptionHandler {
            HandlerType = CilExceptionHandlerType.Fault,
            HandlerStart = _ilInstructions.GetLabelByAddress(handler.HandlerStart),
            HandlerEnd = _ilInstructions.GetLabelAfterInstruction(handler.HandlerEnd),
            TryStart = _ilInstructions.GetLabelByAddress(handler.TryStart),
            TryEnd = _ilInstructions.GetLabelAfterInstruction(handler.TryEnd)
        };
    }

    public CilLocalVariable GetVariable(short index) {
        return _variables[index];
    }

    public void AddInstruction(CilInstruction instruction) {
        _ilInstructions.Add(new IlInstruction(instruction));
    }

    public void AddInstruction(uint address, CilInstruction instruction) {
        _ilInstructions.Add(new IlInstruction(instruction, address));
    }

    public void AddInstruction(CilInstruction instruction, IReference reference) {
        _ilInstructions.Add(new IlInstruction(instruction) {
            Reference = reference
        });
    }

    public void AddInstruction(uint address, CilInstruction instruction, IReference reference) {
        _ilInstructions.Add(new IlInstruction(instruction, address) {
            Reference = reference
        });
    }

    public void InsertInstruction(int index, CilInstruction instruction) {
        _ilInstructions.Insert(_ilInstructions.Count - index, new IlInstruction(instruction));
    }
}