using de4vmp.Core.Architecture;

namespace de4vmp.Core.Translation.Transformation; 

public interface ITransform {
    IEnumerable<VmpCode> Accepts { get; }
    void Transform(VmpRecompiler recompiler, VmpInstruction instruction);
}