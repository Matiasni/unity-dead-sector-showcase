using UnityEngine;

public interface IDashMotor
{
    Vector3 MoveDirection { get; }
    void Dash(Vector3 direction, float speed, float duration);
}
