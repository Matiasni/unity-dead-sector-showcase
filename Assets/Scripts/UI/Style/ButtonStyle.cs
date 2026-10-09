using UnityEngine;

[CreateAssetMenu(menuName = "Game/UI/Button Style")]
public class ButtonStyle : ScriptableObject
{
    [Header("Sprites")]
    public Sprite normal;
    public Sprite highlighted;
    public Sprite pressed;
    public Sprite disabled;

    [Header("Label")]
    public Color labelNormal = Color.white;
    public Color labelHighlighted = Color.white;
    public Color labelPressed = Color.white;
    public Color labelDisabled = Color.gray;
}
