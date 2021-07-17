
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
        var playerRecipe = Relics.RelicManager.Instance.PlayerRelics.GetRelicSO;
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

        var playerRecipe = Relics.RelicManager.Instance.PlayerRelics.GetRelicSO;
     
        for (int i = 0; i < comboRecipeUIs.Length-1; i++)
        {
            if (i * page < playerRecipe.Length)
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
