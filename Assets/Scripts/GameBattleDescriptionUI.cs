using Battles.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class GameBattleDescriptionUI : MonoBehaviour
{
    public static GameBattleDescriptionUI Instance;
    [SerializeField]
    List<KeywordInfoUI> _keywordsInfo;
    [SerializeField]
    GameObject _keywordsContainer;

    CardUI _zoomingCard;


    private bool toStartCounting;

    float _timer;
    [SerializeField] float _maxTime;

    [SerializeField] GameObject _keywordinfoPrefab;
    void Start()
    {

        Instance = this;
        CloseCardUIInfo();
    }


    private void Update()
    {
        if (toStartCounting)
        {
            if (_timer >= _maxTime)
            {
                ShowDescription(_zoomingCard);
                _timer = 0;
                AnalyticsHandler.SendEvent("Looking At Cards Keywords In Battle");
                toStartCounting = false;
                return;
            }
            _timer += Time.deltaTime;
        }
    }
    public void ShowDescription(CardUI card)
    {
        for (int i = 0; i < _keywordsInfo.Count; i++)
        {
            if (_keywordsInfo[i].gameObject.activeSelf)
                _keywordsInfo[i].gameObject.SetActive(false);
        }
        _keywordsContainer.SetActive(true);
        SortKeywords(card.GFX.GetCardReference);
    }

    public void CloseCardUIInfo()
    {
        toStartCounting = false;
        _timer = 0;
        _zoomingCard = null;
        _keywordsContainer.SetActive(false);
    }
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

        _keywordsInfo.ForEach(x => x.gameObject.SetActive(false));

        for (int i = 0; i <= list.Count; i++)
        {
            if (i == list.Count)
                break;
            AssignKeywords(keywords, list[i], i);

        }

        if (card.IsExhausted)
        {
            var lastKeyword = _keywordsInfo[_keywordsInfo.Count - 1];
            if (!lastKeyword.gameObject.activeSelf)
                lastKeyword.gameObject.SetActive(true);
            lastKeyword.SetKeywordName("Exhaust");
            lastKeyword.SetKeywordDescription("Removed until out of combat.");

        }
    }

  

    private void CreateKeywordInfo()
    {
        var keyword = Instantiate(_keywordinfoPrefab, _keywordsContainer.transform).GetComponent<KeywordInfoUI>();
        _keywordsInfo.Add(keyword);
    }

    private async void AssignKeywords(Keywords.KeywordData[] keywords, Keywords.KeywordTypeEnum keywordTypeEnum, int i)
    {

        List<Keywords.KeywordData> listCache = new List<Keywords.KeywordData>();
        listCache = keywords.Where((x) => x.KeywordSO.GetKeywordType == keywordTypeEnum).ToList();


        _keywordsInfo[i].SetKeywordName(keywordTypeEnum.ToString());
        //find the keyword Description and insert int with the corrent amount

        int amount = 0;


        for (int j = 0; j < listCache.Count; j++)
        {
            if (listCache[j].KeywordSO.IgnoreInfoAmount)
                break;
            
            // amount += listCache[j].GetAmountToApply;
            amount += listCache[j].GetAmountToApply;
        }

        _keywordsInfo[i].SetKeywordDescription(listCache[0].KeywordSO.GetDescription(amount));
        await Task.Yield();
        if (!_keywordsInfo[i].gameObject.activeSelf)
            _keywordsInfo[i].gameObject.SetActive(true);
    }

    internal void CheckCardUI(CardUI card)
    {
        if (card == null)
            throw new Exception("GameBattleDescriptionUI : CardUI Is null!");

        _zoomingCard = card;
        toStartCounting = true;
    }
}

