using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

    [SerializeField] private HealthBehaviour health;
    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private float duration = 0.1f;

    private MaterialPropertyBlock propertyBlock;
    private Color originalColor;
    private float flashEndTime;
    private bool isFlashing;

    private void Awake()
    {
        propertyBlock = new MaterialPropertyBlock();
        originalColor = targetRenderer.sharedMaterial.GetColor(BaseColorId);
    }

    private void OnEnable()
    {
        health.OnDamaged += Flash;
    }

    private void OnDisable()
    {
        health.OnDamaged -= Flash;

        if (isFlashing)
        {
            isFlashing = false;
            SetColor(originalColor);
        }
    }

    private void Update()
    {
        if (!isFlashing || Time.time < flashEndTime) return;

        isFlashing = false;
        SetColor(originalColor);
    }

    private void Flash(int amount)
    {
        isFlashing = true;
        flashEndTime = Time.time + duration;
        SetColor(flashColor);
    }

    private void SetColor(Color color)
    {
        targetRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor(BaseColorId, color);
        targetRenderer.SetPropertyBlock(propertyBlock);
    }
}
