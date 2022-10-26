using UnityEngine;
using UnityEngine.UI;

namespace CardMaga.ImageAlpha
{
    public class ImageAlpha : MonoBehaviour
    {
        [SerializeField]
        private ImageAlphaID _imageAlphaID;
        [SerializeField]
        private Image _image;
        public virtual Image Image => _image;

        public ImageAlphaID ImageAlphaID => _imageAlphaID;

        private void Start()
        {
            ImageAlphaHandler.Instance.AddImage(this);
        }

        private void OnDestroy()
        {
            ImageAlphaHandler.Instance.RemoveImage(this);
        }
    }
}

