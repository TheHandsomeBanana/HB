using HB.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Common.IO;
public readonly struct PathIndex {
    public string Value { get; }
    public bool IsDirectory { get; }
    public bool IsFile { get; }

    public PathIndex(string value) {
        if (!value.IsValidPath())
            throw new ArgumentException("The given value does not represent a file or directory", nameof(value));

        IsFile = File.Exists(value);
        IsDirectory = Directory.Exists(value);
        this.Value = value;
    }

    public FileSystemInfo GetInfo() => IsFile ? new FileInfo(Value) : new DirectoryInfo(Value);
    public FileInfo? GetFileInfo() => IsFile ? new FileInfo(Value) : null;
    public DirectoryInfo? GetDirectoryInfo() => IsDirectory ? new DirectoryInfo(Value) : null;

}
