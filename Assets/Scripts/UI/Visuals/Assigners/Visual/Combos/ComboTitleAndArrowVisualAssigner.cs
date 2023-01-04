using Account.GeneralData;
using Battle.Combo;
using UnityEngine.UI;
using UnityEngine;

namespace CardMaga.UI.Visuals
{
    [System.Serializable]
    public class ComboTitleAndArrowVisualAssigner : BaseVisualAssigner<ComboCore>
    {
        [SerializeField] TitleAndArrowComboVisualSO _titleAndArrowComboVisualSO;
        [SerializeField] Image _arrowImage;
        [SerializeField] Image _titleImage;

        public override void CheckValidation()
        {
            if (_arrowImage == null)
                throw new System.Exception("ComboArrowVisualAssigner has no arrow image");
            if (_titleImage == null)
                throw new System.Exception("ComboArrowVisualAssigner has no title image");
            _titleAndArrowComboVisualSO.CheckValidation();
        }
        
        public override void Dispose()
        {
        }

        public override void Init(ComboCore comboData)
        {
            _arrowImage.AssignSprite(_titleAndArrowComboVisualSO.GetArrowSprite(comboData.ComboSO().CraftedCard.CardTypeEnum));
            _titleImage.AssignSprite(_titleAndArrowComboVisualSO.GetTitleSprite(comboData.ComboSO().CraftedCard.CardTypeEnum));
        }
    }
}