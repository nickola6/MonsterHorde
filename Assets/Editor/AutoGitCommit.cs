using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public class AutoGitCommit
{
    private const double Delay = 2d;
    private const string WaitingKey = "AutoGitCommit.Waiting";
    private const string NextSyncTimeKey = "AutoGitCommit.NextSyncTime";

    private static readonly string ScriptPath = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Tools", "SyncToGit.ps1");

    private static double _nextSyncTime;
    private static bool _isWaiting;
    private static bool _isRunning;

    static AutoGitCommit()
    {
        EditorSceneManager.sceneSaved += OnSceneSaved;

        if (SessionState.GetBool(WaitingKey, false))
        {
            _isWaiting = true;
            _nextSyncTime = double.Parse(SessionState.GetString(NextSyncTimeKey, "0"), CultureInfo.InvariantCulture);

            EditorApplication.update += CheckSyncTime;
        }
    }

    private static void OnSceneSaved(UnityEngine.SceneManagement.Scene scene)
    {
        RequestSync();
    }

    internal static void RequestSync()
    {
        _nextSyncTime = EditorApplication.timeSinceStartup + Delay;
        _isWaiting = true;

        SessionState.SetBool(WaitingKey, true);
        SessionState.SetString(NextSyncTimeKey, _nextSyncTime.ToString(CultureInfo.InvariantCulture));

        EditorApplication.update -= CheckSyncTime;
        EditorApplication.update += CheckSyncTime;
    }

    private static void CheckSyncTime()
    {
        if (EditorApplication.timeSinceStartup < _nextSyncTime || _isRunning)
            return;

        _isWaiting = false;
        EditorApplication.update -= CheckSyncTime;

        SessionState.SetBool(WaitingKey, false);
        SessionState.EraseString(NextSyncTimeKey);

        if (File.Exists(ScriptPath) == false)
            return;

        _isRunning = true;

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

        try
        {
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
        }
        catch (Exception)
        {
            process.Dispose();
            _isRunning = false;
        }
    }

    private static void OnOutputReceived(object sender, DataReceivedEventArgs args) { }
    private static void OnErrorReceived(object sender, DataReceivedEventArgs args) { }

    private static void OnProcessExited(object sender, EventArgs args)
    {
        Process process = (Process)sender;

        EditorApplication.delayCall += () =>
        {
            process.Dispose();
            _isRunning = false;
        };
    }
}