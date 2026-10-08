using System;
using UnityEngine;

public abstract class Objective : MonoBehaviour
{
    [Header("Objective")]
    [SerializeField] private string label = "A";
    [SerializeField] private string title;
    [SerializeField] private RewardDefinition completionReward;
    [SerializeField] private Transform marker;

    private IResourceWallet rewardReceiver;

    public string Label => label;
    public ObjectiveState State { get; private set; } = ObjectiveState.Locked;
    public bool IsCompleted => State == ObjectiveState.Completed;

    public event Action<Objective> OnChanged;

    public void Activate(IResourceWallet rewardReceiver)
    {
        if (State != ObjectiveState.Locked) return;

        this.rewardReceiver = rewardReceiver;
        State = ObjectiveState.Active;

        OnActivated();
        NotifyChanged();
    }

    public ObjectiveStatus GetStatus()
    {
        Vector3 position = marker != null ? marker.position : transform.position;
        return new ObjectiveStatus(label, title, GetProgress(), State, position);
    }

    protected void Complete()
    {
        if (State != ObjectiveState.Active) return;

        State = ObjectiveState.Completed;

        if (completionReward != null && rewardReceiver != null)
            completionReward.GrantTo(rewardReceiver);

        OnCompleted();
        NotifyChanged();
        GameEvents.OnNotification("Objective {0} complete", label);
    }

    protected void NotifyChanged()
    {
        OnChanged?.Invoke(this);
    }

    protected virtual void OnActivated() { }
    protected virtual void OnCompleted() { }
    protected abstract LocalizedMessage GetProgress();
}
