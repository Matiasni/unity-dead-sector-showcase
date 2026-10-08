using UnityEngine;

public interface IEnemyAttack
{
    float Range { get; }
    bool IsBusy { get; }

    bool CanStart(Transform target);
    void Begin(Transform target);
    void Tick(Transform target, float deltaTime);
    void Cancel();
}
