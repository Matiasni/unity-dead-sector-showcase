using System.Collections.Generic;
using UnityEngine;

public abstract class ShotPattern : ScriptableObject
{
    public abstract void GetDirections(Vector3 forward, List<Vector3> results);

    protected static Vector3 RotateHorizontally(Vector3 direction, float angle)
    {
        return Quaternion.AngleAxis(angle, Vector3.up) * direction;
    }
}
