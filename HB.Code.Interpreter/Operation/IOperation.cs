using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Code.Interpreter.Operation;
public interface IOperation {
    public void Run();
}

public interface IOperation<T> {
    public T Run();
}
