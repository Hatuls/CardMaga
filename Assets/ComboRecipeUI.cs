using Battles.UI;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using Sirenix.OdinInspector;
using Relics;
public class ComboRecipeUI : MonoBehaviour
{

    [BoxGroup("References")]
    [SerializeField]
    CardUI _cardUI;

    RelicSO _comboRecipe;

    [BoxGroup("References")]
 

    [BoxGroup("References")]
    [SerializeField]
    Transform placeHolderContainer;


    [BoxGroup("References")]
    [SerializeField]
    Art.ArtSO _art;


    [BoxGroup("References")]
    [SerializeField]
    Image _backgroundImage;

    [BoxGroup("References")]
    [SerializeField]
    TextMeshProUGUI _recipeTitle;


    [BoxGroup("References")]
    [SerializeField]
    PlaceHolderSlotUI[] _placeHolderSlotUIs;

    byte activePlaceHolders = 0;
    private void Start()
    {
        activePlaceHolders = 0;
  
    }
    public void InitRecipe(RelicSO relicSO)
    {
        if (_comboRecipe != relicSO)
        {

        _comboRecipe = relicSO;
        _cardUI.GFX.SetCardReference(relicSO.GetCraftedCard, _art);
        ActivatedPlaceHolders(relicSO);
        SetVisual(relicSO);
        }
    }
    private void ActivatedPlaceHolders(RelicSO relicSO)
    {
    
        if (relicSO.GetCombo.Length != activePlaceHolders)
        {
            int remain = relicSO.GetCombo.Length - activePlaceHolders;
            if (remain < 0)
            {
                for (int i = _placeHolderSlotUIs.Length - 1; i >= relicSO.GetCombo.Length; i--)
                {
                    _placeHolderSlotUIs[i].gameObject.SetActive(false);
                    activePlaceHolders--;
                }
            }
            else
            {
                for (int i = 0; i < _placeHolderSlotUIs.Length - 1; i++)
                {
                    _placeHolderSlotUIs[i].gameObject.SetActive(true);
                    activePlaceHolders++;
                }
            }
        }
    }
    private void SetVisual(RelicSO relic)
    {
        _cardUI.GFX.SetCardReference(relic.GetCraftedCard, _art);

        int ComboCheck = 0;

        for (int i = 0; i < _placeHolderSlotUIs.Length; i++)
        {
            if (_placeHolderSlotUIs[i].gameObject.activeSelf && i - ComboCheck >= 0 && i - ComboCheck < relic.GetCombo.Length)
            {
               _placeHolderSlotUIs[i].InitPlaceHolder(_art, relic.GetCombo[i - ComboCheck]);
                ComboCheck = 0;
            }
            else
                ComboCheck++;
        }
    }
}
