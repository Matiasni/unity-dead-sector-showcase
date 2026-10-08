using UnityEngine;

[CreateAssetMenu(menuName = "Game/Character/Player Config")]
public class PlayerConfig : ScriptableObject
{
    [Header("Movement")]
    public MovementSettings Movement;
    [Header("Rotation")]
    public RotationSettings Rotation;
    [Header("Combat")]
    public CombatSettings Combat;

    [System.Serializable]
    public struct MovementSettings
    {
        [Tooltip("Player Movement Speed")]
        public float moveSpeed;
    }

    [System.Serializable]
    public struct RotationSettings
    {
        [Tooltip("How fast does the player takes to rotate")]
        public float rotationSpeed;
    }

    [System.Serializable]
    public struct CombatSettings
    {
        [Range(0f, 1f), Tooltip("How fast does the player move during combat (1 = 100% Normal Speed)")]
        public float moveSpeedMultiplier;
    }
}