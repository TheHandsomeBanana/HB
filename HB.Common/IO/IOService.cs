using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Common.IO;
public static class IOService {
    #region Copy
    public static async Task Copy(PathIndex source, PathIndex target) {
        if (source.IsDirectory && target.IsFile)
            throw new IOException("Cannot copy a directory to a file.");

        if(source.IsFile && target.IsDirectory) 
            await CopyFile(source.Value, Path.Combine(target.Value, Path.GetFileName(source.Value)));
        else if(source.IsDirectory && target.IsDirectory) 
            await CopyDirectory(source.Value, target.Value);
    }

    public static async Task CopyDirectory(string source, string target) {
        Directory.CreateDirectory(target);

        List<Task> copyFileTasks = [];
        foreach(string file in Directory.GetFiles(source)) {
            string targetFile = Path.Combine(target, Path.GetFileName(file));
            copyFileTasks.Add(CopyFile(file, targetFile));
        }
        await Task.WhenAll(copyFileTasks);

        List<Task> copyDirectoryTasks = [];
        foreach(string directory in Directory.GetDirectories(source)) {
            string targetDirectory = Path.Combine(target, Path.GetFileName(directory));
            copyFileTasks.Add(CopyDirectory(directory, targetDirectory));
        }
        await Task.WhenAll(copyDirectoryTasks);
    }

    public static async Task CopyDirectory(DirectoryInfo source, DirectoryInfo target) {
        await CopyDirectory(source.FullName, target.FullName);
    }

    public static async Task CopyFile(string source, string target) {
        using (FileStream fs = File.OpenRead(source)) {
            await CopyFile(source, fs.Length, target);
        }
    }

    public static async Task CopyFile(FileInfo source, string target) {
        await CopyFile(source.FullName, source.Length, target);
    }

    public static async Task CopyFile(string source, long sourceLength, string target) {
        int bufferSize = GetOptimalBufferSize(sourceLength);
        using (FileStream sourceStream = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, true)) {
            using (FileStream destinationStream = new FileStream(target, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize, true)) {
                await sourceStream.CopyToAsync(destinationStream);
            }
        }
    }
    #endregion

    #region Move
    public static async Task Move(PathIndex source, PathIndex target) {
        if (source.IsDirectory && target.IsFile)
            throw new IOException("Cannot move a directory to a file.");

        if (source.IsFile && target.IsDirectory)
            await MoveFile(source.Value, Path.Combine(target.Value, Path.GetFileName(source.Value)));
        else if (source.IsDirectory && target.IsDirectory)
            await MoveDirectory(source.Value, target.Value);
    }

    public static async Task MoveDirectory(string source, string target) {
        Directory.CreateDirectory(target);

        List<Task> moveFileTasks = [];
        foreach (string file in Directory.GetFiles(source)) {
            string targetFile = Path.Combine(target, Path.GetFileName(file));
            moveFileTasks.Add(MoveFile(file, targetFile));
        }
        await Task.WhenAll(moveFileTasks);

        List<Task> moveDirectoryTasks = [];
        foreach (string directory in Directory.GetDirectories(source)) {
            string targetDirectory = Path.Combine(target, Path.GetFileName(directory));
            moveFileTasks.Add(MoveDirectory(directory, targetDirectory));
        }
        await Task.WhenAll(moveDirectoryTasks);
    }

    public static async Task MoveDirectory(DirectoryInfo source, DirectoryInfo target) {
        await MoveDirectory(source.FullName, target.FullName);
    }

    public static async Task MoveFile(string source, string target) {
        using (FileStream fs = File.OpenRead(source)) {
            await Task.Run(() => File.Move(source, target));
        }
    }

    public static async Task MoveFile(FileInfo source, string target) {
        await MoveFile(source.FullName, target);
    }
    #endregion

    #region Replace
    public static async Task Replace(PathIndex source, PathIndex target) {
        if (source.IsDirectory && target.IsFile)
            throw new IOException("Cannot replace a directory with a file.");

        if(source.IsFile && target.IsDirectory)
            throw new IOException("Cannot replace a file with a directory.");

        if (source.IsFile)
            await ReplaceFile(source.Value, Path.Combine(target.Value, Path.GetFileName(source.Value)));
        else if (source.IsDirectory)
            await ReplaceDirectory(source.Value, target.Value);
    }

    public static async Task ReplaceDirectory(string source, string target) {
        if (Directory.Exists(target))
            Directory.Delete(target, true);

        await MoveDirectory(source, target);
    }

    public static async Task ReplaceFile(string source, string target) {
        if (File.Exists(target))
            File.Delete(target);

        await MoveFile(source, target);
    }
    #endregion

    // https://github.com/dotnet/runtime/discussions/74405
    public static int GetOptimalBufferSize(long fileSize) {
        long fileSizeBytes = fileSize * 1024;

        if (fileSizeBytes <= 128 * 1024) // Up to 128 KB
            return 2048; // 2 KB
        else if (fileSizeBytes <= 512 * 1024) // Up to 512 KB
            return 65536; // 64 KB
        else if (fileSizeBytes <= 1 * 1024 * 1024) // Up to 1 MB
            return 81920; // ~80 KB
        else if (fileSizeBytes <= 10 * 1024 * 1024) // Up to 10 MB
            return 131072; // 128 KB
        else if (fileSizeBytes <= 32 * 1024 * 1024) // Up to 32 MB
            return 262144; // 256 KB
        else if (fileSizeBytes <= 100 * 1024 * 1024) // Up to 100 MB
            return 524288; // 512 KB
        else // Larger than 100 MB
            return 1048576; // 1 MB
    }
}
