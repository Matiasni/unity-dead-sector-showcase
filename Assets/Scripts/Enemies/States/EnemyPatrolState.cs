using System;
using UnityEngine;

public class EnemyPatrolState : IState
{
    private const float ArrivalDistance = 1.2f;

    private readonly IEnemyMotor motor;
    private readonly Func<PatrolRoute> routeProvider;

    private int waypointIndex;

    public EnemyPatrolState(IEnemyMotor motor, Func<PatrolRoute> routeProvider)
    {
        this.motor = motor;
        this.routeProvider = routeProvider;
    }

    public void Enter()
    {
        MoveToCurrentWaypoint();
    }

    public void Tick(float deltaTime)
    {
        var route = routeProvider();

        if (route == null || route.Count == 0) return;

        Vector3 offset = route.GetPoint(waypointIndex) - motor.Position;
        offset.y = 0f;

        if (offset.magnitude > ArrivalDistance) return;

        waypointIndex = (waypointIndex + 1) % route.Count;
        MoveToCurrentWaypoint();
    }

    public void Exit()
    {
        motor.Stop();
    }

    private void MoveToCurrentWaypoint()
    {
        var route = routeProvider();

        if (route != null && route.Count > 0)
            motor.MoveTo(route.GetPoint(waypointIndex));
    }
}
