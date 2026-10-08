using UnityEngine;

[RequireComponent(typeof(TextMesh))]
public class DamageNumber : PoolableObject
{
    [SerializeField] private float lifeTime = 0.8f;
    [SerializeField] private float riseSpeed = 2.2f;
    [SerializeField] private float spread = 0.6f;

    private TextMesh textMesh;
    private Camera viewCamera;

    private Color baseColor;
    private Vector3 drift;
    private float elapsed;

    private void Awake()
    {
        textMesh = GetComponent<TextMesh>();
    }

    public void Show(int amount, Color color, float scale)
    {
        textMesh.text = amount.ToString();
        baseColor = color;
        textMesh.color = color;

        transform.localScale = Vector3.one * scale;
        drift = new Vector3(Random.Range(-spread, spread), riseSpeed, 0f);
        elapsed = 0f;
    }

    private void LateUpdate()
    {
        elapsed += Time.deltaTime;

        if (elapsed >= lifeTime)
        {
            Release();
            return;
        }

        if (viewCamera == null)
            viewCamera = Camera.main;

        transform.position += drift * Time.deltaTime;

        if (viewCamera != null)
            transform.rotation = viewCamera.transform.rotation;

        Color color = baseColor;
        color.a = 1f - elapsed / lifeTime;
        textMesh.color = color;
    }
}
