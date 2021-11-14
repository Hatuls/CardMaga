using Battles.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class PresentCardUIScreen : MonoBehaviour
{
    [SerializeField]
    GameObject _gameObject;
    [SerializeField]
    CardUI _cardUI;
    [SerializeField]
    GameObject _keywordinfoPrefab;
    [SerializeField]
    List<KeywordInfoUI> _keywordsInfo;

    [SerializeField]
    Transform _keywordsParent;

  

    public void OpenCardUIInfo(CardUI cardUI, PointerEventData data)
    {

        var art = Factory.GameFactory.Instance.ArtBlackBoard;
        Cards.Card card = cardUI.GFX.GetCardReference;
        _cardUI.GFX.SetCardReference(card, art);
        for (int i = 0; i < _keywordsInfo.Count; i++)
        {
            if (_keywordsInfo[i].gameObject.activeSelf)
            _keywordsInfo[i].gameObject.SetActive(false);
        }     
        _gameObject.SetActive(true);
        SortKeywords(card);
    }
    public void CloseCardUIInfo()
        => _gameObject.SetActive(false);
    private void SortKeywords(Cards.Card card)
    {
        var keywords = card.CardKeywords;
        List<Keywords.KeywordTypeEnum> list = new List<Keywords.KeywordTypeEnum>();

        foreach (var keyword in keywords)
        {
            var keywordType = keyword.KeywordSO.GetKeywordType;
            if (list.Contains(keywordType) == false)
                list.Add(keywordType);
        }

        while (_keywordsInfo.Count < list.Count)
            CreateKeywordInfo();

        for (int i = 0; i <= list.Count; i++)
        {
     

            if (i == list.Count)
                return;
            AssignKeywords(keywords, list[i], i);
        }

        if (card.IsExhausted)
        {
            var lastKeyword = _keywordsInfo[_keywordsInfo.Count - 1];
            if (lastKeyword.gameObject.activeInHierarchy)
                lastKeyword.gameObject.SetActive(true); 
            lastKeyword.SetKeywordName("Exhaust");
            lastKeyword.SetKeywordDescription("Can Be Played Once Per Battle");

        }
    }

    internal void SubScribe(bool toAssign, CardUI[] _cards)
    {
        if (toAssign)
        {
            for (int i = 0; i < _cards.Length; i++)
                _cards[i].Inputs.OnPointerClickEvent +=OpenCardUIInfo;
        }
        else
            for (int i = 0; i < _cards.Length; i++)
                _cards[i].Inputs.OnPointerClickEvent -= OpenCardUIInfo;
    }

    private void CreateKeywordInfo()
    {
        var keyword = Instantiate(_keywordinfoPrefab, _keywordsParent).GetComponent<KeywordInfoUI>();
        _keywordsInfo.Add(keyword);
    }

    private async void AssignKeywords(Keywords.KeywordData[] keywords, Keywords.KeywordTypeEnum keywordTypeEnum, int i)
    {
   
        List<Keywords.KeywordData> listCache = new List<Keywords.KeywordData>();
        listCache = keywords.Where((x) => x.KeywordSO.GetKeywordType == keywordTypeEnum).ToList();

    
        _keywordsInfo[i].SetKeywordName(keywordTypeEnum.ToString());
        //find the keyword Description and insert int with the corrent amount

        int amount = 0;
        for (int j = 0; j < keywords.Length; j++)
            amount += keywords[j].GetAmountToApply;

        _keywordsInfo[i].SetKeywordDescription(amount.ToString());
        await Task.Yield();  
        if (!_keywordsInfo[i].gameObject.activeSelf)
            _keywordsInfo[i].gameObject.SetActive(true);
    }
}
