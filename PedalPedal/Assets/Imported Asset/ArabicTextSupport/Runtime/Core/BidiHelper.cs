using System.Collections.Generic;
using System.Text;

namespace ArabicTextSupport
{
    /// <summary>
    /// Minimal right-to-left reordering for a single line of shaped Arabic text.
    /// Handles mixed Arabic/Latin/number runs, keeps diacritics attached to their
    /// base letter, mirrors brackets, and can keep rich-text tags intact.
    /// This is a practical simplification of the Unicode bidi algorithm that covers
    /// typical game UI strings.
    /// </summary>
    internal static class BidiHelper
    {
        private enum Dir { Rtl, Ltr, Neutral }

        private struct Cluster
        {
            public int Start;
            public int Length;
            public Dir Direction;
            public bool Atomic; // rich-text tag: never mirrored, interior never reordered
        }

        private static readonly Dictionary<char, char> MirroredPairs = new Dictionary<char, char>
        {
            { '(', ')' }, { ')', '(' },
            { '[', ']' }, { ']', '[' },
            { '{', '}' }, { '}', '{' },
            { '\u00AB', '\u00BB' }, { '\u00BB', '\u00AB' }, // guillemets
            { '\uFD3E', '\uFD3F' }, { '\uFD3F', '\uFD3E' }, // ornate parentheses
        };

        /// <summary>
        /// For renderers that draw glyphs strictly left-to-right (UI.Text, TextMesh,
        /// IMGUI): returns the line fully reordered into visual order.
        /// </summary>
        public static string ToVisualForLtrRenderer(string shapedLine, bool protectRichTextTags)
        {
            if (string.IsNullOrEmpty(shapedLine))
                return shapedLine ?? string.Empty;

            List<Cluster> clusters = SplitClusters(shapedLine, protectRichTextTags);
            ResolveNeutrals(clusters);

            var sb = new StringBuilder(shapedLine.Length);
            int runEnd = clusters.Count - 1;
            while (runEnd >= 0)
            {
                int runStart = runEnd;
                while (runStart > 0 && clusters[runStart - 1].Direction == clusters[runEnd].Direction)
                    runStart--;

                if (clusters[runEnd].Direction == Dir.Ltr)
                {
                    for (int i = runStart; i <= runEnd; i++)
                        AppendCluster(sb, shapedLine, clusters[i], mirror: false);
                }
                else
                {
                    for (int i = runEnd; i >= runStart; i--)
                        AppendCluster(sb, shapedLine, clusters[i], mirror: true);
                }

                runEnd = runStart - 1;
            }

            return sb.ToString();
        }

        /// <summary>
        /// For TextMeshPro with <c>isRightToLeftText</c> enabled: the renderer already
        /// lays glyphs out right-to-left (and wraps lines correctly), so the string
        /// stays in logical order. Only embedded left-to-right runs (Latin words,
        /// numbers) are reversed so they read correctly, and brackets are mirrored.
        /// </summary>
        public static string PrepareForRtlRenderer(string shapedLine, bool protectRichTextTags)
        {
            if (string.IsNullOrEmpty(shapedLine))
                return shapedLine ?? string.Empty;

            List<Cluster> clusters = SplitClusters(shapedLine, protectRichTextTags);
            ResolveNeutrals(clusters);

            var sb = new StringBuilder(shapedLine.Length);
            int runStart = 0;
            while (runStart < clusters.Count)
            {
                int runEnd = runStart;
                while (runEnd < clusters.Count - 1 && clusters[runEnd + 1].Direction == clusters[runStart].Direction)
                    runEnd++;

                if (clusters[runStart].Direction == Dir.Ltr)
                {
                    for (int i = runEnd; i >= runStart; i--)
                        AppendCluster(sb, shapedLine, clusters[i], mirror: false);
                }
                else
                {
                    for (int i = runStart; i <= runEnd; i++)
                        AppendCluster(sb, shapedLine, clusters[i], mirror: true);
                }

                runStart = runEnd + 1;
            }

            return sb.ToString();
        }

        private static void AppendCluster(StringBuilder sb, string s, Cluster cluster, bool mirror)
        {
            if (mirror && cluster.Length == 1 && !cluster.Atomic
                && MirroredPairs.TryGetValue(s[cluster.Start], out char mirrored))
            {
                sb.Append(mirrored);
                return;
            }
            sb.Append(s, cluster.Start, cluster.Length);
        }

