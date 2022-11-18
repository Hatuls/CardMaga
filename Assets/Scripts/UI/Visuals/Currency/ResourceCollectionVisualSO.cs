using CardMaga.Rewards;
using UnityEngine;

namespace CardMaga.UI.Visuals
{
    [CreateAssetMenu(fileName = "Resource Collection Visual SO", menuName = "ScriptableObjects/UI/Visuals/Resource Collection Visual SO")]
    public class ResourceCollectionVisualSO:BaseVisualSO
    {
        [SerializeField] ResourceVisualSO[] _resourceVisualSos;

        public override void CheckValidation()
        {
            if (_resourceVisualSos.Length == 0)
                throw new System.Exception("ResourceCollectionVisualSO has no resources visual SOs");

            foreach (ResourceVisualSO resourceVisualSO in _resourceVisualSos)
                resourceVisualSO.CheckValidation();
        }

        public ResourceVisualSO GetResourceSO(CurrencyType type)
        {
            for (int i = 0; i < _resourceVisualSos.Length; i++)
            {
                if (_resourceVisualSos[i].MyCurrencyType == type)
                {
                    return _resourceVisualSos[i];
                }
            }
            throw new System.Exception("ResourceCollectionVisualSO could not find the resource type SO");
        }
    }
}