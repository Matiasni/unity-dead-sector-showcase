using UnityEngine;

public class EnemyChaseState : IState
{
    private readonly IEnemyMotor motor;
    private readonly EnemyPerception perception;
    private readonly float repathInterval;

    private float nextRepathTime;

    public EnemyChaseState(IEnemyMotor motor, EnemyPerception perception, float repathInterval)
    {
        this.motor = motor;
        this.perception = perception;
        this.repathInterval = repathInterval;
    }

    public void Enter()
    {
        nextRepathTime = 0f;
    }

    public void Tick(float deltaTime)
    {
        if (!perception.HasTarget || Time.time < nextRepathTime) return;

        nextRepathTime = Time.time + repathInterval;
        motor.MoveTo(perception.Target.position);
    }

    public void Exit()
    {
        motor.Stop();
    }
}
