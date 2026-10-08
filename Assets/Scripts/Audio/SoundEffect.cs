using UnityEngine;

[CreateAssetMenu(menuName = "Game/Audio/Sound Effect")]
public class SoundEffect : ScriptableObject
{
    public AudioClip[] clips;
    [Range(0f, 1f)] public float volume = 0.8f;
    public Vector2 pitchRange = new(0.95f, 1.05f);
    [Range(0f, 1f), Tooltip("0 = 2D, 1 = fully positional")]
    public float spatialBlend = 0.4f;

    public AudioClip PickClip() => clips != null && clips.Length > 0 ? clips[Random.Range(0, clips.Length)] : null;

    public float PickPitch() => Random.Range(pitchRange.x, pitchRange.y);
}
