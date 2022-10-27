using System.Collections.Generic;
using Battle.Combo;
using CardMaga.Collection;

public class PlayerComboContainer : IGetSourceCollection<ComboData>
{
    private IEnumerable<ComboData> _comboDatas;
    
    public PlayerComboContainer(ComboData[] comboDatas)
    {
        _comboDatas = comboDatas;
    }
    
    public IEnumerable<ComboData> GetCollection
    {
        get => _comboDatas;
    }
}
