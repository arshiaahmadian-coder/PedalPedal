using System.Text;

namespace ArabicTextSupport
{
    public enum DigitStyle
    {
        /// <summary>Keep digits exactly as typed.</summary>
        None = 0,

        /// <summary>Convert 0-9 to Arabic-Indic digits (U+0660..U+0669).</summary>
        ArabicIndic = 1,

        /// <summary>Convert 0-9 to Eastern (Persian/Urdu) digits (U+06F0..U+06F9).</summary>
        EasternArabicIndic = 2,
    }

    [System.Serializable]
    public class ArabicFixerOptions
    {
        /// <summary>Keep harakat/diacritic marks (fatha, damma, shadda...) in the output.</summary>
        public bool preserveTashkeel = true;

        /// <summary>Optionally convert Western digits 0-9 to Arabic-Indic or Eastern digits.</summary>
        public DigitStyle digitStyle = DigitStyle.None;

        /// <summary>Keep rich text tags such as &lt;b&gt; intact instead of reordering their characters.</summary>
        public bool protectRichTextTags = true;

        public static ArabicFixerOptions Default { get; } = new ArabicFixerOptions();
    }

    /// <summary>
    /// Entry point of the plugin. Converts Arabic text typed in logical order into a
    /// string a Unity text renderer can display correctly:
    /// letters get their contextual joining forms, lam-alef ligatures are merged, and
    /// mixed Arabic/Latin/number content is reordered as needed.
    /// </summary>
    public static class ArabicFixer
    {
        /// <summary>
        /// Fix Arabic for renderers that draw glyphs left-to-right:
        /// UI.Text (uGUI), 3D TextMesh, IMGUI and string-based custom renderers.
        /// </summary>
        public static string Fix(string text) => Process(text, ArabicFixerOptions.Default, rtlRenderer: false);

        /// <inheritdoc cref="Fix(string)"/>
        public static string Fix(string text, ArabicFixerOptions options) => Process(text, options, rtlRenderer: false);

        /// <summary>
        /// Fix Arabic for TextMeshPro. The returned string must be shown on a TMP_Text
        /// with <c>isRightToLeftText = true</c> (the ArabicTMPText component does this
        /// automatically). Keeping TMP in RTL mode preserves correct line wrapping.
        /// </summary>
        public static string FixForTextMeshPro(string text) => Process(text, ArabicFixerOptions.Default, rtlRenderer: true);

        /// <inheritdoc cref="FixForTextMeshPro(string)"/>
        public static string FixForTextMeshPro(string text, ArabicFixerOptions options) => Process(text, options, rtlRenderer: true);

        /// <summary>True if the string contains at least one Arabic-script letter.</summary>
        public static bool HasArabicLetters(string text)
        {
            if (string.IsNullOrEmpty(text))
                return false;

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if (ArabicLetterTable.IsArabicLetter(c))
                    return true;
                // Also catch less common Arabic letters and pre-shaped presentation forms.
                if (((c >= '\u0600' && c <= '\u06FF')
                     || (c >= '\uFB50' && c <= '\uFDFF')
                     || (c >= '\uFE70' && c <= '\uFEFE')) && char.IsLetter(c))
                    return true;
            }
            return false;
        }

        private static string Process(string text, ArabicFixerOptions options, bool rtlRenderer)
        {
            if (string.IsNullOrEmpty(text))
                return text ?? string.Empty;

            options = options ?? ArabicFixerOptions.Default;

            text = text.Replace("\r\n", "\n");
            if (options.digitStyle != DigitStyle.None)
                text = ConvertDigits(text, options.digitStyle);

            // Lines are processed independently so reordering never crosses a line break.
            string[] lines = text.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                string shaped = ArabicShaper.Shape(lines[i], options.preserveTashkeel);
                lines[i] = rtlRenderer
                    ? BidiHelper.PrepareForRtlRenderer(shaped, options.protectRichTextTags)
                    : BidiHelper.ToVisualForLtrRenderer(shaped, options.protectRichTextTags);
            }

            return string.Join("\n", lines);
        }

        private static string ConvertDigits(string text, DigitStyle style)
        {
            char zero = style == DigitStyle.ArabicIndic ? '\u0660' : '\u06F0';
            var sb = new StringBuilder(text.Length);
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                sb.Append(c >= '0' && c <= '9' ? (char)(zero + (c - '0')) : c);
            }
            return sb.ToString();
        }
    }
}
