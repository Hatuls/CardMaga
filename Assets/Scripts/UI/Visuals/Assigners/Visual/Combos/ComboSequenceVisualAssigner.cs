using Account.GeneralData;
using Battle.Combo;
using CardMaga.Card;
using UnityEngine;
using UnityEngine.UI;

namespace CardMaga.UI.Visuals
{
    [System.Serializable]
    public class ComboSequenceVisualAssigner : BaseVisualAssigner<ComboCore>
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
        public override void Init(ComboCore comboData)
        {
            ComboSO comboSo = comboData.ComboSO();
            var cardCore = comboSo.CraftedCard.CardCore[comboData.Level].CardCore;
            for (int i = 0; i < comboSo.ComboSequence.Length; i++)
            {
                CardTypeData cardTypeData = comboSo.ComboSequence[i];
                var cardType = cardTypeData.CardType;

                //Set Combo BG Sprite
                _comboSequenceBackgrounds[i].AssignSprite(_bodyPartComboVisualSO.GetBodyPartBG(cardCore.CardSO));
                _comboSequenceBackgrounds[i].AssignColor(_bodyPartComboVisualSO.BaseSO.GetInnerColor(cardTypeData.CardType));
                //Set BattleCard Inner BG sprites and color
                _comboSequenceInnerBackgrounds[i].AssignSprite(_bodyPartComboVisualSO.GetBodyPartInnerBG(cardType));
                _comboSequenceInnerBackgrounds[i].AssignColor(_bodyPartComboVisualSO.BaseSO.GetMainColor(cardTypeData.CardType));
                //Set body part and color
                _comboSequenceBodyParts[i].AssignSprite(_bodyPartComboVisualSO.BaseSO.GetBodyPartSprite(cardTypeData.BodyPart));

                if (cardTypeData.BodyPart == CardMaga.Card.BodyPartEnum.Empty)
                {
                    _comboSequenceBodyParts[i].AssignColor(_bodyPartComboVisualSO.BaseSO.GetMainColor(cardTypeData.CardType).SetColorAlpha(0));
                }
                else
                {
                    _comboSequenceBodyParts[i].AssignColor(_bodyPartComboVisualSO.BaseSO.GetMainColor(cardTypeData.CardType).SetColorAlpha(1));
                }

            }
        }
        public override void Dispose()
        {
        }
    }
}