        private static List<Cluster> SplitClusters(string s, bool protectTags)
        {
            var clusters = new List<Cluster>(s.Length);
            int i = 0;
            while (i < s.Length)
            {
                char c = s[i];

                if (protectTags && c == '<')
                {
                    int close = FindTagEnd(s, i);
                    if (close > i)
                    {
                        clusters.Add(new Cluster { Start = i, Length = close - i + 1, Direction = Dir.Neutral, Atomic = true });
                        i = close + 1;
                        continue;
                    }
                }

                int length = char.IsHighSurrogate(c) && i + 1 < s.Length && char.IsLowSurrogate(s[i + 1]) ? 2 : 1;
                int end = i + length;
                while (end < s.Length && IsCombining(s[end]))
                    end++;

                clusters.Add(new Cluster { Start = i, Length = end - i, Direction = Classify(c) });
                i = end;
            }
            return clusters;
        }

        private static int FindTagEnd(string s, int start)
        {
            if (start + 1 >= s.Length)
                return -1;

            char next = s[start + 1];
            bool looksLikeTag = next == '/' || next == '#'
                                || (next >= 'a' && next <= 'z') || (next >= 'A' && next <= 'Z');
            if (!looksLikeTag)
                return -1;

            int limit = System.Math.Min(s.Length, start + 128);
            for (int i = start + 1; i < limit; i++)
            {
                if (s[i] == '>')
                    return i;
                if (s[i] == '<' || s[i] == '\n')
                    return -1;
            }
            return -1;
        }

        private static bool IsCombining(char c)
        {
            return ArabicLetterTable.IsTashkeel(c)
                   || (c >= '\u0300' && c <= '\u036F')  // generic combining marks
                   || (c >= '\uFE70' && c <= '\uFE7F'); // tashkeel presentation forms
        }

        private static Dir Classify(char c)
        {
            // Digits (of any style) keep their left-to-right order inside Arabic text.
            if ((c >= '0' && c <= '9')
                || (c >= '\u0660' && c <= '\u0669')
                || (c >= '\u06F0' && c <= '\u06F9'))
                return Dir.Ltr;

            if ((c >= '\u0600' && c <= '\u06FF')  // Arabic
                || (c >= '\u0750' && c <= '\u077F')  // Arabic Supplement
                || (c >= '\u08A0' && c <= '\u08FF')  // Arabic Extended-A
                || (c >= '\uFB50' && c <= '\uFDFF')  // Presentation Forms-A
                || (c >= '\uFE70' && c <= '\uFEFF')) // Presentation Forms-B
                return Dir.Rtl;

            if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                return Dir.Ltr;

            if (c > '\u00BF' && char.IsLetter(c))
                return Dir.Ltr; // other scripts: treat as left-to-right

            return Dir.Neutral;
        }

        private static void ResolveNeutrals(List<Cluster> clusters)
        {
            int i = 0;
            while (i < clusters.Count)
            {
                if (clusters[i].Direction != Dir.Neutral)
                {
                    i++;
                    continue;
                }

                int spanEnd = i;
                while (spanEnd < clusters.Count - 1 && clusters[spanEnd + 1].Direction == Dir.Neutral)
                    spanEnd++;

                Dir before = Dir.Rtl;
                for (int k = i - 1; k >= 0; k--)
                {
                    if (clusters[k].Direction != Dir.Neutral) { before = clusters[k].Direction; break; }
                }

                Dir after = Dir.Rtl;
                for (int k = spanEnd + 1; k < clusters.Count; k++)
                {
                    if (clusters[k].Direction != Dir.Neutral) { after = clusters[k].Direction; break; }
                }

                // Neutrals surrounded by Latin stay Latin; everything else falls back
                // to the paragraph direction (right-to-left, since we only run on
                // strings that contain Arabic).
                Dir resolved = before == Dir.Ltr && after == Dir.Ltr ? Dir.Ltr : Dir.Rtl;
                for (int k = i; k <= spanEnd; k++)
                {
                    Cluster cluster = clusters[k];
                    cluster.Direction = resolved;
                    clusters[k] = cluster;
                }

                i = spanEnd + 1;
            }
        }
    }
}
