using UnityEngine;

[CreateAssetMenu(menuName = "Game/Economy/Resource")]
public class ResourceDefinition : ScriptableObject
{
    public string displayName;
    public Color color = Color.white;
    [Min(0), Tooltip("Max amount the player can carry (0 = unlimited)")]
    public int maxAmount;

    public bool HasLimit => maxAmount > 0;
}
