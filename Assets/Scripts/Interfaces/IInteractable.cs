using UnityEngine;

public interface IInteractable
{
    LocalizedMessage InteractionPrompt { get; }
    void Interact(GameObject interactor);
}
