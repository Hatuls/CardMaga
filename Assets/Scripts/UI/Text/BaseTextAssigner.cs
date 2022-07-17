using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Text
{
    [System.Serializable]
    public abstract class BaseTextAssigner
    {
        public abstract void Init();

        public virtual void AssignText(TMPro.TextMeshProUGUI textHolder, string text)
        {
            textHolder.text = text;
        }
        public string GetHexaCodeFromColor(Color keywordsColor)
        {
            string hexaCode;
            hexaCode = ColorUtility.ToHtmlStringRGB(keywordsColor);
            Debug.Log("HexaCode: " + hexaCode);
            return hexaCode;
        }
    }
}
