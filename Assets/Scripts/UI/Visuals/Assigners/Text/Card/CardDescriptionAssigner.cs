using Account.GeneralData;
using CardMaga.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace CardMaga.UI.Text
{
    [System.Serializable]
    public class CardDescriptionAssigner : BaseTextAssigner<CardCore>
    {
        [SerializeField]
        private CardZoomHandler _cardZoomHandler;
        [SerializeField] private KeywordTextAssigner[] _keywordTextAssigners;
       // [SerializeField] private TextMeshProUGUI[] _keywordsDescription;
        [SerializeField] private GameObject[] _rows;
        [SerializeField] private DescriptionColorSO _descriptionColorSO;
        [SerializeField] private bool _isNumberBold = true;
        private int _descriptoins;
        private bool _flag;
        public override void CheckValidation()
        {
            if (_keywordTextAssigners == null)
                throw new System.Exception("CardDescriptionAssigner has no KeywordTextAssigners");
            if (_rows == null || _rows.Length < 2)
                throw new System.Exception("CardDescriptionAssigner has no rows");

     
        }
        public override void Init(CardCore comboData)
        {
            if (_flag == true)
            {
                _cardZoomHandler.OnZoomInStarted -= ActivateKeywordObjects;
                _cardZoomHandler.OnZoomOutStarted -= HideTexts;
            }
            else
                _flag = true;

            _cardZoomHandler.OnZoomInStarted += ActivateKeywordObjects;
            _cardZoomHandler.OnZoomOutStarted += HideTexts;
            List<string[]> keywords = comboData.CardSO.CardDescription(comboData.Level);
            List<Keywords.KeywordData> data = comboData.CardSO.GetCardsKeywords(comboData.Level);

            bool hasExhaust = comboData.CardSO().GetLevelUpgrade(comboData.Level).UpgradesPerLevel.First(x => x.UpgradeType == LevelUpgradeEnum.ToRemoveExhaust).Amount == 1;

            if (_keywordTextAssigners.Length == 0)
                _keywordTextAssigners = _rows[0].transform.parent.GetComponentsInChildren<KeywordTextAssigner>();
            try
            {

            for (int i = 0; i < keywords.Count; i++)
            {
                KeywordTextAssigner current = _keywordTextAssigners[i];

                if (i == keywords.Count - 1 && hasExhaust)
                    current.AssignKeyword("Exhaust", "Exhausted cards will be removed for the rest of the game after played.");
                else
                    current.AssignKeyword(data[i].KeywordSO, data[i].KeywordSO.GetDescription(data[i].GetAmountToApply));// new

                current.Text.text = SetKeywordDescription(_descriptionColorSO.DescriptionColor, keywords[i]); // new
            }
            }

            catch (Exception e)
            {
                Debug.LogError(comboData.CardSO.CardName);
                throw e;
            }
            _descriptoins = keywords.Count;

            HideTexts();
            SetTextAllignment(keywords.Count);
        }
        public void HideTexts() => Array.ForEach(_keywordTextAssigners, x => x.gameObject.SetActive(false));
        private void SetTextAllignment(int keywordsAmount)
        {
            switch (keywordsAmount)
            {
                case 1:
                    //_keywordsDescription[0].alignment = TextAlignmentOptions.Center;

                    _keywordTextAssigners[0].Text.alignment = TextAlignmentOptions.Center; // new
                    break;
                case 2:
                    //_keywordsDescription[0].alignment = TextAlignmentOptions.Center;
                    //_keywordsDescription[1].alignment = TextAlignmentOptions.Center;

                    _keywordTextAssigners[0].Text.alignment = TextAlignmentOptions.Center;        // new
                    _keywordTextAssigners[1].Text.alignment = TextAlignmentOptions.Center;        // new
                    break;
                case 3:
                    //_keywordsDescription[0].alignment = TextAlignmentOptions.Left;
                    //_keywordsDescription[1].alignment = TextAlignmentOptions.Center;
                    //_keywordsDescription[2].alignment = TextAlignmentOptions.Right;

                    _keywordTextAssigners[0].Text.alignment = TextAlignmentOptions.Left;     // new
                    _keywordTextAssigners[1].Text.alignment = TextAlignmentOptions.Center;   // new
                    _keywordTextAssigners[2].Text.alignment = TextAlignmentOptions.Right;    // new
                    break;
                case 4:
                    //_keywordsDescription[0].alignment = TextAlignmentOptions.Left;
                    //_keywordsDescription[1].alignment = TextAlignmentOptions.Left;
                    //_keywordsDescription[2].alignment = TextAlignmentOptions.Right;
                    //_keywordsDescription[3].alignment = TextAlignmentOptions.Right;

                    _keywordTextAssigners[0].Text.alignment = TextAlignmentOptions.Left;     // new
                    _keywordTextAssigners[1].Text.alignment = TextAlignmentOptions.Left;     // new
                    _keywordTextAssigners[2].Text.alignment = TextAlignmentOptions.Right;    // new
                    _keywordTextAssigners[3].Text.alignment = TextAlignmentOptions.Right;    // new
                    break;
                default:
                    break;
            }
        }
        public void ActivateKeywordObjects()
        {
            int keywordsAmount = _descriptoins;
            //activate objects
            for (int i = 0; i < _keywordTextAssigners.Length; i++)
            {

                _keywordTextAssigners[i].gameObject.SetActive(i < keywordsAmount);
                //if (i < keywordsAmount)
                //{
                //    _keywordsDescription[i].gameObject.SetActive(true);
                //}
                //else
                //{
                //    _keywordsDescription[i].gameObject.SetActive(false);
                //}
            }

            _rows[1].SetActive(keywordsAmount != 1);
            //if (keywordsAmount == 1)
            //{
            //    _rows[1].SetActive(false);
            //}
            //else
            //{
            //    _rows[1].SetActive(true);
            //}
        }
        private string SetKeywordDescription(Color color, string[] keyword)
        {
           // string hexaCode = GetHexaCodeFromColor(color);
            string completedString;
            //try parse index
            if (!int.TryParse(keyword[0], out int value))
            {
                completedString = keyword[0].ColorString(color);
              //  completedString = "<color=#" + hexaCode + ">" + keyword[0];
            }
            else
            {
                completedString = AddBoldToNumber(keyword[0]);
                completedString += keyword[1].ColorString(color); 
            }
            return completedString;
        }
        private string AddBoldToNumber(string keyword)
        {
            if (_isNumberBold)
            {
                // return "<B>" + keyword + "</B>";
                 return  keyword.ToBold();
            }
            else
            {
                return keyword;
            }
        }
        public override void Dispose()
        {
            _cardZoomHandler.OnZoomInStarted -= ActivateKeywordObjects;
            _cardZoomHandler.OnZoomOutStarted -= HideTexts;
        }
    }
}