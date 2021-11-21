using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelBarUI : MonoBehaviour
{
    [SerializeField]
    Image _fillImage;

    [SerializeField]
    TextMeshProUGUI _text;
    private void Start()
    {
        Account.GeneralData.EXPStat.OnGainEXP += (UpdateEXP);
        Account.GeneralData.LevelStat.OnLevelUp += (UpdateLevel);
        UpdateFillBar();
    }
    private void OnDestroy()
    {
        Account.GeneralData.EXPStat.OnGainEXP -=(UpdateEXP);
        Account.GeneralData.LevelStat.OnLevelUp -= (UpdateLevel);
    }
    public void UpdateFillBar()
    {
        var level = Account.AccountManager.Instance.AccountGeneralData.AccountLevelData;
        UpdateEXP((int)level.Exp.Value , (int)level.MaxEXP.Value);
        UpdateLevel( level.Level.Value);
    }
    public void UpdateLevel(int value)
    {
        _text.text = value.ToString();
    }

    public void UpdateEXP(int value, int maxVal)
    {
  
        _fillImage.fillAmount = (float)((float)value / (float)maxVal);

    }

}
