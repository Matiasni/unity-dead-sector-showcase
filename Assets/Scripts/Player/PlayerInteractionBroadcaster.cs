using UnityEngine;

[RequireComponent(typeof(PlayerInteractor))]
public class PlayerInteractionBroadcaster : MonoBehaviour
{
    private PlayerInteractor interactor;

    private void Awake()
    {
        interactor = GetComponent<PlayerInteractor>();
    }

    private void OnEnable()
    {
        interactor.OnFocusChanged += Broadcast;
    }

    private void OnDisable()
    {
        interactor.OnFocusChanged -= Broadcast;
    }

    private void Broadcast(IInteractable interactable)
    {
        GameEvents.OnInteractionPromptChanged(interactable != null ? interactable.InteractionPrompt : LocalizedMessage.Empty);
    }
}
