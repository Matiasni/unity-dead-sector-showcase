using UnityEngine;

[RequireComponent(typeof(ForceShield))]
public class ForceShieldView : MonoBehaviour
{
    private const int MaxRipples = 4;

    private static readonly int ColorId = Shader.PropertyToID("_Color");
    private static readonly int FlashId = Shader.PropertyToID("_Flash");
    private static readonly int HitsId = Shader.PropertyToID("_Hits");

    [SerializeField] private Renderer shieldRenderer;
    [SerializeField] private ImpactSurface impactSurface;
    [SerializeField] private Color fullColor = new(0.35f, 0.8f, 1f, 1f);
    [SerializeField] private Color depletedColor = new(1f, 0.3f, 0.22f, 1f);
    [SerializeField, Range(0f, 1f)] private float lowThreshold = 0.3f;
    [SerializeField] private float flashDecay = 6f;

    [Header("Effects")]
    [SerializeField] private PooledEffect shatterEffect;
    [SerializeField] private PooledEffect fadeEffect;

    private readonly Vector4[] ripples = new Vector4[MaxRipples];

    private ForceShield shield;
    private MaterialPropertyBlock propertyBlock;
    private float flash;
    private int nextRipple;

    private void Awake()
    {
        shield = GetComponent<ForceShield>();
        propertyBlock = new MaterialPropertyBlock();
    }

    private void OnEnable()
    {
        shield.OnHit += Flash;
        shield.OnCollapsed += SpawnCollapseEffect;
        impactSurface.OnImpact += AddRipple;

        flash = 1f;

        for (int i = 0; i < MaxRipples; i++)
            ripples[i] = new Vector4(0f, 0f, 0f, -100f);
    }

    private void OnDisable()
    {
        shield.OnHit -= Flash;
        shield.OnCollapsed -= SpawnCollapseEffect;
        impactSurface.OnImpact -= AddRipple;
    }

    private void LateUpdate()
    {
        flash = Mathf.MoveTowards(flash, 0f, flashDecay * Time.deltaTime);

        shieldRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor(ColorId, GetDurabilityColor());
        propertyBlock.SetFloat(FlashId, flash);
        propertyBlock.SetVectorArray(HitsId, ripples);
        shieldRenderer.SetPropertyBlock(propertyBlock);
    }

    private Color GetDurabilityColor()
    {
        float durability = shield.DurabilityNormalized;
        Color color = Color.Lerp(depletedColor, fullColor, durability);

        if (durability <= lowThreshold)
            color.a *= 0.45f + 0.55f * Mathf.Abs(Mathf.Sin(Time.time * 14f));

        return color;
    }

    private void Flash()
    {
        flash = 1f;
    }

    private void AddRipple(Vector3 point)
    {
        ripples[nextRipple] = new Vector4(point.x, point.y, point.z, Time.timeSinceLevelLoad);
        nextRipple = (nextRipple + 1) % MaxRipples;
    }

    private void SpawnCollapseEffect(ForceShield collapsed)
    {
        var effect = collapsed.DurabilityNormalized <= 0f ? shatterEffect : fadeEffect;

        if (effect == null) return;

        var spawned = PoolManager.Spawn(effect, transform.position, Quaternion.identity);
        spawned.SetScale(transform.lossyScale.x * 0.5f);
    }
}
