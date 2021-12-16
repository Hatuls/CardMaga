using UnityEngine;
using TMPro;

public class ReciveDiamondsScript : MonoBehaviour
{
    [SerializeField]
    ushort _diamondAmount;
    [SerializeField]
    TextMeshProUGUI _diamondAmountText;
    [SerializeField]
    GameObject _rewardPanel;
    public void Start()
    {
        UpdateText();
    }
    private void UpdateText()
    {
        _diamondAmountText.text = _diamondAmount.ToString();
    }
    public void OnReciveRewards()
    {
        if(true)//check if bool is true
        {
            Account.AccountManager.Instance.AccountGeneralData.AccountResourcesData.Diamonds.AddValue(_diamondAmount);
        }
        //change bool to false
        _rewardPanel.SetActive(false);
    }

        
}
