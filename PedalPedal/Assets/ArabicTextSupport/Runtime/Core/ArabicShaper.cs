using System.Collections.Generic;
using System.Text;

namespace ArabicTextSupport
{
    /// <summary>
    /// Converts Arabic letters from their generic Unicode code points to contextual
    /// presentation forms (isolated / initial / medial / final) and merges lam-alef
    /// ligatures. The output stays in logical (reading) order.
    /// </summary>
    internal static class ArabicShaper
    {
        private sealed class Unit
        {
            public bool IsLetter;
            public bool Omit;              // joining control chars that produce no glyph
            public char Literal;           // used when !IsLetter
            public ArabicLetterForms Forms;
            public StringBuilder Marks;    // tashkeel attached to this unit

            public void AddMark(char mark)
            {
                Marks ??= new StringBuilder(2);
                Marks.Append(mark);
            }
        }

        public static string Shape(string input, bool preserveTashkeel)
        {
            if (string.IsNullOrEmpty(input))
                return input ?? string.Empty;

            List<Unit> units = Parse(input, preserveTashkeel);

            var sb = new StringBuilder(input.Length);
            for (int i = 0; i < units.Count; i++)
            {
                Unit unit = units[i];
                if (!unit.IsLetter)
                {
                    if (!unit.Omit)
                        sb.Append(unit.Literal);
                    AppendMarks(sb, unit);
                    continue;
                }

                if (unit.Omit)
                {
                    AppendMarks(sb, unit);
                    continue;
                }

                bool linkPrevious = i > 0 && units[i - 1].IsLetter
                                    && units[i - 1].Forms.JoinsNext && unit.Forms.JoinsPrevious;
                bool linkNext = i < units.Count - 1 && units[i + 1].IsLetter
                                && units[i + 1].Forms.JoinsPrevious && unit.Forms.JoinsNext;

                char shaped = linkPrevious
                    ? (linkNext ? unit.Forms.Medial : unit.Forms.Final)
                    : (linkNext ? unit.Forms.Initial : unit.Forms.Isolated);

                sb.Append(shaped);
                AppendMarks(sb, unit);
            }

            return sb.ToString();
        }

        private static void AppendMarks(StringBuilder sb, Unit unit)
        {
            if (unit.Marks != null)
                sb.Append(unit.Marks);
        }

        private static List<Unit> Parse(string input, bool preserveTashkeel)
        {
            var units = new List<Unit>(input.Length);

            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];

                if (ArabicLetterTable.IsTashkeel(c))
                {
                    if (!preserveTashkeel)
                        continue;
                    if (units.Count > 0)
                        units[units.Count - 1].AddMark(c);
                    else
                        units.Add(new Unit { Literal = c });
                    continue;
                }

                if (c == ArabicLetterTable.ZeroWidthNonJoiner)
                {
                    // Breaks joining (needed for correct Persian/Urdu) but renders nothing.
                    units.Add(new Unit { Omit = true, Literal = c });
                    continue;
                }

                if (c == ArabicLetterTable.ZeroWidthJoiner)
                {
                    // Forces joining on both sides, renders nothing.
                    units.Add(new Unit
                    {
                        IsLetter = true,
                        Omit = true,
                        Forms = new ArabicLetterForms(c, c, c, c, true, true),
                    });
                    continue;
                }

                if (!ArabicLetterTable.Letters.TryGetValue(c, out ArabicLetterForms forms))
                {
                    units.Add(new Unit { Literal = c });
                    continue;
                }

                // Lam followed by an alef variant (tashkeel may sit in between)
                // collapses into a single lam-alef ligature glyph.
                if (c == ArabicLetterTable.Lam)
                {
                    int j = i + 1;
                    while (j < input.Length && ArabicLetterTable.IsTashkeel(input[j]))
                        j++;

                    if (j < input.Length && ArabicLetterTable.LamAlefLigatures.TryGetValue(input[j], out char ligature))
                    {
                        var unit = new Unit
                        {
                            IsLetter = true,
                            Forms = new ArabicLetterForms(ligature, (char)(ligature + 1), ligature, (char)(ligature + 1),
                                                          joinsPrevious: true, joinsNext: false),
                        };
                        if (preserveTashkeel)
                        {
                            for (int k = i + 1; k < j; k++)
                                unit.AddMark(input[k]);
                        }
                        units.Add(unit);
                        i = j; // skip past the alef; its own tashkeel attaches normally
                        continue;
                    }
                }

                units.Add(new Unit { IsLetter = true, Forms = forms });
            }

            return units;
        }
    }
}
