using UnityEngine;
using UnityEngine.UI;
namespace UI
{
    public class TabUI : MonoBehaviour
    {
        [SerializeField]
        TabHandler _tabHandler;
        [SerializeField]
        TabAbst _tabToOpen;

        [SerializeField]
        Image _innerImage;
        [SerializeField]
        Image _bgImage;
        public Image BackGroundImage => _bgImage;

        [SerializeField]
        Sprite _onSprite;
 
        [SerializeField]
        Sprite _offSprite;
        public void Open()
        {
            _tabHandler.CloseAllOtherTabsThan(this);
            _tabToOpen.Open();
            _innerImage.sprite = _onSprite;
        }
        public void Close()
        {
            _tabToOpen.Close();
            _innerImage.sprite = _offSprite;
        
        }
    }
}
