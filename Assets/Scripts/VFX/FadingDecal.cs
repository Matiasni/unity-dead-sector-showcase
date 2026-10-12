using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class FadingDecal : MonoBehaviour
{
    private static readonly int ColorId = Shader.PropertyToID("_Color");

    [SerializeField] private float growTime;
    [SerializeField] private float holdTime = 1f;
    [SerializeField] private float fadeTime = 1f;
    [SerializeField] private bool randomSpin = true;

    private Renderer decalRenderer;
    private MaterialPropertyBlock propertyBlock;
    private Color baseColor;
    private Quaternion baseRotation;
    private Vector3 baseScale;
    private float elapsed;

    private void Awake()
    {
        decalRenderer = GetComponent<Renderer>();
        propertyBlock = new MaterialPropertyBlock();
        baseColor = decalRenderer.sharedMaterial.GetColor(ColorId);
        baseRotation = transform.localRotation;
        baseScale = transform.localScale;
    }

    private void OnEnable()
    {
        elapsed = 0f;

        if (randomSpin)
            transform.localRotation = baseRotation * Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

        Apply();
    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        Apply();
    }

    private void Apply()
    {
        float grow = growTime > 0f ? Mathf.Clamp01(elapsed / growTime) : 1f;
        transform.localScale = baseScale * Mathf.Lerp(0.3f, 1f, 1f - (1f - grow) * (1f - grow));

        float fade = fadeTime > 0f ? Mathf.Clamp01((elapsed - holdTime) / fadeTime) : 0f;

        Color color = baseColor;
        color.a *= 1f - fade;

        decalRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor(ColorId, color);
        decalRenderer.SetPropertyBlock(propertyBlock);
    }
}
