namespace de4vmp.Core.Architecture.ExceptionHandlers;

public enum VmpExceptionHandlerType : byte {
    Catch = 0,
    Filter = 1,
    Finally = 2,
    Fault = 4
}