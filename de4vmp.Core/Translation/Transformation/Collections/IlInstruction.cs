using AsmResolver.PE.DotNet.Cil;

namespace de4vmp.Core.Translation.Transformation.Collections;

public class IlInstruction {
    public IlInstruction(CilInstruction instruction, uint rva = 0) {
        Instruction = instruction;
        Rva = rva;
    }

    public CilInstruction Instruction { get; }
    public uint Rva { get; }

    public IReference? Reference { get; init; }

    public override string ToString() {
        string result = $"[{Instruction.OpCode}]";

        object? operand = Instruction.Operand;
        if (operand is not null)
            result += $" | [{operand}]";

        object? annotation = Reference;
        if (annotation is not null)
            result += $" - [{annotation}]";

        return result;
    }
}