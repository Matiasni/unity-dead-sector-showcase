using UnityEngine;

public class InteractObjective : Objective, IInteractable
{
    [Header("Interaction")]
    [SerializeField] private string actionName = "Recover the cache";
    [SerializeField] private GameObject completedVisual;

    public LocalizedMessage InteractionPrompt => State == ObjectiveState.Active ? new LocalizedMessage("[F] {0}", actionName) : LocalizedMessage.Empty;

    public void Interact(GameObject interactor)
    {
        if (State != ObjectiveState.Active) return;

        Complete();
    }

    protected override void OnCompleted()
    {
        if (completedVisual != null)
            completedVisual.SetActive(false);
    }

    protected override LocalizedMessage GetProgress() => new(State == ObjectiveState.Completed ? "Recovered" : actionName);
}
