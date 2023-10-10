using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HB.Common.Threading {
    public static class Locker {
        public static object StaticLock { get; } = new object();
        public static void RunLocked(Action a, object lockObject) {
            lock(lockObject) {
                a();
            }
        }
        public static void RunLocked(Action a) => RunLocked(a, StaticLock);

        public static T RunLocked<T>(Func<T> func, object lockObject) {
            lock (lockObject) {
                return func();
            }
        }
        public static T RunLocked<T>(Func<T> func) => RunLocked(func, StaticLock);
        
        public static SemaphoreSlim StaticSemaphore { get; } = new SemaphoreSlim(1);
        public static async Task RunLockedAsync(Action a, SemaphoreSlim semaphore) {
            await semaphore.WaitAsync();

            try {
                await Task.Run(a);
            }
            finally {
                semaphore.Release();
            }
        }

        public static async Task RunLockedAsync(Action a) => await RunLockedAsync(a, StaticSemaphore);

        public static async Task<T> RunLockedAsync<T>(Func<T> func, SemaphoreSlim semaphore) {
            await semaphore.WaitAsync();

            try {
                return await Task.Run(func);
            }
            finally {
                semaphore.Release();
            }
        }

        public static async Task<T> RunLockedAsync<T>(Func<T> func) => await RunLockedAsync(func, StaticSemaphore);
    }
}
