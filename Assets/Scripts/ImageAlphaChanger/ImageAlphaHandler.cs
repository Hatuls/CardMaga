using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CardMaga.ImageAlpha
{
    public class ImageAlphaHandler : MonoSingleton<ImageAlphaHandler>
    {
        [SerializeField,Sirenix.OdinInspector.ReadOnly]
        private List<ImageAlpha> _alphas = new List<ImageAlpha>();

        public ImageAlpha GetImageAlpha(ImageAlphaID imageAlphaID)
        {
            for (int i = 0; i < _alphas.Count; i++)
            {
                if (_alphas[i].ImageAlphaID == imageAlphaID)
                {
                    return _alphas[i];
                }
            }
            throw new System.Exception("ImageAlphaHandler: ImageAlpha Was not found");
        }

        public void SetAlpha(params AlphaPackage[] alphaPackage)
        {
            for (int i = 0; i < alphaPackage.Length; i++)
            {
                Image image = GetImageAlpha(alphaPackage[i].ImageAlphaID).Image;
                Color color = image.color;
                color.a = alphaPackage[i].AlphaValue;
                image.color = color;
            }
        }

        public void AddImage(ImageAlpha imageAlpha)
            => _alphas.Add(imageAlpha);
 
    }

    [System.Serializable]
    public class AlphaPackage
    {
        public ImageAlphaID ImageAlphaID;
        public float AlphaValue;
    }
}
