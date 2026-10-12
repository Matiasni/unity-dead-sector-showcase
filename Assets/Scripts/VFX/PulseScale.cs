using UnityEngine;

public class PulseScale : MonoBehaviour
{
    [SerializeField] private float amplitude = 0.04f;
    [SerializeField] private float beatsPerSecond = 0.8f;
    [SerializeField] private float sharpness = 4f;

    private Vector3 baseScale;
    private float phase;

    private void Awake()
    {
        baseScale = transform.localScale;
        phase = Random.value;
    }

    private void Update()
    {
        float cycle = Mathf.Repeat(Time.time * beatsPerSecond + phase, 1f);
        float beat = Mathf.Pow(Mathf.Sin(cycle * Mathf.PI), sharpness);

        transform.localScale = baseScale * (1f + amplitude * beat);
    }
}
