using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.IO;
using AsmResolver.PE.DotNet.Cil;
using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.Conversion;

public class VmilConvHandler : HandlerBase {
    private readonly IEnumerable<CilCode> _signature = new List<CilCode> {
        CilCode.Ldarg,
        CilCode.Ldarg,
        CilCode.Call,
        CilCode.Callvirt,
        CilCode.Call,
        CilCode.Stloc,
        CilCode.Ldarg,
        CilCode.Ldarg,
        CilCode.Ldarg,
        CilCode.Call,
        CilCode.Ldloc,
        CilCode.Call,
        CilCode.Call,
        CilCode.Ret
    };

    public override VmpCode Translates => VmpCode.VmilConvCode;

    public override bool Resolve(CilInstructionCollection instructions) {
        return instructions.AreSignatureEqual(_signature);
    }

    public override VmpTranslatorState Translate(VmpTranslator translator, ref BinaryStreamReader reader,
        VmpInstruction instruction, VmpTranslatorContext context) {
        var typeDescriptor = translator.ResolveMember<ITypeDescriptor>(context.DataStack.Pop());
        if (typeDescriptor.FullName.Equals(typeof(bool).FullName)) context.State = VmpState.DoubleJumpState;

        instruction.Operand = typeDescriptor;
        return VmpTranslatorState.Next;
    }
}