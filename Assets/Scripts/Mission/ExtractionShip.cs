using UnityEngine;

public class ExtractionShip : MonoBehaviour
{
    [SerializeField] private Transform visual;
    [SerializeField] private float approachHeight = 40f;
    [SerializeField] private float landDuration = 4f;

    private Vector3 landedLocalPosition;
    private float elapsed;
    private bool isLanding;

    public bool IsLanded { get; private set; }

    private void Awake()
    {
        landedLocalPosition = visual.localPosition;
        visual.gameObject.SetActive(false);
    }

    public void Land()
    {
        elapsed = 0f;
        isLanding = true;
        IsLanded = false;

        visual.gameObject.SetActive(true);
        UpdateVisual(0f);
    }

    private void Update()
    {
        if (!isLanding) return;

        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / landDuration);

        UpdateVisual(t);

        if (t < 1f) return;

        isLanding = false;
        IsLanded = true;
    }

    private void UpdateVisual(float t)
    {
        float eased = 1f - (1f - t) * (1f - t);
        visual.localPosition = landedLocalPosition + Vector3.up * (approachHeight * (1f - eased));
    }
}
