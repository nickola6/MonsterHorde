using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public class AutoGitCommit
{
    private const double Delay = 2d;

    private static readonly string ScriptPath =
        Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Tools", "SyncToGit.ps1");

    private static double _nextSyncTime;
    private static bool _isWaiting;
    private static bool _isRunning;

    static AutoGitCommit()
    {
        EditorSceneManager.sceneSaved += OnSceneSaved;
    }

    private static void OnSceneSaved(UnityEngine.SceneManagement.Scene scene)
    {
        _nextSyncTime = EditorApplication.timeSinceStartup + Delay;

        if (_isWaiting == false)
        {
            _isWaiting = true;
            EditorApplication.update += CheckSyncTime;
        }
    }

    private static void CheckSyncTime()
    {
        if (EditorApplication.timeSinceStartup < _nextSyncTime || _isRunning)
            return;

        _isWaiting = false;
        EditorApplication.update -= CheckSyncTime;

        if (File.Exists(ScriptPath) == false)
        {
            UnityEngine.Debug.LogError($"AutoGitCommit: script not found: {ScriptPath}");
            return;
        }

        _isRunning = true;
        UnityEngine.Debug.Log("AutoGitCommit: starting PowerShell process.");

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "powershell.exe",
            Arguments = $"-NoProfile -ExecutionPolicy Bypass -File \"{ScriptPath}\"",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        Process process = new Process
        {
            StartInfo = startInfo,
            EnableRaisingEvents = true
        };

        process.OutputDataReceived += OnOutputReceived;
        process.ErrorDataReceived += OnErrorReceived;
        process.Exited += OnProcessExited;

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
    }

    private static void OnOutputReceived(object sender, DataReceivedEventArgs args)
    {
        if (string.IsNullOrEmpty(args.Data) == false)
            UnityEngine.Debug.Log($"AutoGitCommit: {args.Data}");
    }

    private static void OnErrorReceived(object sender, DataReceivedEventArgs args)
    {
        if (string.IsNullOrEmpty(args.Data) == false)
            UnityEngine.Debug.LogError($"AutoGitCommit: {args.Data}");
    }

    private static void OnProcessExited(object sender, System.EventArgs args)
    {
        Process process = (Process)sender;
        int exitCode = process.ExitCode;

        EditorApplication.delayCall += () =>
        {
            if (exitCode == 0)
                UnityEngine.Debug.Log("AutoGitCommit: synchronization completed.");
            else
                UnityEngine.Debug.LogError($"AutoGitCommit: process failed with code {exitCode}.");

            process.Dispose();
            _isRunning = false;
        };
    }
}