using System.Text;

namespace de4vmp.Core.Architecture.ExceptionHandlers;

public abstract class VmpExceptionHandlerBase {
    protected VmpExceptionHandlerBase(uint handlerStart) {
        HandlerStart = handlerStart;
    }

    public abstract VmpExceptionHandlerType Type { get; }

    public uint HandlerStart { get; }

    public uint HandlerEnd { get; set; }

    public uint TryStart { get; set; }

    public uint TryEnd { get; set; }

    public override string ToString() {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append($"EHType: {Type}");

        if (TryStart != 0)
            stringBuilder.Append($", TryStart: {TryStart}");

        if (TryEnd != 0)
            stringBuilder.Append($", TryEnd: {TryEnd}");

        stringBuilder.Append($", HandlerStart: {HandlerStart}");

        if (HandlerEnd != 0)
            stringBuilder.Append($", HandlerEnd: {HandlerEnd}");

        return stringBuilder.ToString();
    }
}