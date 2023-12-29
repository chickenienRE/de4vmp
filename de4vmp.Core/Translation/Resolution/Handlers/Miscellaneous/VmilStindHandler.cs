using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Resolution.Handlers.Miscellaneous;

public class VmilStindHandler : IndirectHandlerBase {
    public override VmpCode Translates => VmpCode.VmilStindCode;

    protected override bool HasPush => false;
}