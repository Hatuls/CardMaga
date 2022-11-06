using System.Collections.Generic;
using UnityEngine;

namespace CardMaga.UI.Visuals
{
    public class BuffVisualAssignerHandler : BaseVisualAssignerHandler<BuffVisualData>
    {
        [SerializeField] BuffTypeVisualAssigner _buffTypeVisualAssigner;
        public override IEnumerable<BaseVisualAssigner<BuffVisualData>> VisualAssigners
        {
            get
            {
                yield return _buffTypeVisualAssigner;
            }
        }
    }
}