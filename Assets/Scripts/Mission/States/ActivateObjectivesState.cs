public class ActivateObjectivesState : IState
{
    private readonly Objective[] objectives;
    private readonly IResourceWallet rewardReceiver;

    public ActivateObjectivesState(IResourceWallet rewardReceiver, params Objective[] objectives)
    {
        this.rewardReceiver = rewardReceiver;
        this.objectives = objectives;
    }

    public void Enter()
    {
        foreach (var objective in objectives)
            objective.Activate(rewardReceiver);
    }

    public void Tick(float deltaTime) { }

    public void Exit() { }
}
