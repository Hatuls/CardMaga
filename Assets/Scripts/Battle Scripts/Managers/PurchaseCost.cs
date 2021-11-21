using UnityEngine;

[System.Serializable]
public class PurchaseCost
{
    [SerializeField]
    Rewards.ResourceEnum _resourceEnum;
    [SerializeField]
    ushort _price;

    public PurchaseCost(Rewards.ResourceEnum resourceEnum, ushort price)
    {
        _resourceEnum = resourceEnum;
        _price = price;
    }

    public Rewards.ResourceEnum ResourceEnum { get => _resourceEnum; }
    public ushort Price { get => _price; }
}