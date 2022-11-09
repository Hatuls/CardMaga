using Battle.Combo;
using UnityEngine;
using UnityEngine.UI;

namespace CardMaga.UI.Visuals
{
    [System.Serializable]
    public class ComboSequenceVisualAssigner : BaseVisualAssigner<BattleComboData>
    {
        [SerializeField] BodyPartComboVisualSO _bodyPartComboVisualSO;
        [SerializeField] Image[] _comboSequenceBodyParts;
        [SerializeField] Image[] _comboSequenceBackgrounds;
        [SerializeField] Image[] _comboSequenceInnerBackgrounds;
        public override void CheckValidation()
        {
            _bodyPartComboVisualSO.CheckValidation();

            if (_comboSequenceBodyParts == null)
                throw new System.Exception("ComboSequenceVisualAssigner has no _comboSequenceBodyParts");

            if (_comboSequenceBackgrounds == null)
                throw new System.Exception("ComboSequenceVisualAssigner has no _comboSequenceBackgrounds");

            if (_comboSequenceInnerBackgrounds == null)
                throw new System.Exception("ComboSequenceVisualAssigner has no _comboSequenceInnerBackgrounds");
        }
        public override void Init(BattleComboData battleComboDataData)
        {
            for (int i = 0; i < battleComboDataData.ComboSequence.Length; i++)
            {
                var cardType = battleComboDataData.ComboSequence[i].CardType;

                //Set Combo BG Sprite
                _comboSequenceBackgrounds[i].AssignSprite(_bodyPartComboVisualSO.GetBodyPartBG(cardType));
                _comboSequenceBackgrounds[i].AssignColor(_bodyPartComboVisualSO.BaseSO.GetInnerColor(battleComboDataData.ComboSequence[i].CardType));
                //Set BattleCard Inner BG sprites and color
                _comboSequenceInnerBackgrounds[i].AssignSprite(_bodyPartComboVisualSO.GetBodyPartInnerBG(cardType));
                _comboSequenceInnerBackgrounds[i].AssignColor(_bodyPartComboVisualSO.BaseSO.GetMainColor(battleComboDataData.ComboSequence[i].CardType));
                //Set body part and color
                _comboSequenceBodyParts[i].AssignSprite(_bodyPartComboVisualSO.BaseSO.GetBodyPartSprite(battleComboDataData.ComboSequence[i].BodyPart));

                if (battleComboDataData.ComboSequence[i].BodyPart == CardMaga.Card.BodyPartEnum.Empty)
                {
                    _comboSequenceBodyParts[i].AssignColor(_bodyPartComboVisualSO.BaseSO.GetMainColor(battleComboDataData.ComboSequence[i].CardType).SetColorAlpha(0));
                }
                else
                {
                    _comboSequenceBodyParts[i].AssignColor(_bodyPartComboVisualSO.BaseSO.GetMainColor(battleComboDataData.ComboSequence[i].CardType).SetColorAlpha(1));
                }

            }
        }
        public override void Dispose()
        {
        }
    }
}

