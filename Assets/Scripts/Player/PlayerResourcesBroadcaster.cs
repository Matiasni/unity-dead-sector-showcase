using UnityEngine;

public class PlayerResourcesBroadcaster : MonoBehaviour
{
    private IResourceWallet wallet;

    private void Awake()
    {
        wallet = GetComponent<IResourceWallet>();
    }

    private void OnEnable()
    {
        wallet.OnResourceChanged += GameEvents.OnPlayerResourceChanged;
    }

    private void OnDisable()
    {
        wallet.OnResourceChanged -= GameEvents.OnPlayerResourceChanged;
    }
}
