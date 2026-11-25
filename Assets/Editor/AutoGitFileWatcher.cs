
using System.IO;
using System.Threading;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class AutoGitFileWatcher
{
    private static readonly string ScriptsPath =
        Path.Combine(Application.dataPath, "Scripts");

    private static FileSystemWatcher _watcher;
    private static int _hasChanges;

    static AutoGitFileWatcher()
    {
        InitializeWatcher();

        EditorApplication.update += ProcessChanges;
        AssemblyReloadEvents.beforeAssemblyReload += DisposeWatcher;
        EditorApplication.quitting += DisposeWatcher;
    }

    private static void InitializeWatcher()
    {
        if (Directory.Exists(ScriptsPath) == false)
        {
            Debug.LogError($"AutoGitFileWatcher: folder not found: {ScriptsPath}");
            return;
        }

        _watcher = new FileSystemWatcher
        {
            Path = ScriptsPath,
            Filter = "*.cs",
            IncludeSubdirectories = true,
            NotifyFilter = NotifyFilters.LastWrite |
                           NotifyFilters.FileName |
                           NotifyFilters.Size,
            EnableRaisingEvents = true
        };

        _watcher.Changed += OnFileChanged;
        _watcher.Created += OnFileChanged;
        _watcher.Deleted += OnFileChanged;
        _watcher.Renamed += OnFileRenamed;

        Debug.Log("AutoGitFileWatcher: monitoring started.");
        _watcher.Error += OnWatcherError;
    }

    private static void OnWatcherError(object sender, ErrorEventArgs args)
    {
        Debug.LogError($"FileWatcher error: {args.GetException()}");
    }

    private static void OnFileChanged(object sender, FileSystemEventArgs args)
    {
        Debug.Log($"FileWatcher: {args.ChangeType}: {args.FullPath}");
        Interlocked.Exchange(ref _hasChanges, 1);
    }

    private static void OnFileRenamed(object sender, RenamedEventArgs args)
    {
        Debug.Log($"FileWatcher: Renamed: {args.FullPath}");
        Interlocked.Exchange(ref _hasChanges, 1);
    }

    private static void ProcessChanges()
    {
        if (Interlocked.Exchange(ref _hasChanges, 0) == 0)
            return;

        Debug.Log("AutoGitFileWatcher: script change detected.");

        AutoGitCommit.RequestSync();
    }

    private static void DisposeWatcher()
    {
        EditorApplication.update -= ProcessChanges;
        AssemblyReloadEvents.beforeAssemblyReload -= DisposeWatcher;
        EditorApplication.quitting -= DisposeWatcher;

        if (_watcher == null)
            return;

        _watcher.EnableRaisingEvents = false;
        _watcher.Changed -= OnFileChanged;
        _watcher.Created -= OnFileChanged;
        _watcher.Deleted -= OnFileChanged;
        _watcher.Renamed -= OnFileRenamed;
        _watcher.Dispose();
        _watcher = null;
    }
}