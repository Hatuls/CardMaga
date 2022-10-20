using Battle.Combo;
using UnityEngine;
using UnityEngine.UI;

namespace CardMaga.UI.Visuals
{
    [System.Serializable]
    public class ComboSequenceVisualAssigner : BaseVisualAssigner<ComboData>
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
        public override void Init(ComboData comboDataData)
        {
            for (int i = 0; i < comboDataData.ComboSequence.Length; i++)
            {
            int cardType = (int)comboDataData.ComboSequence[i].CardType - 1;

                //Set Combo BG Sprite
                var sprite = BaseVisualSO.GetSpriteToAssign(cardType, cardType, _bodyPartComboVisualSO.BodyPartsBG);
                _comboSequenceBackgrounds[i].AssignSprite(sprite);
                _comboSequenceBackgrounds[i].AssignColor(_bodyPartComboVisualSO.BaseSO.GetInnerColor(comboDataData.ComboSequence[i].CardType));
                //Set Card Inner BG sprites and color
                sprite = BaseVisualSO.GetSpriteToAssign(cardType, cardType, _bodyPartComboVisualSO.BodyPartsInnerBG);
                _comboSequenceInnerBackgrounds[i].AssignSprite(sprite);
                _comboSequenceInnerBackgrounds[i].AssignColor(_bodyPartComboVisualSO.BaseSO.GetMainColor(comboDataData.ComboSequence[i].CardType));
                //Set body part and color
                _comboSequenceBodyParts[i].AssignSprite(_bodyPartComboVisualSO.BaseSO.GetBodyPartSprite(comboDataData.ComboSequence[i].BodyPart));
                _comboSequenceBodyParts[i].AssignColor(_bodyPartComboVisualSO.BaseSO.GetMainColor(comboDataData.ComboSequence[i].CardType));
            }
        }
        public override void Dispose()
        {
        }
    }
}
