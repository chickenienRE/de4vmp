using AsmResolver.PE.DotNet.Cil;
using de4vmp.Core.Services;

namespace de4vmp.Core.Architecture.Annotations.Comparision;

public class VmpCmp : IAnnotation {
    private readonly int _type;
    private readonly bool _unsigned;

    public VmpCmp(int type, bool unsigned) {
        _type = type;
        _unsigned = unsigned;
    }

    public CilOpCode OpCode {
        get {
            switch (_type) {
                case 0:
                    if (_unsigned)
                        return CilOpCodes.Bne_Un;

                    throw ExceptionService.ComparerInvalidException(_type, Flags, _unsigned);
                case 1:
                    if ((Flags & VmpCmpFlags.ShrUnFlag) != VmpCmpFlags.ShrUnFlag)
                        return CilOpCodes.Ceq;

                    if ((Flags & VmpCmpFlags.AddFlag) == VmpCmpFlags.AddFlag)
                        return _unsigned ? CilOpCodes.Ble_Un : CilOpCodes.Ble;

                    return _unsigned ? CilOpCodes.Clt_Un : CilOpCodes.Clt;
                case -1:
                    if ((Flags & VmpCmpFlags.ShrUnFlag) != VmpCmpFlags.ShrUnFlag)
                        throw ExceptionService.ComparerInvalidException(_type, Flags, _unsigned);

                    if ((Flags & VmpCmpFlags.AddFlag) == VmpCmpFlags.AddFlag)
                        return _unsigned ? CilOpCodes.Cgt_Un : CilOpCodes.Cgt;

                    return _unsigned ? CilOpCodes.Bge_Un : CilOpCodes.Bge;

                default:
                    throw ExceptionService.ComparerInvalidException(_type, Flags, _unsigned);
            }
        }
    }

    public VmpCmpFlags Flags { get; set; }

    /*
        Type=1, UnSigned=false | beq -> and
        Type=0, UnSigned=true | bne_un -> and

        Type=1, UnSigned=false | ble -> add, shrUn, and
        Type=-1, UnSigned=false | bge -> ShrUn, and

        Type=1, UnSigned=false | blt -> shrUn, and
        Type=-1, UnSigned=false | bgt -> add, shrUn, and
    */

    public override string ToString() {
        return $"Cmp: {OpCode}, Flags: {Flags}";
    }
}