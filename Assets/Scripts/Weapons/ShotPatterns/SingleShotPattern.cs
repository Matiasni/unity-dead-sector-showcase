using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Weapons/Shot Patterns/Single Shot")]
public class SingleShotPattern : ShotPattern
{
    [Range(0f, 45f), Tooltip("Max random deviation in degrees")]
    [SerializeField] private float spreadAngle;

    public override void GetDirections(Vector3 forward, List<Vector3> results)
    {
        results.Add(RotateHorizontally(forward, Random.Range(-spreadAngle, spreadAngle)));
    }
}
