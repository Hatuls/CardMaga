using UnityEngine;
using UnityEngine.UI;

namespace CardMaga.UI.Visuals
{
    [System.Serializable]
    public abstract class BaseVisualAssigner
    {
        public abstract void Init();
        public virtual void AssignColor(Image image, Color color)
        {
            image.color = color;
        }
        /// <summary>
        /// Get the color to assign after checking if alpha should be 0
        /// </summary>
        /// <param name="imageIndex"></param>
        /// <param name="colorIndex"></param>
        /// <param name="colors"></param>
        /// <returns></returns>
        public virtual Color GetColorToAssign(int imageIndex, int colorIndex, Color[] colors)
        {
            if (colors.Length < imageIndex)
            {
                Debug.LogWarning($"CardBodyPartVisualAssigner has asked image num: {imageIndex}, it is bigger than the array, taking {colors.ToString()} at location 0");
                return GetColorAlpha(colors[0], imageIndex, colorIndex);
            }
            else
            {
                return GetColorAlpha(colors[imageIndex], imageIndex, colorIndex); ;
            }
        }
        /// <summary>
        /// Get the color to assign when alpha is set by SO
        /// </summary>
        /// <param name="imageIndex"></param>
        /// <param name="colors"></param>
        /// <returns></returns>
        public virtual Color GetColorToAssign(int imageIndex, Color[] colors)
        {
            if (colors.Length < imageIndex)
            {
                Debug.LogWarning($"CardBodyPartVisualAssigner has asked image num: {imageIndex}, it is bigger than the array, taking {colors.ToString()} at location 0");
                return colors[0];
            }
            else
            {
                return colors[imageIndex];
            }
        }
        public virtual Color GetColorAlpha(Color baseColor, int imageIndex, int colorIndex)
        {
            Color color = baseColor;

            // first index meaning there should be alpha
            color.a = (colorIndex == 0) ? 0 : 1;
            return color;
        }

        public virtual Color GetColorAlpha(Color baseColor, float alpha)
        {
            Color color = baseColor;
            color.a = alpha;
            return color;
        }

        public virtual void AssignSprite(Image image, Sprite sprite)
        {
            if (image.sprite == sprite)
            {
                //we already use this sprite
                return;
            }
            image.sprite = sprite;
        }
        public virtual Sprite GetSpriteToAssign(int imageIndex, int spriteIndex, Sprite[] sprites)
        {
            if (sprites.Length <= imageIndex)
            {
                Debug.LogWarning($"CardBodyPartVisualAssigner has asked image num: {imageIndex} ,it is bigger than the array, taking {sprites[0].name} at location 0");
                return sprites[0];
            }
            else
            {
                return sprites[spriteIndex];
            }
        }

    }
}
