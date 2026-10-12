using UnityEngine;

public interface IDashMotor
{
    Vector3 MoveDirection { get; }
    bool IsDashing { get; }
    void Dash(Vector3 direction, float speed, float duration);
}
