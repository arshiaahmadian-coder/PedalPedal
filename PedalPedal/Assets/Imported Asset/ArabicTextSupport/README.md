# Arabic Text Support for Unity

Fixes Arabic (and Persian/Urdu) text rendering in Unity for **both** text systems:

- **Legacy `UI.Text` (uGUI)** — via the `ArabicUIText` component
- **TextMeshPro** (`TextMeshProUGUI` and 3D `TextMeshPro`) — via the `ArabicTMPText` component

Without this plugin Unity draws Arabic letters disconnected and in the wrong
(left-to-right) order. The plugin:

- Converts letters to their correct contextual forms (initial / medial / final / isolated)
- Merges lam-alef ligatures (لا، لأ، لإ، لآ)
- Displays the text right-to-left, including correct line wrapping for paragraphs
- Handles mixed content: English words and numbers inside Arabic sentences keep
  their order, brackets are mirrored
- Optional: keep or strip tashkeel (harakat), convert digits to ٠١٢٣ or ۰۱۲۳
- Supports Persian and Urdu extra letters (پ چ ژ گ ک ی ٹ ڈ ھ ے ...) and the
  zero-width non-joiner used in Persian

Requires Unity 2020.3 or newer. TextMeshPro is optional — in a project without
TMP the package still compiles and the legacy component keeps working.

---

## Quick start — TextMeshPro

1. Add a normal `TextMeshPro - Text (UI)` object (or use
   **GameObject > UI > Arabic Text (TextMeshPro)** which sets everything up).
2. Add the **Arabic Text (TextMeshPro)** component to it.
3. Type your Arabic in the component's **Arabic Text** field — not in the TMP
   text field (the component overwrites the TMP field with the fixed text).

From code:

```csharp
GetComponent<ArabicTextSupport.ArabicTMPText>().Text = "مرحبا بالعالم";
```

The component automatically turns on TMP's `isRightToLeftText`, which is what
makes multi-line paragraphs wrap in the correct reading order.

### Your TMP font asset must contain Arabic glyphs

This is the #1 support question. The default `LiberationSans SDF` has **no
Arabic glyphs**, so you will see squares. Create your own font asset:

1. Download a font with Arabic **presentation forms**, e.g. *Noto Naskh Arabic*,
   *Amiri*, *Cairo*, or *Tahoma*.
2. **Window > TextMeshPro > Font Asset Creator**, select the font.
3. Set **Character Set** to `Unicode Range (Hex)` and enter:
   ```
   20-7E,600-6FF,750-77F,8A0-8FF,FB50-FDFF,FE70-FEFF
   ```
4. Generate, save, and assign the font asset to your TMP text.

(Alternatively set the font asset's *Atlas Population Mode* to **Dynamic** and
Unity will add glyphs as needed while you test.)

## Quick start — Legacy UI.Text

1. Add a `UI > Legacy > Text` object (or use **GameObject > UI > Arabic Text (Legacy)**).
2. Add the **Arabic Text (Legacy Text)** component.
3. Type your Arabic in the component's **Arabic Text** field.

From code:

```csharp
GetComponent<ArabicTextSupport.ArabicUIText>().Text = "مرحبا بالعالم";
```

Use a Font that contains Arabic presentation-form glyphs. With dynamic fonts,
Unity falls back to OS fonts on most platforms, so Arabic usually works out of
the box — but ship a proper Arabic font for reliable results on all devices.

**Auto Line Wrap**: when the Text is set to wrap, the component measures and
wraps the paragraph itself so that wrapped lines read top-to-bottom in the
correct order (a plain "reverse the string" fixer gets this wrong). Disable it
if you want to control line breaks manually. It is automatically skipped when
*Best Fit* is enabled (use manual line breaks in that case).

## Fixing strings from code (any renderer)

```csharp
using ArabicTextSupport;

// Legacy UI.Text, 3D TextMesh, IMGUI (left-to-right renderers):
string visual = ArabicFixer.Fix("السلام عليكم");

// TextMeshPro — pair with isRightToLeftText = true:
tmp.isRightToLeftText = true;
tmp.text = ArabicFixer.FixForTextMeshPro("السلام عليكم");

// Options:
var options = new ArabicFixerOptions
{
    preserveTashkeel = true,
    digitStyle = DigitStyle.ArabicIndic,   // 0-9 -> ٠-٩
    protectRichTextTags = true,
};
string fixedText = ArabicFixer.Fix(input, options);
```

`ArabicFixer.HasArabicLetters(string)` tells you whether a string needs fixing
(useful for multi-language games).

## Exporting / sharing the plugin

**Tools > Arabic Text Support > Export .unitypackage** builds a
`ArabicTextSupport-x.y.z.unitypackage` containing this folder. Anyone can
install it via **Assets > Import Package > Custom Package**. No setup needed
after import: the assembly definitions detect whether TextMeshPro is installed
(`TMP_PRESENT` version define) so the package compiles everywhere.

## Known limitations

- **Rich text**: tags are kept intact, but a tag inside an *English* phrase in
  Arabic text can end up styling a shifted range. Tags around Arabic words work.
- **Tashkeel placement** depends on the font; legacy Text renders marks at a
  fixed offset rather than positioned per-letter (TMP does better).
- No kashida (تطويل) justification.
- Nested bidirectional text (Arabic quoting English quoting Arabic...) uses a
  simplified algorithm; extreme cases may order neutrals differently than a
  full Unicode bidi implementation.
- If a long English run inside an Arabic paragraph wraps mid-run across lines,
  reading order of that English fragment can break (limitation of all
  string-level fixers).

## Folder layout

```
ArabicTextSupport/
  Runtime/
    Core/                 Pure C# engine (no Unity dependencies; unit-testable)
      ArabicLetterTable.cs   Unicode presentation-form tables
      ArabicShaper.cs        Contextual shaping + ligatures
      BidiHelper.cs          RTL reordering, mixed runs, mirroring
      ArabicFixer.cs         Public API
    Components/
      ArabicUIText.cs        Component for legacy UI.Text
      ArabicTMPText.cs       Component for TextMeshPro (compiled only if TMP exists)
  Editor/
    ArabicTextSupportMenus.cs     GameObject creation menu items
    ArabicTextSupportExporter.cs  .unitypackage export menu item
```
