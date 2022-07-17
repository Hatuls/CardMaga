using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace UI.Text
{
    [System.Serializable]
    public class CardDescriptionAssigner : BaseTextAssigner
    {
        [SerializeField] TextMeshProUGUI[] _keywordsDescription;
        [SerializeField] GameObject[] _rows;
        [SerializeField] DescriptionColorSO _descriptionColorSO;
        [SerializeField] bool _isNumberBold = true;
        public override void Init()
        {
            if (_keywordsDescription == null)
                throw new System.Exception("CardDescriptionAssigner has no keywordsObjects");
            if (_rows == null || _rows.Length <2)
                throw new System.Exception("CardDescriptionAssigner has no rows");
        }
        public void SetCardKeywords(List<string[]> keywords)
        {
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
                    _keywordsDescription[0].alignment =TextAlignmentOptions.Center;
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
        void ActivateKeywordObjects(int keywordsAmount)
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
        private string SetKeywordDescription(Color color,string[] keyword)
        {
            string hexaCode = GetHexaCodeFromColor(color);
            string completedString;
            //try parse index
            if (!int.TryParse(keyword[0],out int value))
            {
                completedString = "<color=#" + hexaCode + ">" + keyword[0];
            }
            else
            {
                completedString = AddBoldToNumber(keyword[0]);
                completedString += "<color=#" + hexaCode + ">" + " " + keyword[1];
            }
            return completedString;
        }
        private string AddBoldToNumber(string keyword)
        {
            if (_isNumberBold)
            {
                return "<B>" + keyword + "</B>";
            }
            else
            {
                return keyword;
            }
        }
    }
}
