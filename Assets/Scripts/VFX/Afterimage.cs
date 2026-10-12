using UnityEngine;

public class Afterimage : PoolableObject
{
    private static readonly int ColorId = Shader.PropertyToID("_Color");

    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private float lifetime = 0.25f;

    private MaterialPropertyBlock propertyBlock;
    private Color baseColor;
    private float elapsed;

    private void Awake()
    {
        propertyBlock = new MaterialPropertyBlock();
        baseColor = meshRenderer.sharedMaterial.GetColor(ColorId);
    }

    public void Show(Mesh mesh, Vector3 scale)
    {
        meshFilter.sharedMesh = mesh;
        transform.localScale = scale;
        elapsed = 0f;
        Apply(1f);
    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        float remaining = 1f - Mathf.Clamp01(elapsed / lifetime);

        Apply(remaining);

        if (remaining <= 0f)
            Release();
    }

    private void Apply(float strength)
    {
        Color color = baseColor;
        color.a *= strength * strength;

        meshRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor(ColorId, color);
        meshRenderer.SetPropertyBlock(propertyBlock);
    }
}
