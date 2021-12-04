using Battles.UI;
using UI.Meta.Laboratory;
using UnityEngine;
using TMPro;
namespace Meta.UI
{

    public class UpgradeUIScreen : MonoBehaviour
    {
        [SerializeField]
        MetaCardUIHandler _selectedCardUI;

        [SerializeField]
        MetaCardUIHandler _upgradedVersion;

        [SerializeField]
        MetaCardUIFilterScreen _collectionFilterHandler;

        [SerializeField]
        CardUpgradeCostSO _upgradeCostSO;

        [SerializeField]
        TextMeshProUGUI _instructionText;
        [SerializeField]
        TextMeshProUGUI _costText;
        [SerializeField]
        GameObject _upgradeBtn;
        [SerializeField]
        string _shownText;

        [SerializeField]
        string defaultText = "Choose Card:";
        string costText = "Upgrade Cost: ";

        private void Start()
        {
            ResetScreen();
        }
        public void ResetScreen()
        {
            ActivateGameObject(_upgradeBtn, false);
            ActivateGameObject(_selectedCardUI.gameObject, false);
            ActivateGameObject(_upgradedVersion.gameObject, false);
            _instructionText.text = defaultText;
        }

        public void SelectCardUI(CardUI card)
        {
            if (card == null)
                throw new System.Exception($"UpgradeUIScreen : Card Is Null!");

            _selectedCardUI.CardUI.GFX.SetCardReference(card.GFX.GetCardReference, Factory.GameFactory.Instance.ArtBlackBoard);
            ActivateGameObject(_selectedCardUI.gameObject, true);

            var upgradedVersion = UpgradeHandler.GetUpgradedCardVersion(_selectedCardUI.CardUI.GFX.GetCardReference);

            if (upgradedVersion != null)
            {
                SetCostText();
                _upgradedVersion.CardUI.GFX.SetCardReference(upgradedVersion, Factory.GameFactory.Instance.ArtBlackBoard);
            }


            ActivateGameObject(_upgradedVersion.gameObject, upgradedVersion != null);
        }
        public void RemoveCardUI() { ResetScreen(); }
        private void SetCostText()
        {
            _instructionText.text = costText;
            ActivateGameObject(_upgradeBtn, false);
            _costText.text = _upgradeCostSO.NextCardValue(_selectedCardUI.CardUI.GFX.GetCardReference).ToString();
        }
        private void ActivateGameObject(GameObject go, bool state)
        {
            if (go.activeSelf != state)
                go.SetActive(state);
        }

        public void OnUpgradeClick()
        {
            if (!_selectedCardUI.gameObject.activeSelf || !_upgradedVersion.gameObject.activeSelf)
                return;
            if (UpgradeHandler.TryUpgradeCard(_upgradeCostSO, _selectedCardUI.CardUI.GFX.GetCardReference))
            {
                ResetScreen();
                _collectionFilterHandler.Refresh();
                Debug.Log(" Succeed");
            ActivateGameObject(_upgradeBtn, false);
            }
            else
            {


                Debug.Log("Didnt upgrade");
            }

        }

      
    }



}
namespace Meta
{
    public static class UpgradeHandler
    {
        public static Cards.Card GetUpgradedCardVersion(Cards.Card card)
        {
            if (card.CardLevel == card.CardSO.CardsMaxLevel)
                return null;

            return Factory.GameFactory.Instance.CardFactoryHandler.CreateCard(card.CardSO, (byte)(card.CardLevel + 1));
        }
        public static bool TryUpgradeCard(CardUpgradeCostSO cardUpgradeCostSO,Cards.Card card)
        {
            var account = Account.AccountManager.Instance;
            var chips = account.AccountGeneralData.AccountResourcesData.Chips;
            var Cost = cardUpgradeCostSO.NextCardValue(card);
            if (chips.Value >= Cost )
            {
                account.AccountCards.UpgradeCard(card.CardInstanceID);
                chips.ReduceValue(Cost);
                return true;
            }
            else
                return false;
        }
   
    }

}