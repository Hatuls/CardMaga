using Battles.UI;
using Sirenix.OdinInspector;
using TMPro;
using UI.Meta.Laboratory;
using UnityEngine;
using UnityEngine.UI;

public class CardUIInfoScreen : MonoBehaviour
{
    [SerializeField]
    CardUI _cardUI;
    [TitleGroup("Components")]

    [TabGroup("Components/Type", "Text Mesh Pro")]
    [SerializeField]
    TextMeshProUGUI _rarityText;

    [TabGroup("Components/Type", "Text Mesh Pro")]
    [SerializeField]
    TextMeshProUGUI _bodyPartText;

    [TabGroup("Components/Type", "Text Mesh Pro")]
    [SerializeField]
    TextMeshProUGUI _levelText;

    [SerializeField]
    Button _dismentalBtn;

    [SerializeField]
    Button _useBtn;

    [SerializeField]
    Button _upgradeBtn;

    [SerializeField]
    SoundEventSO _onClickSound;

    [SerializeField]
    UpgradeCardScreenUI _upgradeCardScreenUI;
    [SerializeField]
    ButtonUI _deckButtonInLab;
    public void OpenInfoScreen(CardUI cardUI, IInfoSettings<CardUI> infoSettings)
    {
        var card = cardUI.GFX.GetCardReference;
        _levelText.text = card.CardLevel.ToString();
        _bodyPartText.text = card.CardSO.BodyPartEnum.ToString();
        _rarityText.text = card.CardSO.Rarity.ToString();
        _cardUI.GFX.SetCardReference(card);
        ActivateButton(infoSettings, cardUI);
        this.gameObject.SetActive(true);
    }
    private void ActivateButton(IInfoSettings<CardUI> infoSettings, CardUI cardUI)
    {
        const int _deckMinLength = 8;
        _dismentalBtn.gameObject.SetActive(infoSettings.CanDismental && Account.AccountManager.Instance.AccountCards.CardList.Count > _deckMinLength);
        _upgradeBtn.gameObject.SetActive(infoSettings.CanUpgrade && cardUI.RecieveCardReference().CardsAtMaxLevel == false);
        _useBtn.gameObject.SetActive(infoSettings.CanUse);

        if (_useBtn.gameObject.activeSelf)
            _useBtn.onClick.AddListener(() => OnUseClick());
        else
            _useBtn.onClick.RemoveAllListeners();

        if (_upgradeBtn.gameObject.activeSelf)
            _upgradeBtn.onClick.AddListener(() => OnUpgradeClick());
        else
            _upgradeBtn.onClick.RemoveAllListeners();

        void OnUseClick()
        {
            infoSettings.OnUse(_cardUI);
            _onClickSound.PlaySound();
            Close();
        }

        void OnUpgradeClick()
        {
            _deckButtonInLab.ButtonPressed();
            _upgradeCardScreenUI.SelectCardUI(_cardUI);
            _onClickSound.PlaySound();
            Close();
        }
    }


    public void Close()
    {
        this.gameObject.SetActive(false);
    }



}
