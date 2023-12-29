using System.Collections;
using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.IO;
using de4vmp.Core.Architecture;
using de4vmp.Core.Architecture.Annotations.Variables;
using de4vmp.Core.Architecture.Annotations.Variables.Variants;
using de4vmp.Core.Translation.Emulation;
using de4vmp.Core.Translation.Emulation.Values;

namespace de4vmp.Core.Translation.Resolution.Handlers.Variables;

public class VmilInitVarHandler : HandlerBase {
    public override VmpCode Translates => VmpCode.VmilInitVarCode;

    public override bool Resolve(CilInstructionCollection instructions) {
        return instructions.HasMethodReference(nameof(IList.Add)) &&
               !instructions.HasMethodReference(nameof(IList.Insert)) &&
               !instructions.HasMethodReference(nameof(IntPtr.ToPointer));
    }

    public override VmpTranslatorState Translate(VmpTranslator translator, ref BinaryStreamReader reader,
        VmpInstruction instruction, VmpTranslatorContext context) {
        var typeDescriptor = translator.ResolveMember<ITypeDescriptor>(context.DataStack.Pop());
        IVariable variable = context.ValueStack.TryPop<ParameterValue, IValue>(out var result)
            ? new ArgumentTypeLocal(result.Parameter)
            : new VariableTypeLocal(typeDescriptor);

        translator.Function.Variables.Add(variable);
        return VmpTranslatorState.Next;
    }
}