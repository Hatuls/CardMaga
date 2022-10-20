using Battle.Combo;
using CardMaga.UI.PopUp;
using CardMaga.UI.Text;
using CardMaga.UI.Visuals;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace CardMaga.UI.Combos
{
    public class ComboVisualHandler : BaseComboVisualHandler
    {
        [SerializeField] CardVisualHandler _comboCard;
        [SerializeField] ComboDescriptionPopUp _comboPopUp;
        [SerializeField] ComboTextAssignerHandler _comboTextAssignerHandler;
        [SerializeField] ComboVisualAssignerHandler _comboVisualAssignerHandler;
        public override BaseVisualAssignerHandler<ComboData> ComboVisualAssignerHandler => _comboVisualAssignerHandler;
        public override BaseTextAssignerHandler<ComboData> ComboTextAssignerHandler => _comboTextAssignerHandler;
#if UNITY_EDITOR
        [FormerlySerializedAs("_testCombo")]
        [Header("Test")]
        [SerializeField] ComboData testComboData;

        [Button]
        public void OnTryCombo()
        {
            if (testComboData == null)
            {
                new System.Exception("TestFailed, Enter A combo to the Test Combo Slot");
                return;
            }
            CheckValidation();
            Dispose();
            Init(testComboData);
            _comboPopUp.ActivatePopUP(true);
        }
        [Button]
        public void OnActivatePopUp(bool toActivate)
        {
            _comboPopUp.ActivatePopUP(toActivate);
        }
#endif
        public override void CheckValidation()
        {
            base.CheckValidation();
            _comboPopUp.CheckValidation();
            _comboCard.CheckValidation();
        }
        public override void Init(ComboData comboDataData)
        {
            base.Init(comboDataData);
            _comboPopUp.Init(comboDataData);
            var cardData = Factory.GameFactory.Instance.CardFactoryHandler.CreateCard(comboDataData.CraftedCard,comboDataData.ComboCore.Level);
            _comboCard.Init(cardData);
            _comboPopUp.ActivatePopUP(true);

        }
        public override void Dispose()
        {
            base.Dispose();
            _comboPopUp.Dispose();
            _comboCard.Dispose();
        }
    }
}