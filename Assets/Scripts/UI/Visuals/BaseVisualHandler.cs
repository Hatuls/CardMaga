using System;
using UnityEngine;
using UnityEngine.UI;

namespace CardMaga.UI
{
    [Serializable]
    public abstract class BaseVisualHandler<T> : IDisposable, ICheckValidation
    {
        public abstract void CheckValidation();
        public abstract void Init(T data);
        public abstract void Dispose();
    }

    public interface ICheckValidation
    {
        void CheckValidation();
    }
    public static class TextMeshProUGUIHelper
    {
        public static void AssignText(this TMPro.TextMeshProUGUI textHolder, string text)
        {
            textHolder.text = text;
        }
    }
    public static class StringHelper
    {
        private const string COLOR_HTML = "<color=#";
        private const string COLOR_HTML_CLOSER = "</color>";
        private const string HTML_CLOSER = ">";
        private const string HTML_BOLD = "<b>";
        private const string HTML_BOLD_CLOSER = "</b>";
        public static string ToHexa(this Color color)
         => ColorUtility.ToHtmlStringRGB(color);
        public static string ToBold(this string text) => string.Concat(HTML_BOLD, text, HTML_BOLD_CLOSER);
        public static string ColorString(this string text, Color color)
            => string.Concat(COLOR_HTML, color.ToHexa(), HTML_CLOSER, text, COLOR_HTML_CLOSER);
    }
    public static class ColorHelper
    {
        public static Color GetColorAlpha(this Color baseColor, float alpha)
        {
            Color color = baseColor;
            color.a = alpha;
            return color;
        }
    }
    public static class ImageHelper
    {
        public static void AssignColor(this Image image, Color color)
        {
            image.color = color;
        }
        public static void AssignSprite(this Image image, Sprite sprite)
        {
            if (image.sprite == sprite)
            {
                //we already use this sprite
                return;
            }
            image.sprite = sprite;
        }
    }
}
