
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MoneyIcon : MonoBehaviour
{
    [SerializeField] Image _img;
    [SerializeField] Image _backGroundIMG;
    [SerializeField] TextMeshProUGUI _moneyTxt;


    public void SetMoneyText(int amount ) => _moneyTxt.text = amount.ToString();
    // Need To be Re-Done
    // public void UpdateMoneyText() => SetMoneyText(Account.AccountManager.Instance.BattleData.Player.CharacterData.CharacterStats.Gold);
}
