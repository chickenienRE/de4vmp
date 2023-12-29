using AsmResolver.PE.DotNet.Cil;

namespace de4vmp.Core.Translation.Transformation.Converters;

public class LdindConvert : IConvert {
    private readonly IDictionary<string, CilOpCode> _conversions = new Dictionary<string, CilOpCode> {
        { nameof(IntPtr), CilOpCodes.Ldind_I },
        { nameof(SByte), CilOpCodes.Ldind_I1 },
        { nameof(Int16), CilOpCodes.Ldind_I2 },
        { nameof(Int32), CilOpCodes.Ldind_I4 },
        { nameof(Int64), CilOpCodes.Ldind_I8 },
        { nameof(Byte), CilOpCodes.Ldind_U1 },
        { nameof(UInt16), CilOpCodes.Ldind_U2 },
        { nameof(UInt32), CilOpCodes.Ldind_U4 },
        { nameof(UInt64), CilOpCodes.Ldind_I8 },
        { nameof(Single), CilOpCodes.Ldind_R4 },
        { nameof(Double), CilOpCodes.Ldind_R8 },
        { nameof(Object), CilOpCodes.Ldind_Ref }
    };

    public CilOpCode Resolve(string name) {
        return _conversions[name];
    }

    public bool TryResolve(string name, out CilOpCode opCode) {
        return _conversions.TryGetValue(name, out opCode);
    }
}