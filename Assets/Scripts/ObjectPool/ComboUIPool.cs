using System.Collections;
using System.Collections.Generic;
using Battle.Combo;
using UnityEngine;

public class ComboUIPool : MonoBehaviour
{
    [SerializeField] private ComboUI _comboUIPrefab;
    [SerializeField] private RectTransform _parent;

    private ObjectPool<ComboUI> _cardUiPool;

    private void Awake()
    {
        _cardUiPool = new ObjectPool<ComboUI>(_comboUIPrefab, _parent);
    }

    public List<ComboUI> GetCardUIs(params Combo[] comboData)
    {
        if (comboData == null || comboData.Length == 0)
            return null;
            
        List<ComboUI> output = new List<ComboUI>(comboData.Length);

        for (int i = 0; i < comboData.Length; i++)
        {
            ComboUI cache = _cardUiPool.Pull();
            
            cache.AssignComboData(comboData[i]);
            
            output.Add(cache);
        }

        return output;
    }
}
