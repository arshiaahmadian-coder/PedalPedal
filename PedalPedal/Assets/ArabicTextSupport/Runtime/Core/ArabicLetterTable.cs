using System.Collections.Generic;

namespace ArabicTextSupport
{
    /// <summary>
    /// Contextual glyph forms for a single Arabic letter, plus its joining behaviour.
    /// Form code points come from the Unicode "Arabic Presentation Forms A/B" blocks.
    /// </summary>
    internal readonly struct ArabicLetterForms
    {
        public readonly char Isolated;
        public readonly char Final;
        public readonly char Initial;
        public readonly char Medial;

        /// <summary>The letter accepts a connection from the letter before it.</summary>
        public readonly bool JoinsPrevious;

        /// <summary>The letter connects to the letter after it (dual-joining letters only).</summary>
        public readonly bool JoinsNext;

        public ArabicLetterForms(char isolated, char final, char initial, char medial, bool joinsPrevious, bool joinsNext)
        {
            Isolated = isolated;
            Final = final;
            Initial = initial;
            Medial = medial;
            JoinsPrevious = joinsPrevious;
            JoinsNext = joinsNext;
        }

        public static ArabicLetterForms Dual(char isolated, char final, char initial, char medial)
            => new ArabicLetterForms(isolated, final, initial, medial, true, true);

        /// <summary>Letters such as alef/dal/reh/waw that join only to the preceding letter.</summary>
        public static ArabicLetterForms RightJoining(char isolated, char final)
            => new ArabicLetterForms(isolated, final, isolated, final, true, false);

        public static ArabicLetterForms NonJoining(char isolated)
            => new ArabicLetterForms(isolated, isolated, isolated, isolated, false, false);
    }

    internal static class ArabicLetterTable
    {
        public const char Lam = '\u0644';
        public const char Tatweel = '\u0640';
        public const char ZeroWidthNonJoiner = '\u200C';
        public const char ZeroWidthJoiner = '\u200D';

