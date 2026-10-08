using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float maxOffset = 0.6f;
    [SerializeField] private float maxRotation = 2.5f;
    [SerializeField] private float frequency = 22f;
    [SerializeField] private float traumaDecay = 1.6f;

    private Vector3 restPosition;
    private Quaternion restRotation;
    private float trauma;
    private float seed;

    private void Awake()
    {
        restPosition = transform.localPosition;
        restRotation = transform.localRotation;
        seed = Random.value * 100f;
    }

    private void OnEnable()
    {
        GameEvents.onCameraShake += AddTrauma;
    }

    private void OnDisable()
    {
        GameEvents.onCameraShake -= AddTrauma;
    }

    public void AddTrauma(float amount)
    {
        trauma = Mathf.Clamp01(trauma + amount);
    }

    private void LateUpdate()
    {
        if (trauma <= 0f)
        {
            transform.localPosition = restPosition;
            transform.localRotation = restRotation;
            return;
        }

        float shake = trauma * trauma;
        float time = Time.time * frequency;

        Vector3 offset = new Vector3(Noise(time, 0f), Noise(time, 1f), 0f) * (maxOffset * shake);
        float roll = Noise(time, 2f) * maxRotation * shake;

        transform.localPosition = restPosition + offset;
        transform.localRotation = restRotation * Quaternion.Euler(0f, 0f, roll);

        trauma = Mathf.Max(0f, trauma - traumaDecay * Time.deltaTime);
    }

    private float Noise(float time, float channel)
    {
        return Mathf.PerlinNoise(seed + channel * 10f, time) * 2f - 1f;
    }
}
