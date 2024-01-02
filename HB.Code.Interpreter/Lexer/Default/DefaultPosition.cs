using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace HB.Code.Interpreter.Lexer.Default;
public class DefaultPosition : IPosition {
    public DefaultPosition? Parent { get; private set; }
    public int Index { get; set; }
    public int LineIndex { get; set; }
    public int Line { get; private set; }

    private DefaultPosition() { }

    public static DefaultPosition Init() => new DefaultPosition { Index = -1, Line = 1, LineIndex = -1, Parent = null };
    public static DefaultPosition Create(DefaultPosition current, int index, int line, int lineIndex)
        => new DefaultPosition { Parent = current, Index = index, Line = line, LineIndex = lineIndex };

    public static DefaultPosition Create(DefaultPosition current)
        => new DefaultPosition { Parent = current, Index = current.Index, Line = current.Line, LineIndex = current.LineIndex };

    public char GetValue(string content) {
        if (Index == -1 || Index >= content.Length)
            return CommonCharCollection.NULL;

        return content[Index];
    }

    public TextSpan GetSpanToParent() => new TextSpan(Parent?.Index ?? Index, Index - (Parent?.Index ?? Index));

    public LineSpan GetLineSpanToParent() {
        int start;
        int length;
        if (Parent == null) {
            start = 0;
            length = LineIndex;
        }
        else if(Parent.Line < Line) {
            start = 0;
            length = LineIndex;
        }
        else {
            start = Parent.LineIndex;
            length = LineIndex - Parent.LineIndex;
        }

        return new LineSpan(Line, start, length);
    }

    public string GetStringToParent(string content) {
        StringBuilder sb = new StringBuilder();
        for (int i = Parent!.Index; i < Index; i++)
            sb.Append(content[i]);

        return sb.ToString();
    }
}
