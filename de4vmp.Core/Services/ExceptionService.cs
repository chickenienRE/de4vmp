using de4vmp.Core.Architecture.Annotations.Comparision;
using de4vmp.Core.Translation;
using de4vmp.Core.Translation.Transformation;

namespace de4vmp.Core.Services; 

public static class ExceptionService {
    public static DevirtualizationException ThrowInferredException<TItem>(TItem item, string type) {
        return new DevirtualizationException($"{type}_{item} has already been inferred!");
    }
    
    public static DevirtualizationException ComparerInvalidException(int type, VmpCmpFlags flags, bool unsigned) {
        throw new DevirtualizationException($"Invalid cmp! type: {type}, Flags: {flags}, Unsigned: {unsigned}");
    }
    
    public static VmpTranslatorException ComparerNullException() {
        return new VmpTranslatorException("Comparision is null in comparision state.");
    }
    
    public static VmpTranslatorException UnknownConverterException<TConverter>() {
        return new VmpTranslatorException($"UnSupported converter: {nameof(TConverter)}");
    }

    public static VmpRecompilerException ThrowInvalidAnnotation<TInstruction>(TInstruction instruction) {
        return new VmpRecompilerException($"UnSupported annotation: {instruction}");
    }
    
    public static VmpRecompilerException ThrowInvalidOperand<TInstruction, TExpected>(TInstruction instruction) {
        return new VmpRecompilerException($"UnSupported operand: {instruction}, Expected: {typeof(TExpected)}");
    }
}