using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIObjectiveMarker : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Image pulse;
    [SerializeField] private RectTransform arrowPivot;
    [SerializeField] private TMP_Text distanceLabel;
    [SerializeField] private float pulseDuration = 1.2f;
    [SerializeField] private float pulseAlpha = 0.6f;

    public void Show(Sprite sprite, Vector2 screenPosition, float distance, bool isOffscreen, float arrowAngle)
    {
        gameObject.SetActive(true);

        transform.position = screenPosition;
        icon.sprite = sprite;
        distanceLabel.text = $"{Mathf.RoundToInt(distance)} m";

        arrowPivot.gameObject.SetActive(isOffscreen);
        arrowPivot.localEulerAngles = new Vector3(0f, 0f, arrowAngle);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        float t = Time.unscaledTime % pulseDuration / pulseDuration;

        pulse.rectTransform.localScale = Vector3.one * Mathf.Lerp(0.7f, 1.3f, t);

        Color color = pulse.color;
        color.a = Mathf.Lerp(pulseAlpha, 0f, t);
        pulse.color = color;
    }
}
