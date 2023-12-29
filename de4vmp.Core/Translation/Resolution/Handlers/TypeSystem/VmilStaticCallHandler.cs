using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.TypeSystem;

public class VmilStaticCallHandler : VirtualCallHandlerBase {
    public override VmpCode Translates => VmpCode.VmilStaticCallCode;

    public override bool IsStatic => true;
}