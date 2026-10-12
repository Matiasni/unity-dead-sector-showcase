using UnityEngine;

[RequireComponent(typeof(Light))]
public class PulsingLight : MonoBehaviour
{
    [SerializeField] private float minIntensity = 0.8f;
    [SerializeField] private float maxIntensity = 1.6f;
    [SerializeField] private float frequency = 2f;

    private Light pulsingLight;
    private float phase;

    private void Awake()
    {
        pulsingLight = GetComponent<Light>();
        phase = Random.Range(0f, Mathf.PI * 2f);
    }

    private void Update()
    {
        float t = 0.5f + 0.5f * Mathf.Sin(Time.time * frequency * Mathf.PI * 2f + phase);
        pulsingLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, t);
    }
}
