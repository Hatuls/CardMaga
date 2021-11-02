
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MoneyIcon : MonoBehaviour
{
    [SerializeField] Image _img;
    [SerializeField] Image _backGroundIMG;
    [SerializeField] TextMeshProUGUI _moneyTxt;


    public void SetMoneyText(int amount ) => _moneyTxt.text = amount.ToString();
}
