using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioOneShot : PoolableObject
{
    private AudioSource source;
    private float releaseTime;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
    }

    public void Play(SoundEffect sound, AudioClip clip)
    {
        source.clip = clip;
        source.volume = sound.volume;
        source.pitch = sound.PickPitch();
        source.spatialBlend = sound.spatialBlend;
        source.Play();

        releaseTime = Time.unscaledTime + clip.length / Mathf.Max(0.1f, source.pitch) + 0.1f;
    }

    private void Update()
    {
        if (Time.unscaledTime >= releaseTime)
            Release();
    }
}
