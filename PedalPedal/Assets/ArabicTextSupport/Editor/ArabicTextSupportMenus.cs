using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
#if TMP_PRESENT
using TMPro;
#endif

namespace ArabicTextSupport.EditorTools
{
    internal static class ArabicTextSupportMenus
    {
        // "Marhaban bialealam" (Hello World)
        private const string DefaultText = "\u0645\u0631\u062D\u0628\u0627 \u0628\u0627\u0644\u0639\u0627\u0644\u0645";

        [MenuItem("GameObject/UI/Arabic Text (Legacy)", false, 2062)]
        private static void CreateLegacy(MenuCommand command)
        {
            var go = new GameObject("ArabicText (Legacy)", typeof(RectTransform));
            GameObjectUtility.SetParentAndAlign(go, command.context as GameObject);
            ((RectTransform)go.transform).sizeDelta = new Vector2(300f, 80f);

            var text = go.AddComponent<Text>();
            text.font = GetLegacyFont();
            text.fontSize = 28;

            var arabic = go.AddComponent<ArabicUIText>();
            arabic.Text = DefaultText;

            Undo.RegisterCreatedObjectUndo(go, "Create Arabic Text (Legacy)");
            Selection.activeGameObject = go;
        }

#if TMP_PRESENT
        [MenuItem("GameObject/UI/Arabic Text (TextMeshPro)", false, 2063)]
        private static void CreateTextMeshPro(MenuCommand command)
        {
            var go = new GameObject("ArabicText (TMP)", typeof(RectTransform));
            GameObjectUtility.SetParentAndAlign(go, command.context as GameObject);
            ((RectTransform)go.transform).sizeDelta = new Vector2(300f, 80f);

            var tmp = go.AddComponent<TextMeshProUGUI>();
            tmp.fontSize = 28;

            var arabic = go.AddComponent<ArabicTMPText>();
            arabic.Text = DefaultText;

            Undo.RegisterCreatedObjectUndo(go, "Create Arabic Text (TMP)");
            Selection.activeGameObject = go;
        }
#endif

        private static Font GetLegacyFont()
        {
            Font font = null;
            try { font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"); }
            catch { /* removed on old Unity versions */ }
            if (font == null)
            {
                try { font = Resources.GetBuiltinResource<Font>("Arial.ttf"); }
                catch { /* removed on new Unity versions */ }
            }
            return font;
        }
    }
}
