using System;

namespace HB.NETF.Common.Threading {
    public static class ThreadLocker {
        private static object staticLock = new object();
        public static void LockAction(Action a) {
            if (a is null) throw new ArgumentNullException(nameof(a));

            lock (staticLock) {
                a();
            }
        }

        public static void LockAction(Action a, object lockObject) {
            if (a is null) throw new ArgumentNullException(nameof(a));
            if (lockObject is null) throw new ArgumentNullException(nameof(lockObject));

            lock (lockObject) {
                a();
            }
        }

        public static T LockFunc<T>(Func<T> func) {
            if (func is null) throw new ArgumentNullException(nameof(func));

            lock (staticLock) {
                return func();
            }
        }

        public static T LockFunc<T>(Func<T> func, object lockObject) {
            if (func is null) throw new ArgumentNullException(nameof(func));
            if (lockObject is null) throw new ArgumentNullException(nameof(lockObject));

            lock (lockObject) {
                return func();
            }
        }
    }
}
