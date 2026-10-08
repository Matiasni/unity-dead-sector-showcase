using UnityEngine;

public interface IKnockbackReceiver
{
    void ApplyKnockback(Vector3 velocity, float duration);
}
