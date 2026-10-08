using System;

[Serializable]
public struct ResourceAmount
{
    public ResourceDefinition resource;
    public int amount;

    public ResourceAmount(ResourceDefinition resource, int amount)
    {
        this.resource = resource;
        this.amount = amount;
    }
}
