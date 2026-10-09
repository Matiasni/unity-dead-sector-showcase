using UnityEngine;

public static class UIPalette
{
    public static readonly Color Background = new Color32(12, 14, 16, 255);
    public static readonly Color Metal = new Color32(22, 25, 27, 255);
    public static readonly Color MetalLight = new Color32(32, 36, 39, 255);
    public static readonly Color Steel = new Color32(84, 90, 92, 255);
    public static readonly Color SteelDark = new Color32(56, 60, 62, 255);
    public static readonly Color SteelLight = new Color32(150, 156, 156, 255);
    public static readonly Color Bone = new Color32(228, 222, 206, 255);
    public static readonly Color Amber = new Color32(240, 172, 40, 255);
    public static readonly Color AmberLight = new Color32(255, 214, 120, 255);
    public static readonly Color AmberDark = new Color32(166, 110, 22, 255);
    public static readonly Color Ink = new Color32(28, 20, 8, 255);
    public static readonly Color Rust = new Color32(204, 88, 44, 255);
    public static readonly Color Health = new Color32(138, 192, 72, 255);
    public static readonly Color HealthDark = new Color32(74, 116, 38, 255);
    public static readonly Color Danger = new Color32(222, 60, 44, 255);
    public static readonly Color Experience = new Color32(70, 196, 184, 255);
    public static readonly Color Cyan = new Color32(84, 184, 240, 255);
    public static readonly Color Muted = new Color32(100, 104, 106, 255);

    public static Color WithAlpha(Color color, float alpha)
    {
        color.a = alpha;
        return color;
    }
}
