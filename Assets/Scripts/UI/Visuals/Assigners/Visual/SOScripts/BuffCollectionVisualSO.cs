using CardMaga.Keywords;
using UnityEngine;

namespace CardMaga.UI.Visuals
{
    [CreateAssetMenu(fileName = "Buff Collection Visual SO", menuName = "ScriptableObjects/UI/Visuals/Buff Collection Visual SO")]
    public class BuffCollectionVisualSO : BaseVisualSO
    {
        public BuffVisualSO[] BuffsVisualSos;
        public override void CheckValidation()
        {
            if (BuffsVisualSos.Length == 0)
                throw new System.Exception("BuffCollectionVisualSO Has no Buffs Visuals");
        }
        public BuffVisualSO GetBuffSO(KeywordType keywordType)
        {
            for (int i = 0; i < BuffsVisualSos.Length; i++)
            {
                if(BuffsVisualSos[i].KeywordType == keywordType)
                    return BuffsVisualSos[i];
            }
            throw new System.Exception("BuffCollectionVisualSO GetBuffSO Could not find the Keyword Type In it's Collection");
        }
    }
}