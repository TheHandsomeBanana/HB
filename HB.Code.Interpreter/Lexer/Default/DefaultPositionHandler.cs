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
        CurrentPosition = DefaultPosition.Create(old, 
            GetPossibleIndex(steps, CurrentPosition.Index), 
            CurrentPosition.Line, 
            GetPossibleIndex(steps, CurrentPosition.LineIndex));
    }

    public void MoveNextWhile(int steps, Predicate<DefaultPosition> predicate) {
        DefaultPosition old = CurrentPosition;
        CurrentPosition = DefaultPosition.Create(old);

        while (predicate.Invoke(CurrentPosition)) {
            if (CurrentPosition.Index >= Content.Length) {
                CurrentPosition.Index = Content.Length - 1;
                return;
            }

            CurrentPosition.Index = GetPossibleIndex(steps, CurrentPosition.Index);
            CurrentPosition.LineIndex = GetPossibleIndex(steps, CurrentPosition.LineIndex);
        }
    }

    public void MoveNextDoWhile(int steps, Predicate<DefaultPosition> predicate) {
        DefaultPosition old = CurrentPosition;
        CurrentPosition = DefaultPosition.Create(old);

        do {
            if (CurrentPosition.Index >= Content.Length)
                return;

            CurrentPosition.Index = GetPossibleIndex(steps, CurrentPosition.Index);
            CurrentPosition.LineIndex = GetPossibleIndex(steps, CurrentPosition.LineIndex);
        } while (predicate.Invoke(CurrentPosition));
    }

    public void Reset() {
        CurrentPosition = DefaultPosition.Init();
    }

    public void Skip(int steps) {
        CurrentPosition.Index = GetPossibleIndex(steps, CurrentPosition.Index);
        CurrentPosition.LineIndex = GetPossibleIndex(steps, CurrentPosition.LineIndex);
    }

    public void NewLine() {
        DefaultPosition old = CurrentPosition;
        CurrentPosition = DefaultPosition.Create(old, old.Index + 2, old.Line + 1, 0);
    }

    private int GetPossibleIndex(int steps, int currentIndex) {
        int newIndex = currentIndex + steps;
        return newIndex >= Content.Length ? Content.Length : newIndex;
    }
}
