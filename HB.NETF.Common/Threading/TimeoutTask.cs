using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HB.NETF.Common.Threading {
    public static class TimeoutTask {
        public static void Run(Action a, TimeSpan timeout, Action onSuccess, Action onTimeout) {
            CancellationTokenSource cts = new CancellationTokenSource();
            Task t = Task.Run(a, cts.Token);
            if (t.Wait(timeout)) {
                onSuccess();
                return;
            }
            else {
                onTimeout();
                cts.Cancel(false);
            }
        }

        public static T Run<T>(Func<T> f, TimeSpan timeout, Action onSuccess, Action onTimeout) {
            CancellationTokenSource cts = new CancellationTokenSource();
            Task<T> t = Task.Run(f, cts.Token);
            if (t.Wait(timeout)) {
                onSuccess();
                return t.Result;
            }
            else {
                onTimeout();
                cts.Cancel(false);
                return default;
            }
        }
    }
}
