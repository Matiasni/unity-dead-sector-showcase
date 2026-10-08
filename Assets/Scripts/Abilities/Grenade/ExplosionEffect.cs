using UnityEngine;

public class ExplosionEffect : PoolableObject
{
    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private float duration = 0.35f;

    private MaterialPropertyBlock propertyBlock;
    private Color baseColor;

    private float diameter;
    private float elapsed;

    private void Awake()
    {
        propertyBlock = new MaterialPropertyBlock();
        baseColor = targetRenderer.sharedMaterial.GetColor(BaseColorId);
    }

    public void Play(float radius)
    {
        diameter = radius * 2f;
        elapsed = 0f;
        UpdateVisual(0f);
    }

    private void Update()
    {
        elapsed += Time.deltaTime;

        float t = Mathf.Clamp01(elapsed / duration);
        UpdateVisual(t);

        if (t >= 1f)
            Release();
    }

    private void UpdateVisual(float t)
    {
        float eased = 1f - (1f - t) * (1f - t);
        transform.localScale = Vector3.one * Mathf.Lerp(0.2f, diameter, eased);

        Color color = baseColor;
        color.a = baseColor.a * (1f - t);

        targetRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor(BaseColorId, color);
        targetRenderer.SetPropertyBlock(propertyBlock);
    }
}
