using CardMaga.Card;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CardMaga.UI.Text
{
    [System.Serializable]
    public class CardDescriptionAssigner : BaseTextAssigner<CardData>
    {
        [SerializeField] TextMeshProUGUI[] _keywordsDescription;
        [SerializeField] GameObject[] _rows;
        [SerializeField] DescriptionColorSO _descriptionColorSO;
        [SerializeField] bool _isNumberBold = true;
        public override void CheckValidation()
        {
            if (_keywordsDescription == null)
                throw new System.Exception("CardDescriptionAssigner has no keywordsObjects");
            if (_rows == null || _rows.Length < 2)
                throw new System.Exception("CardDescriptionAssigner has no rows");
        }
        public override void Init(CardData cardData)
        {
            List<string[]> keywords = cardData.CardSO.CardDescription(cardData.CardLevel);

            for (int i = 0; i < keywords.Count; i++)
            {
                _keywordsDescription[i].text = SetKeywordDescription(_descriptionColorSO.DescriptionColor, keywords[i]);
            }
            ActivateKeywordObjects(keywords.Count);
            SetTextAllignment(keywords.Count);
        }
        private void SetTextAllignment(int keywordsAmount)
        {
            switch (keywordsAmount)
            {
                case 1:
                    _keywordsDescription[0].alignment = TextAlignmentOptions.Center;
                    break;
                case 2:
                    _keywordsDescription[0].alignment = TextAlignmentOptions.Center;
                    _keywordsDescription[1].alignment = TextAlignmentOptions.Center;
                    break;
                case 3:
                    _keywordsDescription[0].alignment = TextAlignmentOptions.Left;
                    _keywordsDescription[1].alignment = TextAlignmentOptions.Center;
                    _keywordsDescription[2].alignment = TextAlignmentOptions.Right;
                    break;
                case 4:
                    _keywordsDescription[0].alignment = TextAlignmentOptions.Left;
                    _keywordsDescription[1].alignment = TextAlignmentOptions.Left;
                    _keywordsDescription[2].alignment = TextAlignmentOptions.Right;
                    _keywordsDescription[3].alignment = TextAlignmentOptions.Right;
                    break;
                default:
                    break;
            }
        }
        private void ActivateKeywordObjects(int keywordsAmount)
        {
            //activate objects
            for (int i = 0; i < _keywordsDescription.Length; i++)
            {
                if (i < keywordsAmount)
                {
                    _keywordsDescription[i].gameObject.SetActive(true);
                }
                else
                {
                    _keywordsDescription[i].gameObject.SetActive(false);
                }
            }
            if (keywordsAmount == 1)
            {
                _rows[1].SetActive(false);
            }
            else
            {
                _rows[1].SetActive(true);
            }
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
        }
    }
}