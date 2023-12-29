using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.Miscellaneous;

public class VmilLdindHandler : IndirectHandlerBase {
    public override VmpCode Translates => VmpCode.VmilLdindCode;

    protected override bool HasPush => true;
}