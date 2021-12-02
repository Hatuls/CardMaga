using Sirenix.OdinInspector;
using Battles.UI;
using TMPro;
using UnityEngine;

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







    public void OpenInfoScreen(CardUI cardUI)
    { var card = cardUI.GFX.GetCardReference;
        _levelText.text = card.CardLevel.ToString();
        _bodyPartText.text = card.CardSO.BodyPartEnum.ToString();
        _rarityText.text = card.CardSO.Rarity.ToString();
        _cardUI.GFX.SetCardReference(card, Factory.GameFactory.Instance.ArtBlackBoard);
        this.gameObject.SetActive(true);
    }
    public void Close()
    {
        this.gameObject.SetActive(false);
    }



}
