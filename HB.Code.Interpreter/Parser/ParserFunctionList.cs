namespace HB.Code.Interpreter.Parser;
public class ParserFunctionList {
    private readonly Dictionary<string, IParserFunction> parserFunctions = [];
    public IParserFunction<T>? Get<T>() {
        if (parserFunctions.TryGetValue(typeof(T).FullName!, out IParserFunction? value))
            return value as IParserFunction<T>;

        return null;
    }

    public bool TryGet<T>(out IParserFunction<T>? parserFunction) {
        bool hasValue = parserFunctions.TryGetValue(typeof(T).FullName!, out IParserFunction? temp);
        parserFunction = temp as IParserFunction<T>;
        return hasValue && parserFunction != null;
    }

    public void AddOrUpdate<T>(IParserFunction<T> parserFunction) {
        parserFunctions[typeof(T).FullName!] = parserFunction;
    }
}
