using System.IO;
using UnityEditor;
using UnityEngine;

namespace ArabicTextSupport.EditorTools
{
    /// <summary>
    /// One-click export of the plugin folder as a .unitypackage that can be
    /// shared and imported into any Unity project.
    /// </summary>
    internal static class ArabicTextSupportExporter
    {
        private const string RootFolder = "Assets/ArabicTextSupport";
        private const string Version = "1.0.0";

        [MenuItem("Tools/Arabic Text Support/Export .unitypackage")]
        private static void Export()
        {
            if (!AssetDatabase.IsValidFolder(RootFolder))
            {
                EditorUtility.DisplayDialog("Arabic Text Support",
                    $"Folder '{RootFolder}' was not found. The plugin folder must live directly under Assets to be exported.", "OK");
                return;
            }

            string path = EditorUtility.SaveFilePanel(
                "Export Arabic Text Support",
                Directory.GetParent(Application.dataPath)!.FullName,
                $"ArabicTextSupport-{Version}",
                "unitypackage");
            if (string.IsNullOrEmpty(path))
                return;

            AssetDatabase.ExportPackage(RootFolder, path, ExportPackageOptions.Recurse);
            Debug.Log($"Arabic Text Support exported to: {path}");
            EditorUtility.RevealInFinder(path);
        }
    }
}
