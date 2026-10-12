using UnityEngine;

[RequireComponent(typeof(Light))]
public class EffectLight : MonoBehaviour
{
    [SerializeField] private float duration = 0.1f;

    private Light flash;
    private float peakIntensity;
    private float elapsed;

    private void Awake()
    {
        flash = GetComponent<Light>();
        peakIntensity = flash.intensity;
    }

    private void OnEnable()
    {
        elapsed = 0f;
        flash.intensity = peakIntensity;
        flash.enabled = true;
    }

    private void Update()
    {
        if (!flash.enabled) return;

        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / duration);

        flash.intensity = peakIntensity * (1f - t);

        if (t >= 1f)
            flash.enabled = false;
    }
}
