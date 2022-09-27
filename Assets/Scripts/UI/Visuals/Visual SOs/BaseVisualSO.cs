using UnityEngine;

namespace CardMaga.UI
{
    public abstract class BaseVisualSO : ScriptableObject, ICheckValidation
    {
        /// <summary>
        /// Get the color to assign after checking if alpha should be 0
        /// </summary>
        /// <param name="imageIndex"></param>
        /// <param name="colorIndex"></param>
        /// <param name="colors"></param>
        /// <returns></returns>
        public static Color GetColorToAssign(int imageIndex, int colorIndex, Color[] colors)
        {
            if (colors.Length < imageIndex)
            {
                Debug.LogWarning($"BaseVisualSO has asked image num: {imageIndex}, it is bigger than the array, taking {colors.ToString()} at location 0");
                return GetColorAlpha(colors[0], colorIndex);
            }
            else
            {
                return GetColorAlpha(colors[imageIndex], colorIndex); ;
            }
        }

        /// <summary>
        /// Get the color to assign when alpha is set by SO
        /// </summary>
        /// <param name="imageIndex"></param>
        /// <param name="colors"></param>
        /// <returns></returns>
        public static Color GetColorToAssign(int imageIndex, Color[] colors)
        {
            if (colors.Length < imageIndex)
            {
                Debug.LogWarning($"BaseVisualSO has asked image num: {imageIndex}, it is bigger than the array, taking {colors.ToString()} at location 0");
                return colors[0];
            }
            else
            {
                return colors[imageIndex];
            }
        }
        public static Color GetColorAlpha(Color baseColor, int colorIndex)
        {
            Color color = baseColor;

            // first index meaning there should be alpha
            color.a = (colorIndex == 0) ? 0 : 1;
            return color;
        }
        public static Sprite GetSpriteToAssign(int imageIndex, int spriteIndex, Sprite[] sprites)
        {
            if (sprites.Length <= imageIndex)
            {
                Debug.LogWarning($"BaseVisualSO has asked image num: {imageIndex} ,it is bigger than the array, taking {sprites[0].name} at location 0");
                return sprites[0];
            }
            else
            {
                return sprites[spriteIndex];
            }
        }

        public abstract void CheckValidation();
    }
}
