#if TMP_PRESENT
using TMPro;
using UnityEngine;

namespace ArabicTextSupport
{
    /// <summary>
    /// Attach next to any TextMeshPro text component (UI or 3D) and type Arabic in
    /// the "Arabic Text" field (or set the <see cref="Text"/> property from code).
    /// The component shapes the letters and enables TextMeshPro's right-to-left
    /// mode, which keeps line wrapping and alignment correct on multi-line text.
    /// </summary>
    [AddComponentMenu("UI/Arabic Text (TextMeshPro)")]
    [ExecuteAlways]
    [DisallowMultipleComponent]
    public class ArabicTMPText : MonoBehaviour
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
        [Tooltip("Switch a left-aligned text to right alignment when the content is Arabic.")]
        private bool forceRightAlignment = true;

        private TMP_Text target;

        /// <summary>The Arabic text in logical order. Setting it refreshes the label.</summary>
        public string Text
        {
            get => arabicText;
            set { arabicText = value ?? string.Empty; Apply(); }
        }

        public TMP_Text Target => target != null ? target : (target = GetComponent<TMP_Text>());

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

        private void OnEnable()
        {
            if (Target == null)
                Debug.LogWarning("ArabicTMPText needs a TextMeshPro component (TextMeshProUGUI or TextMeshPro) on the same GameObject.", this);
            Apply();
        }

        private void OnValidate() => Apply();

        private void Reset()
        {
            // Adding the component to an existing label keeps whatever was typed there.
            var tmp = GetComponent<TMP_Text>();
            if (tmp != null && !string.IsNullOrEmpty(tmp.text))
                arabicText = tmp.text;
            Apply();
        }

        /// <summary>Re-runs the fix. Call if you change fixer-related settings from code.</summary>
        public void Refresh() => Apply();

        private void Apply()
        {
            var tmp = Target;
            if (tmp == null)
                return;

            bool hasArabic = ArabicFixer.HasArabicLetters(arabicText);

            // TMP's own right-to-left mode lays glyphs out from the right edge and
            // wraps long paragraphs correctly, so the fixed string stays in logical
            // order (see ArabicFixer.FixForTextMeshPro).
            tmp.isRightToLeftText = hasArabic;

            if (hasArabic && forceRightAlignment && tmp.horizontalAlignment == HorizontalAlignmentOptions.Left)
                tmp.horizontalAlignment = HorizontalAlignmentOptions.Right;

            if (!hasArabic)
            {
                tmp.text = arabicText;
                return;
            }

            var options = new ArabicFixerOptions
            {
                preserveTashkeel = preserveTashkeel,
                digitStyle = digitStyle,
                protectRichTextTags = tmp.richText,
            };
            tmp.text = ArabicFixer.FixForTextMeshPro(arabicText, options);
        }
    }
}
#endif
