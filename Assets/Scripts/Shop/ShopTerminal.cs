using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ShopTerminal : MonoBehaviour, IInteractable
{
    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

    [SerializeField] private ShopItem item;
    [SerializeField] private Transform displayPoint;
    [SerializeField] private Renderer iconRenderer;

    [Header("Pricing")]
    [SerializeField, Min(0f), Tooltip("Price increase after each purchase (0.1 = +10%)")]
    private float priceIncreasePerPurchase;

    private int purchaseCount;

    public int CurrentPrice => Mathf.RoundToInt(item.price * Mathf.Pow(1f + priceIncreasePerPurchase, purchaseCount));

    public LocalizedMessage InteractionPrompt => new("[F] Buy {0} - {1} {2}", item.displayName, CurrentPrice, item.currency.displayName);

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void Start()
    {
        SetupDisplay();
    }

    public void Interact(GameObject interactor)
    {
        if (!interactor.TryGetComponent(out IResourceWallet wallet)) return;

        if (!item.CanGrant(interactor, out LocalizedMessage reason))
        {
            GameEvents.OnNotification(reason.Key, reason.Args);
            return;
        }

        if (!wallet.TrySpend(item.currency, CurrentPrice))
        {
            GameEvents.OnNotification("Not enough {0}", item.currency.displayName);
            return;
        }

        item.Grant(interactor);
        purchaseCount++;

        GameEvents.OnNotification("Bought {0}", item.displayName);
        GameEvents.OnInteractionPromptChanged(InteractionPrompt);
    }

    private void SetupDisplay()
    {
        if (displayPoint != null && item.DisplayPrefab != null)
            PoolManager.Spawn(item.DisplayPrefab, displayPoint);

        if (iconRenderer == null) return;

        iconRenderer.enabled = item.DisplayPrefab == null;

        var propertyBlock = new MaterialPropertyBlock();
        iconRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor(BaseColorId, item.displayColor);
        iconRenderer.SetPropertyBlock(propertyBlock);
    }
}
