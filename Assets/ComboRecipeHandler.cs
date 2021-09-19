
using UnityEngine;

public class ComboRecipeHandler : MonoSingleton<ComboRecipeHandler>
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

        this.gameObject.SetActive(false);

    }


    private void SetActivePanels()
    {
        var recipes = Managers.PlayerManager.Instance.Recipes;
        Combo.ComboSO[] playerRecipe = new Combo.ComboSO[recipes.Length];
        for (int i = 0; i < playerRecipe.Length; i++)
            playerRecipe[i] = recipes[i].ComboRecipe;

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
            playerRecipe[i] = recipes[i].ComboRecipe;
            Debug.LogWarning($"Recipe { playerRecipe[i].name}"); ;

        }

        for (int i = 0; i < comboRecipeUIs.Length; i++)
        {
            if (comboRecipeUIs[i]  != null && comboRecipeUIs[i].gameObject.activeSelf && i * page < playerRecipe.Length)
            {

            comboRecipeUIs[i].InitRecipe(playerRecipe[i* page]);
            }
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
}
