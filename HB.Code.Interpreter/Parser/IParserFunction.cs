using System.Collections.Immutable;

namespace HB.Code.Interpreter.Parser;

public interface IParserFunction { // Flag
}

public interface IParserFunction<T> : IParserFunction {
    T Evaluate<TToken>(ImmutableArray<TToken> tokens);
}
