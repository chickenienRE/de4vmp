using de4vmp.Core.Architecture.Annotations.Comparision;
using de4vmp.Core.Architecture.ExceptionHandlers;
using de4vmp.Core.Translation.Emulation;

namespace de4vmp.Core.Translation; 

public class VmpTranslatorContext {
    public IList<VmpExceptionHandlerScope> Scopes { get; } = new List<VmpExceptionHandlerScope>();
    public VmpState State { get; set; } = VmpState.NormalState;

    public Stack<VmpFlags> FlagsStack { get; } = new();
    public Stack<IValue> ValueStack { get; } = new();
    public Stack<VmpCmp> CmpStack { get; } = new();
    public Stack<int> DataStack { get; } = new();
}