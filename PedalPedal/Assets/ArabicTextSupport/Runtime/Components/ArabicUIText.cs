using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace ArabicTextSupport
{
    /// <summary>
    /// Attach next to a legacy UI.Text component and type Arabic in the
    /// "Arabic Text" field (or set the <see cref="Text"/> property from code).
    /// The component shapes the letters, reorders the line for the left-to-right
    /// renderer, and re-implements word wrapping so wrapped Arabic paragraphs
    /// read in the correct order.
    /// </summary>
    [AddComponentMenu("UI/Arabic Text (Legacy Text)")]
    [RequireComponent(typeof(Text))]
    [ExecuteAlways]
    [DisallowMultipleComponent]
    public class ArabicUIText : MonoBehaviour
    {
        [SerializeField, TextArea(3, 10)]
        [Tooltip("Type the Arabic text here in normal (logical) order.")]
        private string arabicText = string.Empty;

        [SerializeField]
        [Tooltip("Keep harakat/diacritic marks (fatha, damma, shadda...) in the output.")]
        private bool preserveTashkeel = true;

        [SerializeField]
        [Tooltip("Optionally convert Western digits 0-9 to Arabic-Indic or Eastern digits.")]
        private DigitStyle digitStyle = DigitStyle.None;

        [SerializeField]
        [Tooltip("Wrap long paragraphs manually so that wrapped Arabic lines read in the correct order. Disable if you insert your own line breaks.")]
        private bool autoLineWrap = true;

        [SerializeField]
        [Tooltip("Switch a left-aligned Text to right alignment when the content is Arabic.")]
        private bool forceRightAlignment = true;

        private static readonly TextGenerator Measurer = new TextGenerator();

        private Text target;
        private bool applying;
        private float lastWidth = -1f;
#if UNITY_EDITOR
        private int lastSettingsHash;
#endif

        /// <summary>The Arabic text in logical order. Setting it refreshes the label.</summary>
        public string Text
        {
            get => arabicText;
            set { arabicText = value ?? string.Empty; Apply(); }
        }

        public Text Target => target != null ? target : (target = GetComponent<Text>());

        public bool PreserveTashkeel
        {
            get => preserveTashkeel;
            set { preserveTashkeel = value; Apply(); }
        }

        public DigitStyle Digits
        {
            get => digitStyle;
            set { digitStyle = value; Apply(); }
        }

        public bool AutoLineWrap
        {
            get => autoLineWrap;
            set { autoLineWrap = value; Apply(); }
        }

        private void OnEnable() => Apply();

        private void OnValidate() => Apply();

        private void Reset()
        {
            // Adding the component to an existing label keeps whatever was typed there.
            var text = GetComponent<Text>();
            if (text != null && !string.IsNullOrEmpty(text.text))
                arabicText = text.text;
            Apply();
        }

        private void OnRectTransformDimensionsChange()
        {
            if (applying || !isActiveAndEnabled)
                return;

            var rectTransform = transform as RectTransform;
            if (rectTransform == null)
                return;

            if (!Mathf.Approximately(rectTransform.rect.width, lastWidth))
                Apply();
        }

#if UNITY_EDITOR
        // In the editor, pick up font/size/overflow tweaks made on the Text component
        // without requiring a manual refresh.
        private void Update()
        {
            if (Application.isPlaying)
                return;

            int hash = SettingsHash();
            if (hash != lastSettingsHash)
            {
                lastSettingsHash = hash;
                Apply();
            }
        }

        private int SettingsHash()
        {
            var text = Target;
            if (text == null)
                return 0;

            var rectTransform = (RectTransform)transform;
            unchecked
            {
                int hash = text.fontSize;
                hash = hash * 31 + (text.font != null ? text.font.GetInstanceID() : 0);
                hash = hash * 31 + (int)text.fontStyle;
                hash = hash * 31 + (int)text.horizontalOverflow;
                hash = hash * 31 + (text.resizeTextForBestFit ? 1 : 0);
                hash = hash * 31 + (text.supportRichText ? 1 : 0);
                hash = hash * 31 + Mathf.RoundToInt(text.lineSpacing * 100f);
                hash = hash * 31 + Mathf.RoundToInt(rectTransform.rect.width * 10f);
                return hash;
            }
        }
#endif

        /// <summary>Re-runs the fix. Call after changing the Text font or size from code.</summary>
        public void Refresh() => Apply();

        private void Apply()
        {
            var text = Target;
            if (text == null)
                return;

            var rectTransform = (RectTransform)transform;
            lastWidth = rectTransform.rect.width;

            if (forceRightAlignment && ArabicFixer.HasArabicLetters(arabicText))
                text.alignment = ToRightAligned(text.alignment);

            string output = BuildOutput(text, rectTransform.rect.width);
            if (text.text == output)
                return;

            applying = true;
            text.text = output;
            applying = false;
        }

        private string BuildOutput(Text text, float maxWidth)
        {
            if (string.IsNullOrEmpty(arabicText))
                return string.Empty;
            if (!ArabicFixer.HasArabicLetters(arabicText))
                return arabicText;

            var options = new ArabicFixerOptions
            {
                preserveTashkeel = preserveTashkeel,
                digitStyle = digitStyle,
                protectRichTextTags = text.supportRichText,
            };

            string source = arabicText.Replace("\r\n", "\n");

            bool canWrap = autoLineWrap
                           && text.horizontalOverflow == HorizontalWrapMode.Wrap
                           && !text.resizeTextForBestFit
                           && text.font != null
                           && maxWidth > 1f;
            if (!canWrap)
                return ArabicFixer.Fix(source, options);

            var sb = new StringBuilder(source.Length + 16);
            string[] paragraphs = source.Split('\n');
            for (int i = 0; i < paragraphs.Length; i++)
            {
                if (i > 0)
                    sb.Append('\n');
                AppendWrappedParagraph(sb, paragraphs[i], text, maxWidth, options);
            }
            return sb.ToString();
        }

        // Greedy word wrapping done on the logical text: this way the first words of
        // the paragraph land on the first line (a plain reversed string would wrap
        // with the paragraph's END on the first line).
        private void AppendWrappedParagraph(StringBuilder sb, string paragraph, Text text, float maxWidth, ArabicFixerOptions options)
        {
            string[] words = paragraph.Split(' ');
            string current = string.Empty;
            bool firstLine = true;

            foreach (string word in words)
            {
                string candidate = current.Length == 0 ? word : current + " " + word;
                if (current.Length > 0 && MeasureWidth(text, ArabicFixer.Fix(candidate, options)) > maxWidth)
                {
                    AppendLine(sb, ArabicFixer.Fix(current, options), ref firstLine);
                    current = word;
                }
                else
                {
                    current = candidate;
                }
            }

            AppendLine(sb, ArabicFixer.Fix(current, options), ref firstLine);
        }

        private static void AppendLine(StringBuilder sb, string line, ref bool firstLine)
        {
            if (!firstLine)
                sb.Append('\n');
            sb.Append(line);
            firstLine = false;
        }

        private static float MeasureWidth(Text text, string line)
        {
            TextGenerationSettings settings = text.GetGenerationSettings(Vector2.zero);
            settings.horizontalOverflow = HorizontalWrapMode.Overflow;
            settings.verticalOverflow = VerticalWrapMode.Overflow;

            float pixelsPerUnit = text.pixelsPerUnit;
            if (pixelsPerUnit <= 0f)
                return 0f;
            return Measurer.GetPreferredWidth(line, settings) / pixelsPerUnit;
        }

        private static TextAnchor ToRightAligned(TextAnchor anchor)
        {
            switch (anchor)
            {
                case TextAnchor.UpperLeft: return TextAnchor.UpperRight;
                case TextAnchor.MiddleLeft: return TextAnchor.MiddleRight;
                case TextAnchor.LowerLeft: return TextAnchor.LowerRight;
                default: return anchor;
            }
        }
    }
}
