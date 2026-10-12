using UnityEngine;

public class Hover : MonoBehaviour
{
    [SerializeField] private float spinSpeed = 70f;
    [SerializeField] private float bobHeight = 0.18f;
    [SerializeField] private float bobSpeed = 2.2f;

    private Vector3 basePosition;
    private float phase;

    private void Awake()
    {
        basePosition = transform.localPosition;
        phase = Random.Range(0f, Mathf.PI * 2f);
    }

    private void Update()
    {
        float lift = (Mathf.Sin(Time.time * bobSpeed + phase) + 1f) * 0.5f * bobHeight;

        transform.localPosition = basePosition + Vector3.up * lift;
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f, Space.World);
    }
}
