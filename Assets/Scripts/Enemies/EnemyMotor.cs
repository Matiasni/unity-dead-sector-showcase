using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMotor : MonoBehaviour, IEnemyMotor
{
    [SerializeField] private float turnSpeed = 10f;

    private NavMeshAgent agent;

    public Vector3 Position => transform.position;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void Configure(float speed, float acceleration, float stoppingDistance)
    {
        agent.speed = speed;
        agent.acceleration = acceleration;
        agent.stoppingDistance = stoppingDistance;
    }

    public void MoveTo(Vector3 destination)
    {
        if (!agent.isOnNavMesh) return;

        agent.isStopped = false;
        agent.SetDestination(destination);
    }

    public void Stop()
    {
        if (!agent.isOnNavMesh) return;

        agent.isStopped = true;
        agent.ResetPath();
    }

    public void Warp(Vector3 position)
    {
        if (NavMesh.SamplePosition(position, out NavMeshHit hit, 3f, NavMesh.AllAreas))
            agent.Warp(hit.position);
    }

    public void FaceTowards(Vector3 point, float deltaTime)
    {
        Vector3 direction = point - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f) return;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(direction),
            turnSpeed * deltaTime
        );
    }

    public float MoveDirect(Vector3 displacement)
    {
        if (!agent.isOnNavMesh) return 0f;

        Vector3 before = transform.position;
        agent.Move(displacement);

        Vector3 moved = transform.position - before;
        moved.y = 0f;
        return moved.magnitude;
    }
}
