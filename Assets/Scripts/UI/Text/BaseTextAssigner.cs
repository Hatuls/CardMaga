﻿using UnityEngine;

namespace CardMaga.UI.Text
{
    [System.Serializable]
    public abstract class BaseTextAssigner<T>
    {
        public abstract void Init(T type);

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

        public virtual void OnDestroy() { }
      
    }
}
