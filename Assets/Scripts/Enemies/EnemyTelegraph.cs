using UnityEngine;

public class EnemyTelegraph : MonoBehaviour, ITelegraph
{
    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

    [SerializeField] private Renderer indicator;

    private MaterialPropertyBlock propertyBlock;

    private void Awake()
    {
        propertyBlock = new MaterialPropertyBlock();
        indicator.enabled = false;
    }

    private void OnDisable()
    {
        Hide();
    }

    public void Show(Color color)
    {
        indicator.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor(BaseColorId, color);
        indicator.SetPropertyBlock(propertyBlock);

        indicator.enabled = true;
    }

    public void Hide()
    {
        indicator.enabled = false;
    }
}
