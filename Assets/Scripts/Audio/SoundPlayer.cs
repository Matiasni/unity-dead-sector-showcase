using UnityEngine;

public static class SoundPlayer
{
    private static AudioOneShot prefab;

    public static void Play(SoundEffect sound, Vector3 position)
    {
        if (sound == null) return;

        var clip = sound.PickClip();

        if (clip == null || !EnsurePrefab()) return;

        var instance = PoolManager.Spawn(prefab, position, Quaternion.identity);
        instance.Play(sound, clip);
    }

    private static bool EnsurePrefab()
    {
        if (prefab != null) return true;

        prefab = Resources.Load<AudioOneShot>("AudioOneShot");
        return prefab != null;
    }
}
