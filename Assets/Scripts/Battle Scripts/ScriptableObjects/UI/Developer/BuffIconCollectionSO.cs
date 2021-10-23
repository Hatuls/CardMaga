using System;

using UnityEngine;
using Keywords;

[CreateAssetMenu(fileName = "BuffIconCollectionOS", menuName = "ScriptableObjects/UI/BuffIconCollection")]
public class BuffIconCollectionSO : ScriptableObject
{
    #region Fields

    [SerializeField]
    [Tooltip("0 Bleed\n 1 Strength")]
    UIIconSO[] _buffUIIcons;
    #endregion

    public UIIconSO GetIconData(KeywordTypeEnum iconEnum)
    {
        UIIconSO uIIconSO = null;
        for (int i = 0; i < _buffUIIcons.Length; i++)
        {
            if (_buffUIIcons[i].KeywordIcon == iconEnum)
            {
                uIIconSO = _buffUIIcons[i];
                break;
            }
        }
        if (uIIconSO == null)
            throw new Exception("Could not Find UIICONSO for the keyword " + iconEnum.ToString());


        return uIIconSO;
    }
}
