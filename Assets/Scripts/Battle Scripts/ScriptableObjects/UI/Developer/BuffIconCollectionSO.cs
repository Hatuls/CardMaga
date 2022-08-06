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

        return uIIconSO;
    }

#if UNITY_EDITOR
    [Sirenix.OdinInspector.Button]
    private void LoadIconsSO()
    {
        _buffUIIcons = Resources.LoadAll<UIIconSO>("Art/Battle Keywords Icons");
    }
#endif
}
