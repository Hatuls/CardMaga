using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelBarUI : MonoBehaviour
{
    [SerializeField]
    Image _fillImage;

    [SerializeField]
    TextMeshProUGUI _text;

    [SerializeField]
    LeanTweenType _leanTweenType;
    [SerializeField]
    float _expFillTimer;

    private void Start()
    {
        Account.GeneralData.EXPStat.OnGainEXP += (UpdateEXP);
        Account.GeneralData.LevelStat.OnLevelUp += (UpdateLevel);
        UpdateFillBar();
    }
    private void OnDestroy()
    {
        Account.GeneralData.EXPStat.OnGainEXP -= (UpdateEXP);
        Account.GeneralData.LevelStat.OnLevelUp -= (UpdateLevel);
    }
    // Need To be Re-Done
    public void UpdateFillBar()
    {
        //var level = Account.AccountManager.Instance.AccountGeneralData.AccountLevelData;
        //_fillImage.fillAmount = (float)((float)level.Exp.Value / (float)level.MaxEXP.Value);
        //UpdateLevel(level.Level.Value);
    }
    public void UpdateLevel(int value)
    {
        _text.text = value.ToString();
    }

    public void UpdateEXP(int value, int maxVal)
    {
        float val = _fillImage.fillAmount;
        LeanTween.value(val, (float)((float)value / (float)maxVal), _expFillTimer).setEase(_leanTweenType).setOnUpdate((f) =>  _fillImage.fillAmount = f);

    }

}
