using AsmResolver.PE.DotNet.Cil;

namespace de4vmp.Core.Translation.Transformation.Converters;

public class ConvConvert : IConvert {
    private readonly IDictionary<string, CilOpCode> _conversions = new Dictionary<string, CilOpCode> {
        { nameof(IntPtr), CilOpCodes.Conv_I },
        { nameof(UIntPtr), CilOpCodes.Conv_U },
        { nameof(SByte), CilOpCodes.Conv_I1 },
        { nameof(Int16), CilOpCodes.Conv_I2 },
        { nameof(Int32), CilOpCodes.Conv_I4 },
        { nameof(Int64), CilOpCodes.Conv_I8 },
        { nameof(Byte), CilOpCodes.Conv_U1 },
        { nameof(UInt16), CilOpCodes.Conv_U2 },
        { nameof(UInt32), CilOpCodes.Conv_U4 },
        { nameof(UInt64), CilOpCodes.Conv_U8 },
        { nameof(Single), CilOpCodes.Conv_R4 },
        { nameof(Double), CilOpCodes.Conv_R8 }
    };

    public CilOpCode Resolve(string name) {
        return _conversions[name];
    }

    public bool TryResolve(string name, out CilOpCode opCode) {
        return _conversions.TryGetValue(name, out opCode);
    }
}