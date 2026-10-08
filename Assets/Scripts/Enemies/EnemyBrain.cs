using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    [SerializeField] private EnemyDefinition definition;
    [SerializeField] private WeaponView weaponView;
    [SerializeField] private float repathInterval = 0.2f;

    private readonly StateMachine<IState> stateMachine = new();

    private EnemyPerception perception;
    private PatrolRoute patrolRoute;
    private IState idleState;

    public EnemyDefinition Definition => definition;
    public IState CurrentState => stateMachine.CurrentState;

    private void Awake()
    {
        var motor = GetComponent<IEnemyMotor>();
        motor.Configure(
            definition.moveSpeed,
            definition.acceleration,
            definition.attack.attackRange * definition.stoppingRangeFactor
        );

        var context = new EnemyContext(
            transform,
            motor,
            weaponView,
            GetComponent<ITelegraph>(),
            GetComponentInChildren<IAimIndicator>(true),
            GetComponent<IDamageModifierHost>()
        );

        var attack = definition.attack.CreateAttack(context);
        perception = new EnemyPerception(definition.aggroRadius, definition.alertRadius, definition.targetMask, definition.scanInterval);

        BuildStateMachine(motor, attack);
    }

    private void BuildStateMachine(IEnemyMotor motor, IEnemyAttack attack)
    {
        var idle = new EnemyIdleState(motor);
        var patrol = new EnemyPatrolState(motor, () => patrolRoute);
        var chase = new EnemyChaseState(motor, perception, repathInterval);
        var attacking = new EnemyAttackState(attack, perception);

        stateMachine.AddTransition(idle, chase, () => perception.HasTarget);
        stateMachine.AddTransition(idle, patrol, () => patrolRoute != null);
        stateMachine.AddTransition(patrol, chase, () => perception.HasTarget);
        stateMachine.AddTransition(patrol, idle, () => patrolRoute == null);
        stateMachine.AddTransition(chase, idle, () => !perception.HasTarget);
        stateMachine.AddTransition(chase, attacking, () => attack.CanStart(perception.Target));
        stateMachine.AddTransition(attacking, chase, () => !attack.IsBusy);

        idleState = idle;
    }

    public void Alert()
    {
        perception.Alert();
    }

    public void AssignPatrol(PatrolRoute route)
    {
        patrolRoute = route;
    }

    private void OnEnable()
    {
        stateMachine.SetState(idleState);
    }

    private void OnDisable()
    {
        stateMachine.SetState(null);
        perception.Clear();
        patrolRoute = null;
    }

    private void Update()
    {
        perception.Tick(transform.position);
        stateMachine.Tick(Time.deltaTime);
    }
}
