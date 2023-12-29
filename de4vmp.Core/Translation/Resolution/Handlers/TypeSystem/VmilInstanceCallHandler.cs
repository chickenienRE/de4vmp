using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.TypeSystem;

public class VmilInstanceCallHandler : VirtualCallHandlerBase {
    public override VmpCode Translates => VmpCode.VmilInstanceCallCode;

    public override bool IsStatic => false;
}