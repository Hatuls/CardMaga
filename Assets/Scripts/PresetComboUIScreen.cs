using CardMaga.Card;
using CardMaga.Keywords;
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
            _comboScreen.InitRecipe(combo.BattleComboData);
            BattleCardData battleCard = combo.BattleCardUI.BattleCardData;
   
            for (int i = 0; i < _keywordsInfo.Count; i++)
            {
                if (_keywordsInfo[i].gameObject.activeSelf)
                    _keywordsInfo[i].gameObject.SetActive(false);
            }
            SortKeywords(battleCard);
            _gameObject.SetActive(true);
        }
        private void SortKeywords(BattleCardData battleCard)
        {
            var keywords = battleCard.CardKeywords;
            List<KeywordType> list = new List<KeywordType>();



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

            if (battleCard.IsExhausted)
            {
                var lastKeyword = _keywordsInfo[_keywordsInfo.Count - 1];
                if (!lastKeyword.gameObject.activeSelf)
                    lastKeyword.gameObject.SetActive(true);
                lastKeyword.SetKeywordName("Exhaust");
                lastKeyword.SetKeywordDescription("Removed until out of combat.");

            }
        }
        public void CloseComboUIScreen() => gameObject.SetActive(false);
        private async void AssignKeywords(KeywordData[] keywords, KeywordType keywordTypeEnum, int i)
        {

            List<KeywordData> listCache = new List<KeywordData>();
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