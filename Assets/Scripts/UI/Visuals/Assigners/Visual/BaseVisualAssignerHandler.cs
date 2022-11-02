using System.Collections.Generic;

namespace CardMaga.UI.Visuals
{
    [System.Serializable]
    public abstract class BaseVisualAssignerHandler<T> : BaseVisualHandler<T>
    {
        public abstract IEnumerable<BaseVisualAssigner<T>> VisualAssigners { get; }
        public override void Init(T data)
        {
            foreach (var assigner in VisualAssigners)
            {
                assigner.Init(data);
            }
        }
        public override void CheckValidation()
        {
            foreach (var assigner in VisualAssigners)
            {
                assigner.CheckValidation();
            }
        }
        public override void Dispose()
        {
            foreach (var assigner in VisualAssigners)
            {
                assigner.Dispose();
            }
        }
    }
}
