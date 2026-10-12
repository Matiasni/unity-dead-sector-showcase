using UnityEngine;

public interface IChargeFeedback
{
    void ShowWindup();
    void ShowCharge();
    void ShowImpact(Vector3 point);
    void Stop();
}
