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
        public override BaseVisualAssignerHandler<BattleComboData> ComboVisualAssignerHandler => _comboVisualAssignerHandler;
        public override BaseTextAssignerHandler<BattleComboData> ComboTextAssignerHandler => _comboTextAssignerHandler;
        public CardVisualHandler ComboCard => _comboCard;
#if UNITY_EDITOR
        [FormerlySerializedAs("_testCombo")]
        [Header("Test")]
        [SerializeField] BattleComboData testBattleComboData;

        [Button]
        public void OnTryCombo()
        {
            if (testBattleComboData == null)
            {
                new System.Exception("TestFailed, Enter A combo to the Test Combo Slot");
                return;
            }
            CheckValidation();
            Dispose();
            Init(testBattleComboData);
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
        public override void Init(BattleComboData battleComboDataData)
        {
            base.Init(battleComboDataData);
            _comboPopUp.Init(battleComboDataData);
            var cardData = Factory.GameFactory.Instance.CardFactoryHandler.CreateCard(battleComboDataData.CraftedCard,battleComboDataData.ComboCore.Level);
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