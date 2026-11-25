
using System.IO;
using UnityEditor;

public class AutoGitAssetPostprocessor : AssetPostprocessor
{
    private static void OnPostprocessAllAssets(
        string[] importedAssets,
        string[] deletedAssets,
        string[] movedAssets,
        string[] movedFromAssetPaths)
    {
        if (ContainsScripts(importedAssets) == false &&
            ContainsScripts(deletedAssets) == false &&
            ContainsScripts(movedAssets) == false)
            return;

        AutoGitCommit.RequestSync();
    }

    private static bool ContainsScripts(string[] assetPaths)
    {
        foreach (string assetPath in assetPaths)
        {
            if (Path.GetExtension(assetPath) == ".cs")
                return true;
        }

        return false;
    }
}
