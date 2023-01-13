using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Common.Enumerators {
    public static class EnumeratorExtensions {
        public static CustomIntEnumerator GetEnumerator(this Range range) => new CustomIntEnumerator(range);
        public static CustomIntEnumerator GetEnumerator(this int number) => new CustomIntEnumerator(new Range(0, number));
    }
}
