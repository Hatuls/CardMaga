using Sirenix.OdinInspector;
using TMPro;
using UI.Meta.Laboratory;
using UnityEngine;

public class MapUpgradesCollection : MonoBehaviour
{


    [TitleGroup("Upgrades")]
    [SerializeField]
    DojoManager _dojoManager;
    [TabGroup("Upgrades/Components", "Panels")]
    [SerializeField]
    GameObject _upgradeCardsPanel;
    [TabGroup("Upgrades/Components", "Panels")]
    [SerializeField]
    GameObject _upgradeComboPanel;
    [TabGroup("Upgrades/Components", "Panels")]
    [SerializeField]
    GameObject _upgradeButtons;
    [TabGroup("Upgrades/Components", "Panels")]
    [SerializeField]
    GameObject _upgradeCardsTitle;

    [TabGroup("Upgrades/Components", "Panels")]
    [SerializeField]
    GameObject _upgradeScrolls;

    [SerializeField]
    [TabGroup("Upgrades/Components", "Filters")]
    MetaComboUIFilterScreen _metaComboFilter;
    [SerializeField]
    [TabGroup("Upgrades/Components", "Filters")]
    MetaCardUIFilterScreen _metaCardFilter;



    [TabGroup("Upgrades/Components", "Upgrade Screens")]
    [SerializeField]
    UpgradeCardScreenUI _upgradeCardScreen;
    [TabGroup("Upgrades/Components", "Upgrade Screens")]
    [SerializeField]
    UpgradeComboScreenUI _upgradeComboScreen;

    [TabGroup("Upgrades/Components", "Title Texts")]
    [SerializeField] TextMeshProUGUI _titleText;
    [TabGroup("Upgrades/Components", "Title Texts")]
    [SerializeField] string _upgradeCardsTitleText = "Upgrade Cards";
    [TabGroup("Upgrades/Components", "Title Texts")]
    [SerializeField] string _upgradeCombosTitleText = "Upgrade Combos";



    public void Open()
    {
        _upgradeCardScreen.OnOpenUpgradeScreen();
        _upgradeComboScreen.OnOpenUpgradeScreen();
        _upgradeButtons.gameObject.SetActive(true);
        _upgradeCardsTitle.SetActive(true);
        _upgradeScrolls.SetActive(true);
        OpenCardsUpgrades();
    }

    public void Close()
    {

       _upgradeCardScreen.CloseUpgradeScreen();
       _upgradeComboScreen.CloseUpgradeScreen();
        CloseCardsUpgrades();
        CloseComboUpgrades();
        _upgradeScrolls.SetActive(false);
        _upgradeButtons.gameObject.SetActive(false);
        _upgradeCardsTitle.SetActive(false);
    }


    public void OpenCardsUpgrades()
    {
        CloseComboUpgrades();
        _titleText.text = _upgradeCardsTitleText;
        _metaCardFilter.gameObject.SetActive(true);
        _upgradeCardScreen.OnOpenUpgradeScreen();

        _upgradeCardsPanel.SetActive(true);
    }

    public void OpenCombosUpgrades()
    {
        CloseCardsUpgrades();
        _titleText.text = _upgradeCombosTitleText;
        _upgradeComboScreen.OnOpenUpgradeScreen(); 
        _metaComboFilter.gameObject.SetActive(true);
        _upgradeComboPanel.SetActive(true);
    }

    public void CloseComboUpgrades()
    {
        _metaComboFilter.gameObject.SetActive(false);
        _upgradeComboPanel.SetActive(false);

    }

    public void CloseCardsUpgrades()
    {
        _metaCardFilter.gameObject.SetActive(false);
        _upgradeCardsPanel.SetActive(false);

    }
}
