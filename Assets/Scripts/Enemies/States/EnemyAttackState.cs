public class EnemyAttackState : IState
{
    private readonly IEnemyAttack attack;
    private readonly EnemyPerception perception;

    public EnemyAttackState(IEnemyAttack attack, EnemyPerception perception)
    {
        this.attack = attack;
        this.perception = perception;
    }

    public void Enter()
    {
        attack.Begin(perception.Target);
    }

    public void Tick(float deltaTime)
    {
        attack.Tick(perception.HasTarget ? perception.Target : null, deltaTime);
    }

    public void Exit()
    {
        attack.Cancel();
    }
}
