using CardMaga.UI.Visuals;
using System.Collections.Generic;
using UnityEngine;

namespace CardMaga.UI.Text
{
    [System.Serializable]
    public class BuffTextAssignerHandler : BaseTextAssignerHandler<BuffVisualData>
    {
        [SerializeField] BuffAmountTextAssigner _buffAmountTextAssigner;
        public override IEnumerable<BaseTextAssigner<BuffVisualData>> TextAssigners
        { 
            get 
            {
               yield return _buffAmountTextAssigner;
            } 
        }
    }
}