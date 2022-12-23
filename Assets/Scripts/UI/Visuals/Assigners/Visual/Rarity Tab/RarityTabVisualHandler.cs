using CardMaga.UI.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CardMaga.UI.Visuals
{

    public class RarityTabVisualHandler : BaseVisualAssignerHandler<RarityTextData>
    {
        [SerializeField]
        private RarityTabVisualAssigner _visalAssigner;
        public override IEnumerable<BaseVisualAssigner<RarityTextData>> VisualAssigners
        {
            get
            {
                yield return _visalAssigner;
            }
        }

    }
   
}