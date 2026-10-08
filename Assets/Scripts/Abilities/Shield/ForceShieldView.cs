using UnityEngine;

[RequireComponent(typeof(ForceShield))]
public class ForceShieldView : MonoBehaviour
{
    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

    [SerializeField] private Renderer shieldRenderer;
    [SerializeField] private Color fullColor = new(0.3f, 0.8f, 1f, 0.25f);
    [SerializeField] private Color depletedColor = new(1f, 0.25f, 0.2f, 0.35f);
    [SerializeField] private Color hitColor = new(1f, 1f, 1f, 0.55f);
    [SerializeField] private float hitFlashDuration = 0.08f;
    [SerializeField, Range(0f, 1f)] private float lowThreshold = 0.3f;

    private ForceShield shield;
    private MaterialPropertyBlock propertyBlock;
    private float hitFlashEndTime;

    private void Awake()
    {
        shield = GetComponent<ForceShield>();
        propertyBlock = new MaterialPropertyBlock();
    }

    private void OnEnable()
    {
        shield.OnHit += Flash;
    }

    private void OnDisable()
    {
        shield.OnHit -= Flash;
    }

    private void LateUpdate()
    {
        SetColor(Time.time < hitFlashEndTime ? hitColor : GetDurabilityColor());
    }

    private Color GetDurabilityColor()
    {
        float durability = shield.DurabilityNormalized;
        Color color = Color.Lerp(depletedColor, fullColor, durability);

        if (durability <= lowThreshold)
            color.a *= 0.5f + 0.5f * Mathf.Abs(Mathf.Sin(Time.time * 12f));

        return color;
    }

    private void Flash()
    {
        hitFlashEndTime = Time.time + hitFlashDuration;
    }

    private void SetColor(Color color)
    {
        shieldRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor(BaseColorId, color);
        shieldRenderer.SetPropertyBlock(propertyBlock);
    }
}
