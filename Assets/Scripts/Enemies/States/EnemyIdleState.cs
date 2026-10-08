public class EnemyIdleState : IState
{
    private readonly IEnemyMotor motor;

    public EnemyIdleState(IEnemyMotor motor)
    {
        this.motor = motor;
    }

    public void Enter()
    {
        motor.Stop();
    }

    public void Tick(float deltaTime) { }

    public void Exit() { }
}
