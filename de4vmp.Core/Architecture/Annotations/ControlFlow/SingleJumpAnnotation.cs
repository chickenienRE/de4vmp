namespace de4vmp.Core.Architecture.Annotations.ControlFlow;

public class SingleJumpAnnotation : IAnnotation {
    public SingleJumpAnnotation(uint address) {
        if (address == 0)
            WillReturn = true;
        else
            Address = address;
    }

    public uint Address { get; }
    public bool WillReturn { get; }

    public override string ToString() {
        return WillReturn ? "return" : $"jump_{Address}";
    }
}