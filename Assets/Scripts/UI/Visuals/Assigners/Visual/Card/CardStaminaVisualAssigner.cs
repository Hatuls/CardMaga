using Account.GeneralData;
using UnityEngine;
using UnityEngine.UI;

namespace CardMaga.UI.Visuals
{
    [System.Serializable]
    public class CardStaminaVisualAssigner : BaseVisualAssigner<CardCore>
    {
        [SerializeField] StaminaCardSO _staminaCardSO;
        [SerializeField] Image _staminaBG;
        [SerializeField] Image _staminaInnerCircle;
        [SerializeField] Image _staminaFront;

        public override void CheckValidation()
        {
            //SO Checks
            if (_staminaCardSO.StaminaBG.Length == 0)
                throw new System.Exception("StaminaCardSO has no staminaBG");

            if (_staminaCardSO.StaminaInnerCircle.Length == 0)
                throw new System.Exception("StaminaCardSO has no staminaInnerCircle");

            if (_staminaCardSO.StaminaFront.Length == 0)
                throw new System.Exception("StaminaCardSO has no staminaFront");
        }

        public override void Dispose()
        {
        }

        public override void Init(CardCore cardCore)
        {
            //Hard Coded Value
            var staminaType = 0;

            //Set BG
            var sprite = BaseVisualSO.GetSpriteToAssign(staminaType, staminaType, _staminaCardSO.StaminaBG);
            _staminaBG.AssignSprite(sprite);
            //Set Circle
            sprite = BaseVisualSO.GetSpriteToAssign(staminaType, staminaType, _staminaCardSO.StaminaInnerCircle);
            _staminaInnerCircle.AssignSprite(sprite);
            //Set Front
            sprite = BaseVisualSO.GetSpriteToAssign(staminaType, staminaType, _staminaCardSO.StaminaFront);
            _staminaFront.AssignSprite(sprite);
        }
    }
}
