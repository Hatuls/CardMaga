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
        if(Account.AccountManager.Instance.RewardGift.NeedToBeRewarded)
        {
            _rewardPanel.SetActive(true);
            UpdateText();
        }
    }
    private void UpdateText()
    {
        _diamondAmountText.text = _diamondAmount.ToString();
    }
    public void OnReciveRewards()
    {
        Debug.Log("Reciving Rewards");
        Account.AccountManager.Instance.AccountGeneralData.AccountResourcesData.Diamonds.AddValue(_diamondAmount);
        Account.AccountManager.Instance.RewardGift.NeedToBeRewarded = false;
        _rewardPanel.SetActive(false);
    }

        
}
