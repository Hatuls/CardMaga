using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CardMaga.ImageAlpha
{
    public static class ImageAlphaHandler
    {

        private static List<ImageAlpha> _imagesAlphas = new List<ImageAlpha>();

        public static ImageAlpha GetImageAlpha(ImageAlphaID imageAlphaID)
        {
            for (int i = 0; i < _imagesAlphas.Count; i++)
            {
                if (_imagesAlphas[i].ImageAlphaID == imageAlphaID)
                {
                    return _imagesAlphas[i];
                }
            }
            throw new System.Exception("ImageAlphaHandler: ImageAlpha Was not found");
        }

        internal static void RemoveImage(ImageAlpha imageAlpha)
        {
            _imagesAlphas.Remove(imageAlpha);
        }

        public static void SetAlpha(params AlphaPackage[] alphaPackage)
        {
            for (int i = 0; i < alphaPackage.Length; i++)
            {
                Image image = GetImageAlpha(alphaPackage[i].ImageAlphaID).Image;
                Color color = image.color;
                color.a = alphaPackage[i].AlphaValue;
                image.color = color;
            }
        }

        public static void AddImage(ImageAlpha imageAlpha)
            => _imagesAlphas.Add(imageAlpha);
 
    }

    [System.Serializable]
    public class AlphaPackage
    {
        public ImageAlphaID ImageAlphaID;
        public float AlphaValue;
    }
}
