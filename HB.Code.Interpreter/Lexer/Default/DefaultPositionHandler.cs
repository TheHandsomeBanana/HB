using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Code.Interpreter.Lexer.Default;
public class DefaultPositionHandler : IPositionHandler<DefaultPosition> {
    public string Content { get; private set; } = string.Empty;
    public DefaultPosition CurrentPosition { get; private set; } = DefaultPosition.Init();

    public void Init(string content) {
        Content = content;
        CurrentPosition = DefaultPosition.Init();
    }

    public void MoveNext(int steps) {
        DefaultPosition old = CurrentPosition;
        CurrentPosition = DefaultPosition.Create(old, CurrentPosition.Index + steps, CurrentPosition.Line, CurrentPosition.LineIndex + steps);
    }

    public void MoveNextWhile(int steps, Predicate<DefaultPosition> predicate) {
        DefaultPosition old = CurrentPosition;
        CurrentPosition = DefaultPosition.Create(old);

        while (predicate.Invoke(CurrentPosition)) {
            if (CurrentPosition.GetValue(Content) == CommonCharCollection.NULL)
                return;

            CurrentPosition.Index += steps;
            CurrentPosition.LineIndex += steps;
        }
    }

    public void MoveNextDoWhile(int steps, Predicate<DefaultPosition> predicate) {
        DefaultPosition old = CurrentPosition;
        CurrentPosition = DefaultPosition.Create(old);

        do {
            if (CurrentPosition.GetValue(Content) == CommonCharCollection.NULL)
                return;

            CurrentPosition.Index += steps;
            CurrentPosition.LineIndex += steps;
        } while (predicate.Invoke(CurrentPosition));
    }

    public void Reset() {
        CurrentPosition = DefaultPosition.Init();
    }

    public void Skip(int steps) {
        CurrentPosition.Index += steps;
        CurrentPosition.LineIndex += steps;
    }

    public void NewLine() {
        DefaultPosition old = CurrentPosition;
        CurrentPosition = DefaultPosition.Create(old, old.Index + 2, old.Line + 1, -1);
    }
}
