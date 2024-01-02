using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Code.Interpreter.Operation;
public interface IAsyncOperation {
    public Task Run();
}

public interface IAsyncOperation<T> {
    public Task<T> Run(); 
}
