using System.Linq;
using Battle.Combo;
using CardMaga.Battle.UI;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class ComboRecipeDisplayer : MonoBehaviour, IPointerClickHandler
{
    public static ComboRecipeDisplayer Instance;
    [SerializeField]
    ComboRecipeUI[] comboRecipeUIs;

    [SerializeField]
    int _currentPage;
    [SerializeField] GameObject _panel;
    private void Start()
    {
        Instance = this;
        Init();
    }
    public void Init()
    {
        _currentPage = 1;

        Instance?.gameObject?.SetActive(false);

    }


    private void SetActivePanels()
    {
        _currentPage = 1;
        var leftplayer = BattleUiManager.Instance.BattleDataManager.PlayersManager.GetCharacter(true);
        var recipes = leftplayer.Combos.GetCollection.ToArray();
        ComboSO[] playerRecipe = new ComboSO[recipes.Length];
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
        var leftplayer = BattleUiManager.Instance.BattleDataManager.PlayersManager.GetCharacter(true);
        var recipes = leftplayer.Combos.GetCollection.ToArray();
        BattleComboData[] playerRecipe = new BattleComboData[recipes.Length];
        for (int i = 0; i < playerRecipe.Length; i++)
        {
            playerRecipe[i] = recipes[i];
            Debug.LogWarning($"Recipe { playerRecipe[i].ComboSO.ComboName}"); ;

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
        var leftplayer = BattleUiManager.Instance.BattleDataManager.PlayersManager.GetCharacter(true);
        var recipes = leftplayer.Combos.GetCollection.ToArray();
        if (recipes.Length > comboRecipeUIs.Length * _currentPage)
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
        
        gameObject.SetActive(!gameObject.activeSelf);

        if (gameObject.activeSelf)
        {
            SetActivePanels();
        }
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Init();
    }
}
