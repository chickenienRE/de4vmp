namespace de4vmp.Core.Architecture; 

public class VmpInstruction {
    public VmpInstruction(uint address, VmpCode code) {
        Address = address;
        Code = code;
    }
    
    public uint Address { get; }

    public VmpCode Code { get; }

    public object? Operand { get; set; }
    
    public IAnnotation? Annotation { get; set; }

    public override string ToString() {
        string result = $"[{Address}] # {Code}";

        if (Operand is not null)
            result += $" : [{Operand}]";
        
        if (Annotation is not null)
            result += $" | [{Annotation}]";

        return result;
    }
}