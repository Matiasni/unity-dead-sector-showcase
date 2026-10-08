using UnityEngine;

[CreateAssetMenu(menuName = "Game/Character/Health Config")]
public class HealthConfig : ScriptableObject
{
    [Min(1), Tooltip("Max amount of health")]
    public int maxHealth = 100;
}
