﻿using System.Collections.Generic;
using System.Linq;
using Battle.Combo;

public class ComboDataSort
{
    public List<ComboData> SortComboData(List<ComboData> comboDatas)
    {
        return comboDatas.OrderBy(datas => datas.CraftedCard.CardTypeEnum).ThenBy(datas => datas.Level).ToList();
    } 
}