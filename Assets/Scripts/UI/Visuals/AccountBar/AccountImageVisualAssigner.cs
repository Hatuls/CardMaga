using UnityEngine;
using UnityEngine.UI;

namespace CardMaga.UI.Visuals
{
    [System.Serializable]
    public class AccountImageVisualAssigner : BaseVisualAssigner<AccountBarVisualData>
    {
        [SerializeField] Image _accountPortrait;
        [SerializeField] AccountPortraitCollectionVisualSO _accountPortraitCollectionVisualSos;
        public override void CheckValidation()
        {
            if (_accountPortrait == null)
                throw new System.Exception("AccountImageVisualAssigner has no accountPortriat Image");
            _accountPortraitCollectionVisualSos.CheckValidation();
        }
        public override void Init(AccountBarVisualData comboData)
        {
            _accountPortrait.AssignSprite(_accountPortraitCollectionVisualSos.GetPortraitSO(comboData.AccountImageID).AccountPortraitSprite);
        }
        public override void Dispose()
        {

        }
    }
}