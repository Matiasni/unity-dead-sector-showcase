using UnityEngine;

public class MuzzleFlame : MonoBehaviour
{
    private static readonly int MainTexStId = Shader.PropertyToID("_MainTex_ST");

    [SerializeField] private Renderer[] flames;
    [SerializeField] private int columns = 2;
    [SerializeField] private int rows = 2;
    [SerializeField] private float duration = 0.05f;
    [SerializeField] private Vector2 lengthRange = new(0.8f, 1.2f);

    private MaterialPropertyBlock propertyBlock;
    private Vector3[] baseScales;
    private Vector3[] basePositions;
    private float elapsed;

    private void Awake()
    {
        propertyBlock = new MaterialPropertyBlock();
        baseScales = new Vector3[flames.Length];
        basePositions = new Vector3[flames.Length];

        for (int i = 0; i < flames.Length; i++)
        {
            baseScales[i] = flames[i].transform.localScale;
            basePositions[i] = flames[i].transform.localPosition;
        }
    }

    private void OnEnable()
    {
        elapsed = 0f;
        transform.localRotation = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward);

        float length = Random.Range(lengthRange.x, lengthRange.y);

        for (int i = 0; i < flames.Length; i++)
        {
            int frame = Random.Range(0, columns * rows);
            var tiling = new Vector4(1f / columns, 1f / rows, (float)(frame % columns) / columns, (float)(frame / columns) / rows);

            flames[i].GetPropertyBlock(propertyBlock);
            propertyBlock.SetVector(MainTexStId, tiling);
            flames[i].SetPropertyBlock(propertyBlock);

            var flameTransform = flames[i].transform;
            flameTransform.localScale = new Vector3(baseScales[i].x * length, baseScales[i].y, baseScales[i].z);
            flameTransform.localPosition = basePositions[i] * length;

            flames[i].enabled = true;
        }
    }

    private void Update()
    {
        elapsed += Time.deltaTime;

        if (elapsed < duration) return;

        foreach (var flame in flames)
            flame.enabled = false;
    }
}
