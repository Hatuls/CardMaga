using Battles.UI;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using Sirenix.OdinInspector;
using Combo;
public class ComboRecipeUI : MonoBehaviour
{

    [BoxGroup("References")]
    [SerializeField]
    CardUI _cardUI;

    ComboSO _comboRecipe;

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
    CraftingSlotUI[] _placeHolderSlotUIs;

    byte activePlaceHolders = 0;
    private void Start()
    {
        activePlaceHolders = 0;
  
    }
    public void InitRecipe(ComboSO relicSO)
    {
        if (_comboRecipe != relicSO)
        {

        _comboRecipe = relicSO;
        _cardUI.GFX.SetCardReference(relicSO.CraftedCard, _art);
        ActivatedPlaceHolders(relicSO);
        SetVisual(relicSO);
        }
    }
    private void ActivatedPlaceHolders(ComboSO relicSO)
    {
    
        if (relicSO.ComboSequance.Length != activePlaceHolders)
        {
            int remain = relicSO.ComboSequance.Length - activePlaceHolders;
            if (remain < 0)
            {
                for (int i = _placeHolderSlotUIs.Length - 1; i >= relicSO.ComboSequance.Length; i--)
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
    private void SetVisual(ComboSO relic)
    {
        _cardUI.GFX.SetCardReference(relic.CraftedCard, _art);

        int ComboCheck = 0;

        for (int i = 0; i < _placeHolderSlotUIs.Length; i++)
        {
            if (_placeHolderSlotUIs[i].gameObject.activeSelf && i - ComboCheck >= 0 && i - ComboCheck < relic.ComboSequance.Length)
            {
               _placeHolderSlotUIs[i].InitPlaceHolder(relic.ComboSequance[i - ComboCheck]);
                ComboCheck = 0;
            }
            else
                ComboCheck++;
        }
    }
}
