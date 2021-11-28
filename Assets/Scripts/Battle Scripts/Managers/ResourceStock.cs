using UnityEngine;

[System.Serializable]
public class ResourceStock
{
    [SerializeField]
    Rewards.ResourceEnum _resourceEnum;
    [SerializeField]
    ushort _price;

    public ResourceStock(Rewards.ResourceEnum resourceEnum, ushort price)
    {
        _resourceEnum = resourceEnum;
        _price = price;
    }

    public Rewards.ResourceEnum ResourceEnum { get => _resourceEnum; }
    public ushort Price { get => _price; }
}