
using UnityEngine;
using UnityEngine.EventSystems;

public class ComboRecipeHandler : MonoSingleton<ComboRecipeHandler>, IPointerClickHandler
{
    [SerializeField]
    ComboRecipeUI[] comboRecipeUIs;
    bool isFirstTime;
    [SerializeField]
    int _currentPage;
    [SerializeField] GameObject _panel;

    public override void Init()
    {
        _currentPage = 1;

        Instance?.gameObject?.SetActive(false);

    }


    private void SetActivePanels()
    {
        var recipes = Managers.PlayerManager.Instance.Recipes;
        Combo.ComboSO[] playerRecipe = new Combo.ComboSO[recipes.Length];
        for (int i = 0; i < playerRecipe.Length; i++)
            playerRecipe[i] = recipes[i].ComboSO;

        if (playerRecipe.Length < comboRecipeUIs.Length)
        {
            for (int i = comboRecipeUIs.Length - 1; i >= playerRecipe.Length; i--)
            {
                comboRecipeUIs[i].gameObject.SetActive(false);
            }
        }
    
        InitPage();
    }
    private void InitPage(int page=1)
    {

        if (page <= 0)
            page = 1;

        var recipes = Managers.PlayerManager.Instance.Recipes;
        Combo.ComboSO[] playerRecipe = new Combo.ComboSO[recipes.Length];
        for (int i = 0; i < playerRecipe.Length; i++)
        {
            playerRecipe[i] = recipes[i].ComboSO;
            Debug.LogWarning($"Recipe { playerRecipe[i].name}"); ;

        }

        for (int i = 0; i < comboRecipeUIs.Length; i++)
        {
            var calculation = (comboRecipeUIs.Length * (page - 1)) + i;
            if (calculation < playerRecipe.Length)
            {
                if(!comboRecipeUIs[i].gameObject.activeSelf)
                {
                    comboRecipeUIs[i].gameObject.SetActive(true);
                }
                comboRecipeUIs[i].InitRecipe(playerRecipe[calculation]);
            }
            else
            {

                comboRecipeUIs[i].gameObject.SetActive(false);
            }
        }
    }
    public void PageRight()
    {
        var recipes = Managers.PlayerManager.Instance.Recipes;
        if(recipes.Length > comboRecipeUIs.Length * _currentPage)
        {
            _currentPage++;
            InitPage(_currentPage);
        }
    }
    public void PageLeft()
    {
        if (comboRecipeUIs.Length * _currentPage > comboRecipeUIs.Length)
        {
            _currentPage--;
            InitPage(_currentPage);
        }
    }

    public void ActivateRelicPanel()
    {
        
        this.gameObject.SetActive(!this.gameObject.activeSelf);

        if (this.gameObject.activeSelf)
        {
            SetActivePanels();
        }
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Init();
    }
}
