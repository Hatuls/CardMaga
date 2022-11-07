using System.Collections.Generic;
using UnityEngine;

namespace CardMaga.UI.Visuals
{
    [System.Serializable]
    public class BuffVisualAssignerHandler : BaseVisualAssignerHandler<BuffVisualData>
    {
        [SerializeField] BuffTypeVisualAssigner _buffTypeVisualAssigner;
        [SerializeField] BuffIconVisualAssigner _buffIconVisualAssigner;
        public override IEnumerable<BaseVisualAssigner<BuffVisualData>> VisualAssigners
        {
            get
            {
                yield return _buffTypeVisualAssigner;
                yield return _buffIconVisualAssigner;
            }
        }
    }
}