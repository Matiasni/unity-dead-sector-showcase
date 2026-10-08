using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Weapons/Shot Patterns/Cone Shot")]
public class ConeShotPattern : ShotPattern
{
    [Min(1), Tooltip("Projectiles per shot")]
    [SerializeField] private int pellets = 8;
    [Range(0f, 90f), Tooltip("Total cone width in degrees")]
    [SerializeField] private float coneAngle = 30f;
    [Range(0f, 10f), Tooltip("Random deviation applied to each pellet")]
    [SerializeField] private float jitter = 2f;

    public override void GetDirections(Vector3 forward, List<Vector3> results)
    {
        float halfCone = coneAngle * 0.5f;

        for (int i = 0; i < pellets; i++)
        {
            float t = pellets == 1 ? 0.5f : (float)i / (pellets - 1);
            float angle = Mathf.Lerp(-halfCone, halfCone, t) + Random.Range(-jitter, jitter);

            results.Add(RotateHorizontally(forward, angle));
        }
    }
}
