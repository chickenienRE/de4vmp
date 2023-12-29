namespace de4vmp.Core.Architecture.Annotations.ControlFlow;

public class MultipleJumpAnnotation : IAnnotation {
    public MultipleJumpAnnotation(IList<uint> addresses) {
        Addresses = addresses;
    }

    public IList<uint> Addresses { get; }

    public override string ToString() {
        return Addresses.Aggregate($"{Addresses.Count}|", (current, address) => current + $"_{address}");
    }
}