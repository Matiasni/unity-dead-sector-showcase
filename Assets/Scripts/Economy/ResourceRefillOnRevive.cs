using UnityEngine;

[RequireComponent(typeof(HealthBehaviour))]
public class ResourceRefillOnRevive : MonoBehaviour
{
    [SerializeField, Tooltip("Resources refilled to full capacity when revived")]
    private ResourceDefinition[] resources;

    private HealthBehaviour health;
    private IResourceWallet wallet;

    private void Awake()
    {
        health = GetComponent<HealthBehaviour>();
        wallet = GetComponent<IResourceWallet>();
    }

    private void OnEnable()
    {
        health.OnRevived += Refill;
    }

    private void OnDisable()
    {
        health.OnRevived -= Refill;
    }

    private void Refill()
    {
        if (wallet == null) return;

        foreach (var resource in resources)
            wallet.Add(resource, wallet.GetCapacity(resource) - wallet.Get(resource));
    }
}
