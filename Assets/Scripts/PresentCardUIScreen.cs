using Battles.UI;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UI.Meta.Laboratory;
using UnityEngine;
using UnityEngine.Events;
namespace UI
{
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
        public CardUI CardUI => _cardUI;
        [SerializeField]
        Transform _keywordsParent;

        [SerializeField]
        UnityEvent OnCloseEvent;

        public void OpenCardUIInfo(CardUI cardUI)
        {

            Cards.Card card = cardUI.GFX.GetCardReference;
            _cardUI.GFX.SetCardReference(card);
            for (int i = 0; i < _keywordsInfo.Count; i++)
            {
                if (_keywordsInfo[i].gameObject.activeSelf)
                    _keywordsInfo[i].gameObject.SetActive(false);
            }
            _gameObject.SetActive(true);
            SortKeywords(card);
        }
        public void CloseCardUIInfo()
        {
            OnCloseEvent?.Invoke();
            _gameObject.SetActive(false);
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

        internal void SubScribe(bool toAssign, MetaCardUIHandler[] _cards)
        {
            if (toAssign)
            {
                for (int i = 0; i < _cards.Length; i++)
                {

                    _cards[i].OnCardUIClicked += OpenCardUIInfo;
                    _cards[i].ToOnlyClickCardUIBehaviour = true;
                }
            }
            else
            {

                for (int i = 0; i < _cards.Length; i++)
                {
                    _cards[i].OnCardUIClicked -= OpenCardUIInfo;
                    _cards[i].ToOnlyClickCardUIBehaviour = false;
                }
            }
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
            bool toIgnore = false;

            for (int j = 0; j < listCache.Count; j++)
            {
                if (listCache[j].KeywordSO.IgnoreInfoAmount)
                {

                    toIgnore = true;
                    break;
                }
                // amount += listCache[j].GetAmountToApply;
                amount += listCache[j].GetAmountToApply;
            }

            _keywordsInfo[i].SetKeywordDescription(listCache[0].KeywordSO.GetDescription(amount));
            await Task.Yield();
            if (!_keywordsInfo[i].gameObject.activeSelf)
                _keywordsInfo[i].gameObject.SetActive(true);
        }
    }

}