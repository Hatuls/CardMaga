using CardMaga.Keywords;
using System;
using TMPro;
using UnityEngine;

public class KeywordTextAssigner : MonoBehaviour
{
    public static event Action<KeywordTextAssigner> OnKeywordPointerDown;
    public static event Action OnKeywordPointerUp;


    public TextMeshProUGUI Text;
    private KeywordSO _keywordSO;
    public string KeywordDescription;

    public string KeywordName;
    public void AssignKeyword(KeywordSO keywordSO, string description)
    {
        KeywordDescription = description;
        _keywordSO = keywordSO;
        KeywordName = keywordSO.KeywordName;
    }
    public void AssignKeyword(string keywordSO, string description)
    {
        KeywordDescription = description;
        KeywordName = keywordSO;
    }
    public void ShowPopUp() => OnKeywordPointerDown?.Invoke(this);
    public void HidePopUp() => OnKeywordPointerUp?.Invoke();
}