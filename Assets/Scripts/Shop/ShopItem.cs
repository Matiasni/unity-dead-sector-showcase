using UnityEngine;

public abstract class ShopItem : ScriptableObject
{
    [Header("Shop")]
    public string displayName;
    public Color displayColor = Color.white;

    [Header("Price")]
    public ResourceDefinition currency;
    [Min(0)] public int price;

    public virtual PoolableObject DisplayPrefab => null;

    public abstract bool CanGrant(GameObject buyer, out LocalizedMessage reason);
    public abstract void Grant(GameObject buyer);
}
