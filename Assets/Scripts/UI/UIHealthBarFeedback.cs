using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class UIHealthBarFeedback : MonoBehaviour
{
    [SerializeField] private Image fill;
    [SerializeField] private RectTransform trail;
    [SerializeField] private Sprite normalFill;
    [SerializeField] private Sprite dangerFill;
    [SerializeField, Range(0f, 1f)] private float dangerThreshold = 0.3f;
    [SerializeField] private float trailDelay = 0.4f;
    [SerializeField] private float trailSpeed = 0.8f;

    private Slider slider;
    private float trailValue = 1f;
    private float trailHoldUntil;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(HandleValueChanged);

        trailValue = slider.normalizedValue;
        ApplyTrail();
    }

    private void OnDestroy()
    {
        slider.onValueChanged.RemoveListener(HandleValueChanged);
    }

    private void HandleValueChanged(float value)
    {
        float normalized = slider.normalizedValue;
        fill.sprite = normalized < dangerThreshold ? dangerFill : normalFill;

        if (normalized < trailValue)
            trailHoldUntil = Time.time + trailDelay;
        else
            trailValue = normalized;

        ApplyTrail();
    }

    private void Update()
    {
        float normalized = slider.normalizedValue;

        if (trailValue <= normalized || Time.time < trailHoldUntil) return;

        trailValue = Mathf.MoveTowards(trailValue, normalized, trailSpeed * Time.deltaTime);
        ApplyTrail();
    }

    private void ApplyTrail()
    {
        trail.anchorMax = new Vector2(trailValue, trail.anchorMax.y);
    }
}
