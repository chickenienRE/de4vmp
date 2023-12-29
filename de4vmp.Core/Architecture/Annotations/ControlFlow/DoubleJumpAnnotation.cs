using de4vmp.Core.Architecture.Annotations.Comparision;

namespace de4vmp.Core.Architecture.Annotations.ControlFlow;

public class DoubleJumpAnnotation : IAnnotation {
    public DoubleJumpAnnotation(uint firstAddress, uint secondAddress) {
        FirstAddress = firstAddress;
        SecondAddress = secondAddress;
    }

    public uint FirstAddress { get; }
    public uint SecondAddress { get; }

    public bool Type { get; set; }
    public VmpCmp? Comparision { get; set; }

    public override string ToString() {
        return $"{(Type ? "brtrue" : "brfalse")}.{FirstAddress}_{SecondAddress}";
    }
}