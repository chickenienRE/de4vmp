namespace de4vmp.Core.Architecture.ExceptionHandlers;

public class VmpExceptionHandlerScope {
    public VmpExceptionHandlerScope(int scopeStart, int scopeEnd) {
        ScopeStart = scopeStart;
        ScopeEnd = scopeEnd;
    }

    public IList<VmpExceptionHandlerBase> Handlers { get; } = new List<VmpExceptionHandlerBase>();

    public int ScopeStart { get; }

    public int ScopeEnd { get; }

    public override string ToString() {
        return $"ScopeStart: {ScopeStart}, ScopeEnd: {ScopeEnd}, Handlers: {Handlers.Count}";
    }
}