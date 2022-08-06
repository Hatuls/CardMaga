using System;
using System.Collections.Generic;
using UnityEngine;

public enum RelicIcons
{
    QuadPunch = 0,
}
[CreateAssetMenu(fileName = "RelicIconCollectionOS", menuName = "ScriptableObjects/UI/RelicIconCollection")]
public class RelicIconsCollectionSO : ScriptableObject
{
    #region Fields
    Dictionary<RelicIcons,Sprite> _relicIconsSprite = new Dictionary<RelicIcons, Sprite>();
    [SerializeField]
    [Tooltip("0 QuadPunch\n")]
    Sprite[] _relicSprites;
    #endregion
    private void OnEnable()
    {
        if (_relicSprites.Length == 0)
        {
            Debug.LogError("_RelicSprites is 0");
            return;
        }
        if(_relicSprites.Length == _relicIconsSprite.Count)
        {
            return;
        }
        foreach (var item in (RelicIcons[])Enum.GetValues(typeof(RelicIcons)))
        {
            if (!_relicIconsSprite.ContainsKey(item))
            {
                _relicIconsSprite.Add(item, _relicSprites[(int)item]);
            }
        }
        
    }
    public Sprite GetSprite(RelicIcons iconEnum)
    {
        if (_relicSprites == null)
        {
            Debug.LogError("Error in relics GetSprite");
            return null;
        }
        return _relicIconsSprite[iconEnum];
    }
}
