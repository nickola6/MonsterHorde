using System.IO;
using System.Threading;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class AutoGitFileWatcher
{
    private static readonly string ScriptsPath = Path.Combine(Application.dataPath, "Scripts");

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
            return;

        _watcher = new FileSystemWatcher
        {
            Path = ScriptsPath,
            Filter = "*.cs",
            IncludeSubdirectories = true,
            NotifyFilter = NotifyFilters.LastWrite |
                           NotifyFilters.FileName |
                           NotifyFilters.Size,
            EnableRaisingEvents = false
        };

        _watcher.Changed += OnFileChanged;
        _watcher.Created += OnFileChanged;
        _watcher.Deleted += OnFileChanged;
        _watcher.Renamed += OnFileRenamed;

        _watcher.EnableRaisingEvents = true;
    }

    private static void OnFileChanged(object sender, FileSystemEventArgs args)
    {
        Interlocked.Exchange(ref _hasChanges, 1);
    }

    private static void OnFileRenamed(object sender, RenamedEventArgs args)
    {
        Interlocked.Exchange(ref _hasChanges, 1);
    }

    private static void ProcessChanges()
    {
        if (Interlocked.Exchange(ref _hasChanges, 0) == 0)
            return;

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