using Battle.UI;

namespace UI.Meta.Laboratory
{
    public interface IInfoSettings<T> where T : class
    {
        void OnUse(T usedObject);
        bool CanDismental { get; set; }
        bool CanUpgrade { get; set; }
        bool CanUse { get; set; }
    }
}