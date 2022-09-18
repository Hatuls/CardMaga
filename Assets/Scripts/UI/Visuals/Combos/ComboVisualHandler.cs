using Battle.Combo;
using CardMaga.UI.PopUp;
using CardMaga.UI.Text;
using CardMaga.UI.Visuals;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CardMaga.UI.Combos
{
    public class ComboVisualHandler : BaseComboVisualHandler
    {
        [SerializeField] CardVisualHandler _comboCard;
        [SerializeField] ComboDescriptionPopUp _comboPopUp;
        [SerializeField] ComboTextAssignerHandler _comboTextAssignerHandler;
        [SerializeField] ComboVisualAssignerHandler _comboVisualAssignerHandler;
        public override BaseVisualAssignerHandler<Combo> ComboVisualAssignerHandler => _comboVisualAssignerHandler;
        public override BaseTextAssignerHandler<Combo> ComboTextAssignerHandler => _comboTextAssignerHandler;
#if UNITY_EDITOR
        [Header("Test")]
        [SerializeField] Combo _testCombo;

        [Button]
        public void OnTryCombo()
        {
            if (_testCombo == null)
            {
                new System.Exception("TestFailed, Enter A combo to the Test Combo Slot");
                return;
            }
            CheckValidation();
            Dispose();
            Init(_testCombo);
        }
#endif
        public override void CheckValidation()
        {
            base.CheckValidation();
            _comboPopUp.CheckValidation();
            _comboCard.CheckValidation();
        }
        public override void Init(Combo comboData)
        {
            base.Init(comboData);
            _comboPopUp.Init(comboData);
            var cardData = Factory.GameFactory.Instance.CardFactoryHandler.CreateCard(comboData.CraftedCard,comboData.ComboCore.Level);
            _comboCard.Init(cardData);
        }
        public override void Dispose()
        {
            base.Dispose();
            _comboPopUp.Dispose();
            _comboCard.Dispose();
        }
    }
}