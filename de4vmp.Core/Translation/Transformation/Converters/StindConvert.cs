using AsmResolver.PE.DotNet.Cil;

namespace de4vmp.Core.Translation.Transformation.Converters;

public class StindConvert : IConvert {
    private readonly IDictionary<string, CilOpCode> _conversions = new Dictionary<string, CilOpCode> {
        { nameof(IntPtr), CilOpCodes.Stind_I },
        { nameof(SByte), CilOpCodes.Stind_I1 },
        { nameof(Int16), CilOpCodes.Stind_I2 },
        { nameof(Int32), CilOpCodes.Stind_I4 },
        { nameof(Int64), CilOpCodes.Stind_I8 },
        { nameof(Single), CilOpCodes.Stind_R4 },
        { nameof(Double), CilOpCodes.Stind_R8 },
        { nameof(Object), CilOpCodes.Stind_Ref }
    };

    public CilOpCode Resolve(string name) {
        return _conversions[name];
    }

    public bool TryResolve(string name, out CilOpCode opCode) {
        return _conversions.TryGetValue(name, out opCode);
    }
}