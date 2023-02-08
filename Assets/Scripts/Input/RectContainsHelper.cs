using UnityEngine;

namespace CardMaga.Input
{
    public class RectContainsHelper
    {
        public static bool CheckIfInRect(RectTransform rectTransform)
        {
            Vector2 localMousePosition = rectTransform.InverseTransformPoint(InputReciever.Instance.TouchScreenPosition);
            if (rectTransform.rect.Contains(localMousePosition))
            {
                return true;
            }

            return false;
        }
    }
}