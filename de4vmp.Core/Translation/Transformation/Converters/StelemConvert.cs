using AsmResolver.PE.DotNet.Cil;

namespace de4vmp.Core.Translation.Transformation.Converters;

public class StelemConvert : IConvert {
    private readonly IDictionary<string, CilOpCode> _conversions = new Dictionary<string, CilOpCode> {
        { nameof(IntPtr), CilOpCodes.Stelem_I },
        { nameof(SByte), CilOpCodes.Stelem_I1 },
        { nameof(Int16), CilOpCodes.Stelem_I2 },
        { nameof(Int32), CilOpCodes.Stelem_I4 },
        { nameof(Int64), CilOpCodes.Stelem_I8 },
        { nameof(Single), CilOpCodes.Stelem_R4 },
        { nameof(Double), CilOpCodes.Stelem_R8 },
        { nameof(Object), CilOpCodes.Stelem_Ref }
    };

    public CilOpCode Resolve(string name) {
        return _conversions[name];
    }

    public bool TryResolve(string name, out CilOpCode opCode) {
        return _conversions.TryGetValue(name, out opCode);
    }
}