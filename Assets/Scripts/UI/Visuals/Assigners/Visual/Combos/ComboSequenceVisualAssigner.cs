using Battle.Combo;
using UnityEngine;
using UnityEngine.UI;

namespace CardMaga.UI.Visuals
{
    [System.Serializable]
    public class ComboSequenceVisualAssigner : BaseVisualAssigner<Combo>
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
        public override void Init(Combo comboData)
        {
            for (int i = 0; i < comboData.ComboSequence.Length; i++)
            {
            int cardType = (int)comboData.ComboSequence[i].CardType - 1;

                //Set Combo BG Sprite
                var sprite = BaseVisualSO.GetSpriteToAssign(cardType, cardType, _bodyPartComboVisualSO.BodyPartsBG);
                _comboSequenceBackgrounds[i].AssignSprite(sprite);
                _comboSequenceBackgrounds[i].AssignColor(_bodyPartComboVisualSO.BaseSO.GetInnerColor(comboData.ComboSequence[i].CardType));
                //Set Card Inner BG sprites and color
                sprite = BaseVisualSO.GetSpriteToAssign(cardType, cardType, _bodyPartComboVisualSO.BodyPartsInnerBG);
                _comboSequenceInnerBackgrounds[i].AssignSprite(sprite);
                _comboSequenceInnerBackgrounds[i].AssignColor(_bodyPartComboVisualSO.BaseSO.GetMainColor(comboData.ComboSequence[i].CardType));
                //Set body part and color
                _comboSequenceBodyParts[i].AssignSprite(_bodyPartComboVisualSO.BaseSO.GetBodyPartSprite(comboData.ComboSequence[i].BodyPart));
                _comboSequenceBodyParts[i].AssignColor(_bodyPartComboVisualSO.BaseSO.GetMainColor(comboData.ComboSequence[i].CardType));
            }
        }
        public override void Dispose()
        {
        }
    }
}
