using UnityEngine;

public class MusicDirector : MonoBehaviour
{
    [SerializeField] private AudioSource calmLayer;
    [SerializeField] private AudioSource intenseLayer;
    [SerializeField, Range(0f, 1f)] private float masterVolume = 0.45f;
    [SerializeField] private float fadeSpeed = 0.5f;

    private float targetIntensity;
    private float intensity;

    private void OnEnable()
    {
        GameEvents.onMusicIntensityChanged += SetIntensity;
    }

    private void OnDisable()
    {
        GameEvents.onMusicIntensityChanged -= SetIntensity;
    }

    private void Start()
    {
        calmLayer.loop = true;
        intenseLayer.loop = true;
        calmLayer.Play();
        intenseLayer.Play();
        ApplyVolumes();
    }

    public void SetIntensity(float value)
    {
        targetIntensity = Mathf.Clamp01(value);
    }

    private void Update()
    {
        intensity = Mathf.MoveTowards(intensity, targetIntensity, fadeSpeed * Time.unscaledDeltaTime);
        ApplyVolumes();
    }

    private void ApplyVolumes()
    {
        calmLayer.volume = masterVolume * (1f - intensity * 0.4f);
        intenseLayer.volume = masterVolume * intensity;
    }
}
