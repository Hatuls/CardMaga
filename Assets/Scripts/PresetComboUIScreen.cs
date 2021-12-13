using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
namespace UI
{
    public class PresetComboUIScreen : MonoBehaviour
    {
        [SerializeField]
        ComboRecipeUI _comboScreen;
        [SerializeField]
        GameObject _gameObject;

        [SerializeField]
        GameObject _keywordinfoPrefab;

        [SerializeField]
        List<KeywordInfoUI> _keywordsInfo;

        public ComboRecipeUI ComboRecipeUI => _comboScreen;

        [SerializeField]
        Transform _keywordsParent;


        [SerializeField]
        UnityEvent OnCloseEvent;

        public void OpenComboUIscreen(ComboRecipeUI combo)
        {
            _comboScreen.InitRecipe(combo.Combo);
            Cards.Card card = combo.CardUI.GFX.GetCardReference;
   
            for (int i = 0; i < _keywordsInfo.Count; i++)
            {
                if (_keywordsInfo[i].gameObject.activeSelf)
                    _keywordsInfo[i].gameObject.SetActive(false);
            }
            SortKeywords(card);
            _gameObject.SetActive(true);
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
        private void CreateKeywordInfo()
        {
            var keyword = Instantiate(_keywordinfoPrefab, _keywordsParent).GetComponent<KeywordInfoUI>();
            _keywordsInfo.Add(keyword);
        }
    }

}