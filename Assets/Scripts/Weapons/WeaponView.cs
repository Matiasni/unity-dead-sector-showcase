using UnityEngine;

public class WeaponView : PoolableObject
{
    [SerializeField] private Transform muzzle;

    public Transform Muzzle => muzzle;
}