        public static readonly Dictionary<char, ArabicLetterForms> Letters = new Dictionary<char, ArabicLetterForms>
        {
            { '\u0621', ArabicLetterForms.NonJoining('\uFE80') }, // hamza
            { '\u0622', ArabicLetterForms.RightJoining('\uFE81', '\uFE82') }, // alef with madda above
            { '\u0623', ArabicLetterForms.RightJoining('\uFE83', '\uFE84') }, // alef with hamza above
            { '\u0624', ArabicLetterForms.RightJoining('\uFE85', '\uFE86') }, // waw with hamza above
            { '\u0625', ArabicLetterForms.RightJoining('\uFE87', '\uFE88') }, // alef with hamza below
            { '\u0627', ArabicLetterForms.RightJoining('\uFE8D', '\uFE8E') }, // alef
            { '\u0629', ArabicLetterForms.RightJoining('\uFE93', '\uFE94') }, // teh marbuta
            { '\u062F', ArabicLetterForms.RightJoining('\uFEA9', '\uFEAA') }, // dal
            { '\u0630', ArabicLetterForms.RightJoining('\uFEAB', '\uFEAC') }, // thal
            { '\u0631', ArabicLetterForms.RightJoining('\uFEAD', '\uFEAE') }, // reh
            { '\u0632', ArabicLetterForms.RightJoining('\uFEAF', '\uFEB0') }, // zain
            { '\u0648', ArabicLetterForms.RightJoining('\uFEED', '\uFEEE') }, // waw
            { '\u0649', ArabicLetterForms.RightJoining('\uFEEF', '\uFEF0') }, // alef maksura
            { '\u0671', ArabicLetterForms.RightJoining('\uFB50', '\uFB51') }, // alef wasla
            { '\u0698', ArabicLetterForms.RightJoining('\uFB8A', '\uFB8B') }, // jeh (Persian)
            { '\u0688', ArabicLetterForms.RightJoining('\uFB88', '\uFB89') }, // ddal (Urdu)
            { '\u0691', ArabicLetterForms.RightJoining('\uFB8C', '\uFB8D') }, // rreh (Urdu)
            { '\u06BA', ArabicLetterForms.RightJoining('\uFB9E', '\uFB9F') }, // noon ghunna (Urdu)
            { '\u06C0', ArabicLetterForms.RightJoining('\uFBA4', '\uFBA5') }, // heh with yeh above
            { '\u06D2', ArabicLetterForms.RightJoining('\uFBAE', '\uFBAF') }, // yeh barree (Urdu)
            { '\u06D3', ArabicLetterForms.RightJoining('\uFBB0', '\uFBB1') }, // yeh barree with hamza (Urdu)
            { '\u0626', ArabicLetterForms.Dual('\uFE89', '\uFE8A', '\uFE8B', '\uFE8C') }, // yeh with hamza above
            { '\u0628', ArabicLetterForms.Dual('\uFE8F', '\uFE90', '\uFE91', '\uFE92') }, // beh
            { '\u062A', ArabicLetterForms.Dual('\uFE95', '\uFE96', '\uFE97', '\uFE98') }, // teh
            { '\u062B', ArabicLetterForms.Dual('\uFE99', '\uFE9A', '\uFE9B', '\uFE9C') }, // theh
            { '\u062C', ArabicLetterForms.Dual('\uFE9D', '\uFE9E', '\uFE9F', '\uFEA0') }, // jeem
            { '\u062D', ArabicLetterForms.Dual('\uFEA1', '\uFEA2', '\uFEA3', '\uFEA4') }, // hah
            { '\u062E', ArabicLetterForms.Dual('\uFEA5', '\uFEA6', '\uFEA7', '\uFEA8') }, // khah
            { '\u0633', ArabicLetterForms.Dual('\uFEB1', '\uFEB2', '\uFEB3', '\uFEB4') }, // seen
            { '\u0634', ArabicLetterForms.Dual('\uFEB5', '\uFEB6', '\uFEB7', '\uFEB8') }, // sheen
            { '\u0635', ArabicLetterForms.Dual('\uFEB9', '\uFEBA', '\uFEBB', '\uFEBC') }, // sad
            { '\u0636', ArabicLetterForms.Dual('\uFEBD', '\uFEBE', '\uFEBF', '\uFEC0') }, // dad
            { '\u0637', ArabicLetterForms.Dual('\uFEC1', '\uFEC2', '\uFEC3', '\uFEC4') }, // tah
            { '\u0638', ArabicLetterForms.Dual('\uFEC5', '\uFEC6', '\uFEC7', '\uFEC8') }, // zah
            { '\u0639', ArabicLetterForms.Dual('\uFEC9', '\uFECA', '\uFECB', '\uFECC') }, // ain
            { '\u063A', ArabicLetterForms.Dual('\uFECD', '\uFECE', '\uFECF', '\uFED0') }, // ghain
            { '\u0641', ArabicLetterForms.Dual('\uFED1', '\uFED2', '\uFED3', '\uFED4') }, // feh
            { '\u0642', ArabicLetterForms.Dual('\uFED5', '\uFED6', '\uFED7', '\uFED8') }, // qaf
            { '\u0643', ArabicLetterForms.Dual('\uFED9', '\uFEDA', '\uFEDB', '\uFEDC') }, // kaf
            { '\u0644', ArabicLetterForms.Dual('\uFEDD', '\uFEDE', '\uFEDF', '\uFEE0') }, // lam
            { '\u0645', ArabicLetterForms.Dual('\uFEE1', '\uFEE2', '\uFEE3', '\uFEE4') }, // meem
            { '\u0646', ArabicLetterForms.Dual('\uFEE5', '\uFEE6', '\uFEE7', '\uFEE8') }, // noon
            { '\u0647', ArabicLetterForms.Dual('\uFEE9', '\uFEEA', '\uFEEB', '\uFEEC') }, // heh
            { '\u064A', ArabicLetterForms.Dual('\uFEF1', '\uFEF2', '\uFEF3', '\uFEF4') }, // yeh
            { '\u067E', ArabicLetterForms.Dual('\uFB56', '\uFB57', '\uFB58', '\uFB59') }, // peh (Persian)
            { '\u0686', ArabicLetterForms.Dual('\uFB7A', '\uFB7B', '\uFB7C', '\uFB7D') }, // tcheh (Persian)
            { '\u06A9', ArabicLetterForms.Dual('\uFB8E', '\uFB8F', '\uFB90', '\uFB91') }, // keheh (Persian)
            { '\u06AF', ArabicLetterForms.Dual('\uFB92', '\uFB93', '\uFB94', '\uFB95') }, // gaf (Persian)
            { '\u06CC', ArabicLetterForms.Dual('\uFBFC', '\uFBFD', '\uFBFE', '\uFBFF') }, // farsi yeh (Persian)
            { '\u0679', ArabicLetterForms.Dual('\uFB66', '\uFB67', '\uFB68', '\uFB69') }, // tteh (Urdu)
            { '\u06BE', ArabicLetterForms.Dual('\uFBAA', '\uFBAB', '\uFBAC', '\uFBAD') }, // heh doachashmee (Urdu)
            { '\u06C1', ArabicLetterForms.Dual('\uFBA6', '\uFBA7', '\uFBA8', '\uFBA9') }, // heh goal (Urdu)
            { Tatweel, ArabicLetterForms.Dual(Tatweel, Tatweel, Tatweel, Tatweel) }, // tatweel/kashida
        };

        // Lam + alef pairs collapse into a single ligature glyph.
        // Value = isolated ligature form; the final form is the next code point.
        public static readonly Dictionary<char, char> LamAlefLigatures = new Dictionary<char, char>
        {
            { '\u0622', '\uFEF5' }, // lam + alef with madda
            { '\u0623', '\uFEF7' }, // lam + alef with hamza above
            { '\u0625', '\uFEF9' }, // lam + alef with hamza below
            { '\u0627', '\uFEFB' }, // lam + alef
        };

        /// <summary>Harakat / diacritic marks that attach to the preceding letter.</summary>
        public static bool IsTashkeel(char c) => (c >= '\u064B' && c <= '\u065F') || c == '\u0670';

        public static bool IsArabicLetter(char c) => Letters.ContainsKey(c);
    }
}
