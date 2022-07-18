using Battles.UI;
using System;
using System.Collections.Generic;
using System.Linq;
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
        BuffIconInteraction.OnRelease += CloseCardUIInfo;
        BuffIconInteraction.OnBuffIconHold += ShowKeyword;
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
                UnityAnalyticHandler.SendEvent("Looking At Cards Keywords In Battle");
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
        SortKeywords(card.CardData);
    }

    public void CloseCardUIInfo()
    {
        toStartCounting = false;
        _timer = 0;
        _zoomingCard = null;
        _keywordsContainer.SetActive(false);
        _keywordsInfo.ForEach(x => { 
            if (x.gameObject.activeSelf)
                x.gameObject.SetActive(false); 
        });
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

        _keywordsInfo.ForEach(x => x.gameObject.SetActive(false));

        for (int i = 0; i <= list.Count; i++)
        {
            if (i == list.Count)
                break;
            AssignKeywords(keywords, list[i]);

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

    private KeywordInfoUI GetEmptyKeywordInfoUI()
    {
        for (int i = 0; i < _keywordsInfo.Count; i++)
        {
            if (_keywordsInfo[i].gameObject.activeSelf == false)
                return _keywordsInfo[i];
        }
        return CreateKeywordInfo();
    }

    private KeywordInfoUI CreateKeywordInfo()
    {
        var keyword = Instantiate(_keywordinfoPrefab, _keywordsContainer.transform).GetComponent<KeywordInfoUI>();
        _keywordsInfo.Add(keyword);
        return keyword;
    }

    private async void AssignKeywords(Keywords.KeywordData[] keywords, Keywords.KeywordTypeEnum keywordTypeEnum)
    {

        List<Keywords.KeywordData> listCache = new List<Keywords.KeywordData>();
        listCache = keywords.Where((x) => x.KeywordSO.GetKeywordType == keywordTypeEnum).ToList();

        var keywordInfo = GetEmptyKeywordInfoUI();
        keywordInfo.SetKeywordName(keywordTypeEnum.ToString());
        //find the keyword Description and insert int with the corrent amount

        int amount = 0;


        for (int j = 0; j < listCache.Count; j++)
        {
            if (listCache[j].KeywordSO.IgnoreInfoAmount)
                break;

            // amount += listCache[j].GetAmountToApply;
            amount += listCache[j].GetAmountToApply;
        }

        keywordInfo.SetKeywordDescription(listCache[0].KeywordSO.GetDescription(amount));
        //  await Task.Yield();
        if (!keywordInfo.gameObject.activeSelf)
            keywordInfo.gameObject.SetActive(true);
    }
    public void ShowKeyword(Keywords.KeywordTypeEnum keywordTypeEnum)
    {
        /* 
         * set the container to true!
         * need to get empty keywordInfo
         * get from the keyword enum the keyword so
        *  get keyword description from SO
        *  set the keywordinfo description
        *  set the keywordinfo active state to true
         */
    
        _keywordsContainer.SetActive(true);
        var keywordSO = Factory.GameFactory.Instance.KeywordSOHandler.GetKeywordSO(keywordTypeEnum);
        KeywordInfoUI keywordInfo = GetEmptyKeywordInfoUI();
        keywordInfo.SetKeywordDescription(keywordSO.GetDescription(0));
        keywordInfo.SetKeywordName(keywordTypeEnum.ToString());
        keywordInfo.gameObject.SetActive(true);

    }
    internal void CheckCardUI(CardUI card)
    {
        if (card == null)
            throw new Exception("GameBattleDescriptionUI : CardUI Is null!");

        _zoomingCard = card;
        toStartCounting = true;
    }

    private void OnDestroy()
    {
        BuffIconInteraction.OnRelease -= CloseCardUIInfo;
        BuffIconInteraction.OnBuffIconHold -= ShowKeyword;
    }
}

