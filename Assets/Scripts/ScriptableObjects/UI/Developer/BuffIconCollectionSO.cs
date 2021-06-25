using System;
using System.Collections.Generic;
using UnityEngine;
public enum BuffIcons
{
    Bleed = 0,
    Strength = 1
}
[CreateAssetMenu(fileName = "BuffIconCollectionOS", menuName = "ScriptableObjects/UI/BuffIconCollection")]
public class BuffIconCollectionSO : ScriptableObject
{
    #region Fields
    Dictionary<BuffIcons, UIIconSO> _buffIconData = new Dictionary<BuffIcons, UIIconSO>();
    [SerializeField]
    [Tooltip("0 Bleed\n 1 Strength")]
    UIIconSO[] _buffUIIcons;
    #endregion
    private void OnEnable()
    {
        if(_buffUIIcons.Length == 0)
        {
            Debug.LogError("_buffSprites is 0");
            return;
        }
        if(_buffUIIcons.Length == _buffIconData.Count)
        {
            return;
        }
        foreach (var item in (BuffIcons[])Enum.GetValues(typeof(BuffIcons)))
        {
            if(!_buffIconData.ContainsKey(item))
            {
                _buffIconData.Add(item, _buffUIIcons[(int)item]);
            }
        }
    }
    public UIIconSO GetIconData(BuffIcons iconEnum)
    {
        if(_buffUIIcons == null)
        {
            Debug.LogError("Error in Buffs GetSprite");
            return null;
        }
        return _buffIconData[iconEnum];
    }
}
