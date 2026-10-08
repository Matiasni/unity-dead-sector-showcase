using UnityEngine;

public interface IEnemyMotor
{
    Vector3 Position { get; }

    void Configure(float speed, float acceleration, float stoppingDistance);
    void MoveTo(Vector3 destination);
    void Stop();
    void Warp(Vector3 position);
    void FaceTowards(Vector3 point, float deltaTime);
    float MoveDirect(Vector3 displacement);
}
