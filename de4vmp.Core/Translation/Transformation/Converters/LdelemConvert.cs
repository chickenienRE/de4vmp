using AsmResolver.PE.DotNet.Cil;

namespace de4vmp.Core.Translation.Transformation.Converters;

public class LdelemConvert : IConvert {
    private readonly IDictionary<string, CilOpCode> _conversions = new Dictionary<string, CilOpCode> {
        { nameof(IntPtr), CilOpCodes.Ldelem_I },
        { nameof(SByte), CilOpCodes.Ldelem_I1 },
        { nameof(Int16), CilOpCodes.Ldelem_I2 },
        { nameof(Int32), CilOpCodes.Ldelem_I4 },
        { nameof(Int64), CilOpCodes.Ldelem_I8 },
        { nameof(Byte), CilOpCodes.Ldelem_U1 },
        { nameof(UInt16), CilOpCodes.Ldelem_U2 },
        { nameof(UInt32), CilOpCodes.Ldelem_U4 },
        { nameof(UInt64), CilOpCodes.Ldelem_I8 },
        { nameof(Single), CilOpCodes.Ldelem_R4 },
        { nameof(Double), CilOpCodes.Ldelem_R8 },
        { nameof(Object), CilOpCodes.Ldelem_Ref }
    };

    public CilOpCode Resolve(string name) {
        return _conversions[name];
    }

    public bool TryResolve(string name, out CilOpCode opCode) {
        return _conversions.TryGetValue(name, out opCode);
    }
}