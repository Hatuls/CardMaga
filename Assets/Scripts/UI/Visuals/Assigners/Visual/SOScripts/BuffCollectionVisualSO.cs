using CardMaga.Keywords;
using UnityEngine;

namespace CardMaga.UI.Visuals
{
    [CreateAssetMenu(fileName = "Buff Collection Visual SO", menuName = "ScriptableObjects/UI/Visuals/Buff Collection Visual SO")]
    public class BuffCollectionVisualSO : BaseVisualSO
    {
        public BuffVisualSO[] BuffsVisualSOs;
        public override void CheckValidation()
        {
            if (BuffsVisualSOs.Length == 0)
                throw new System.Exception("BuffCollectionVisualSO Has no Buffs Visuals");
        }
        public BuffVisualSO GetBuffSO(KeywordType keywordType)
        {
            for (int i = 0; i < BuffsVisualSOs.Length; i++)
            {
                if(BuffsVisualSOs[i].KeywordType == keywordType)
                    return BuffsVisualSOs[i];
            }
            throw new System.Exception("BuffCollectionVisualSO GetBuffSO Could not find the Keyword Type In it's Collection");
        }
    }
}