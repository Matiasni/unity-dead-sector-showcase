using UnityEngine;

[RequireComponent(typeof(Grenade))]
public class GrenadeBlink : MonoBehaviour
{
    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private Color blinkColor = new(1f, 0.1f, 0.1f);
    [SerializeField] private float minBlinkRate = 3f;
    [SerializeField] private float maxBlinkRate = 14f;

    private Grenade grenade;
    private MaterialPropertyBlock propertyBlock;
    private Color baseColor;

    private void Awake()
    {
        grenade = GetComponent<Grenade>();
        propertyBlock = new MaterialPropertyBlock();
        baseColor = targetRenderer.sharedMaterial.GetColor(BaseColorId);
    }

    private void OnDisable()
    {
        SetColor(baseColor);
    }

    private void Update()
    {
        float progress = grenade.ArmedProgress;

        if (progress < 0f)
        {
            SetColor(baseColor);
            return;
        }

        float rate = Mathf.Lerp(minBlinkRate, maxBlinkRate, progress);
        bool lit = Mathf.Repeat(Time.time * rate, 1f) < 0.5f;

        SetColor(lit ? blinkColor : baseColor);
    }

    private void SetColor(Color color)
    {
        targetRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor(BaseColorId, color);
        targetRenderer.SetPropertyBlock(propertyBlock);
    }
}
