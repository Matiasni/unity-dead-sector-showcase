using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class KeyGlyphs
{
    private static readonly Regex Token = new(@"\[([^\]\s]+)\]");

    private static readonly Dictionary<string, string> MouseSprites = new()
    {
        { "LMB", "icon_mouse_lmb" },
        { "RMB", "icon_mouse_rmb" },
        { "WHEEL", "icon_mouse_wheel" }
    };

    private static readonly string KeyBackground = ColorUtility.ToHtmlStringRGBA(UIPalette.WithAlpha(UIPalette.Amber, 0.25f));
    private static readonly string KeyColor = ColorUtility.ToHtmlStringRGB(UIPalette.AmberLight);

    public static string Format(string text)
    {
        if (string.IsNullOrEmpty(text)) return text;

        return Token.Replace(text, match => ToGlyph(match.Groups[1].Value));
    }

    private static string ToGlyph(string key)
    {
        if (MouseSprites.TryGetValue(key, out string sprite))
            return $"<size=150%><sprite name=\"{sprite}\" tint=1></size>";

        return $"<mark=#{KeyBackground} padding=\"8,8,3,3\"><b><color=#{KeyColor}>{key}</color></b></mark>";
    }
}
