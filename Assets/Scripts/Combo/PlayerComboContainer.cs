using System.Collections.Generic;
using Battle.Combo;
using CardMaga.Collection;

public class PlayerComboContainer : IGetCollection<ComboData>
{
    private IEnumerable<ComboData> _comboDatas;
    
    public PlayerComboContainer(ComboData[] comboDatas)
    {
        List<ComboData> comboData = new List<ComboData>(comboDatas.Length);
        for (int i = 0; i < comboDatas.Length; i++)
            comboData.Add(new ComboData(comboDatas[i].ComboSO, comboDatas[i].Level));
        _comboDatas = comboData;
    }
    
    public IEnumerable<ComboData> GetCollection
    {
        get => _comboDatas;
    }
}
