using System;

public class MissionEndState : IState
{
    private readonly Action onEnter;

    public MissionEndState(Action onEnter)
    {
        this.onEnter = onEnter;
    }

    public void Enter()
    {
        onEnter();
    }

    public void Tick(float deltaTime) { }

    public void Exit() { }
}
