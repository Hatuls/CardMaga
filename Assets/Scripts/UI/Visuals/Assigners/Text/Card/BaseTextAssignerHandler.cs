using System.Collections.Generic;

namespace CardMaga.UI.Text
{
    [System.Serializable]
    public abstract class BaseTextAssignerHandler<T> : BaseVisualHandler<T>
    {
        public abstract IEnumerable<BaseTextAssigner<T>> TextAssigners { get; }
        public override void Init(T data)
        {
            foreach (var assigner in TextAssigners)
            {
                assigner.Init(data);
            }
        }
        public override void CheckValidation()
        {
            foreach (var assigner in TextAssigners)
            {
                assigner.CheckValidation();
            }
        }
        public override void Dispose()
        {
            foreach (var assigner in TextAssigners)
            {
                assigner.Dispose();
            }
        }
    }
}
