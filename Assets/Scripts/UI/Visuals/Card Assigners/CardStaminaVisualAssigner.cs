using UnityEngine;
using UnityEngine.UI;

namespace CardMaga.UI.Visuals
{
    [System.Serializable]
    public class CardStaminaVisualAssigner : BaseVisualAssigner
    {
        [SerializeField] StaminaCardSO _staminaCardSO;
        [SerializeField] Image _staminaBG;
        [SerializeField] Image _staminaInnerCircle;
        [SerializeField] Image _staminaFront;
        public override void Init()
        {
            //SO Checks
            if (_staminaCardSO.StaminaBG.Length == 0)
                throw new System.Exception("StaminaCardSO has no staminaBG");

            if (_staminaCardSO.StaminaInnerCircle.Length == 0)
                throw new System.Exception("StaminaCardSO has no staminaInnerCircle");

            if (_staminaCardSO.StaminaFront.Length == 0)
                throw new System.Exception("StaminaCardSO has no staminaFront");
        }

        public void SetStamina(int staminaTypeNum)
        {
            var staminaType = staminaTypeNum - 1;
            //Set BG
            var sprite = GetSpriteToAssign(staminaType, staminaType, _staminaCardSO.StaminaBG);
            AssignSprite(_staminaBG, sprite);
            //Set Circle
            sprite = GetSpriteToAssign(staminaType, staminaType, _staminaCardSO.StaminaInnerCircle);
            AssignSprite(_staminaInnerCircle, sprite);
            //Set Front
            sprite = GetSpriteToAssign(staminaType, staminaType, _staminaCardSO.StaminaFront);
            AssignSprite(_staminaFront, sprite);
        }
    }
}
