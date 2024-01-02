using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Common.Extensions;
public static class StringExtensions {
    public static bool IsValidPath(this string path)
        => !string.IsNullOrEmpty(path) 
        && path.IndexOfAny(Path.GetInvalidPathChars()) == -1
        && Path.IsPathFullyQualified(path);
}
