using System.Collections;
using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.IO;
using de4vmp.Core.Architecture;
using de4vmp.Core.Architecture.ExceptionHandlers;
using de4vmp.Core.Architecture.ExceptionHandlers.Variants;

namespace de4vmp.Core.Translation.Resolution.Handlers.ExceptionHandlers;

public class VmilInitEhHandler : HandlerBase {
    public override VmpCode Translates => VmpCode.VmilInitEhCode;

    public override bool Resolve(CilInstructionCollection instructions) {
        return instructions.HasMethodReference(nameof(IList.Insert));
    }

    public override VmpTranslatorState Translate(VmpTranslator translator, ref BinaryStreamReader reader,
        VmpInstruction instruction,
        VmpTranslatorContext context) {
        var handlerType = (VmpExceptionHandlerType) reader.ReadByte();
        int scopeStart = reader.ReadInt32();
        int scopeEnd = reader.ReadInt32();
        uint handlerStart = (uint) reader.ReadInt32();
        uint catchTypeOrFilterStart = (uint) reader.ReadInt32();
        
        var scope = context.Scopes.FirstOrDefault(handlerScope => handlerScope.ScopeStart == scopeStart && handlerScope.ScopeEnd == scopeEnd);
        if (scope == null) {
            scope = new VmpExceptionHandlerScope(scopeStart, scopeEnd);
            context.Scopes.Add(scope);
        }

        VmpExceptionHandlerBase result;

        switch (handlerType) {
            case VmpExceptionHandlerType.Catch:
                var typeDescriptor = translator.ResolveMember<ITypeDescriptor>(catchTypeOrFilterStart);
                if (typeDescriptor == null)
                    throw new VmpTranslatorException($"Couldn't resolve Catch block type, token: 0x{catchTypeOrFilterStart:X}");

                result = new VmpCatchExceptionHandler(handlerStart, typeDescriptor);
                break;
            case VmpExceptionHandlerType.Finally:
                result = new VmpFinallyExceptionHandler(handlerStart);
                break;
            case VmpExceptionHandlerType.Filter:
                result = new VmpFilterExceptionHandler(handlerStart, catchTypeOrFilterStart);
                break;
            case VmpExceptionHandlerType.Fault:
                result = new VmpFaultExceptionHandler(handlerStart);
                break;
            default:
                throw new ArgumentOutOfRangeException($"Couldn't resolve exception handler type, type: {handlerType}");
        }

        scope.Handlers.Add(result);
        return VmpTranslatorState.Next;
    }
}