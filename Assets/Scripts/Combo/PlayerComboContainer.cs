using System.Collections.Generic;
using Battle.Combo;
using CardMaga.Collection;

public class PlayerComboContainer : IGetCollection<BattleComboData>
{
    private IEnumerable<BattleComboData> _comboDatas;
    
    public PlayerComboContainer(BattleComboData[] comboDatas)
    {
        List<BattleComboData> comboData = new List<BattleComboData>(comboDatas.Length);
        for (int i = 0; i < comboDatas.Length; i++)
            comboData.Add(new BattleComboData(comboDatas[i].ComboSO, comboDatas[i].Level));
        _comboDatas = comboData;
    }
    
    public IEnumerable<BattleComboData> GetCollection
    {
        get => _comboDatas;
    }
}
