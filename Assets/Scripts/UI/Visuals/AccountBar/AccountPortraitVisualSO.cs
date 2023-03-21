using UnityEngine;

namespace CardMaga.UI.Visuals
{
    [CreateAssetMenu(fileName = "New Account Portrait SO", menuName = "ScriptableObjects/UI/Visuals/Account Portrait SO")]
    public class AccountPortraitVisualSO : BaseVisualSO
    {
        public Sprite AccountPortraitSprite;
        public int AccountPortraitID;
        public override void CheckValidation()
        {
            if (AccountPortraitSprite == null)
                throw new System.Exception($"AccountPortraitSO of CoreID:{AccountPortraitID} has no sprite");
        }
    }
}