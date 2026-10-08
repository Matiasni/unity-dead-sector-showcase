using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Game/Abilities/Grenade Settings")]
public class GrenadeSettings : ScriptableObject
{
    [Header("Prefabs")]
    public Grenade grenadePrefab;
    public ExplosionEffect explosionPrefab;

    [Header("Detonation")]
    [Tooltip("Sticks to the first surface or target it hits")]
    public bool sticky = true;
    [FormerlySerializedAs("fuseTime"), Tooltip("Seconds from impact until it explodes")]
    public float detonationDelay = 1.5f;
    [Tooltip("Explodes after this time even if it never hits anything")]
    public float maxFlightTime = 4f;

    [Header("Explosion")]
    public float explosionRadius = 4f;
    public int damage = 60;
    [Tooltip("Layers affected by the explosion")]
    public LayerMask damageMask = ~0;

    [Header("Feedback")]
    public SoundEffect explosionSound;
    [Range(0f, 1f)] public float explosionShake = 0.45f;
}
