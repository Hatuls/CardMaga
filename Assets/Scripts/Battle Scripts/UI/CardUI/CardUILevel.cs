using UnityEngine;
using UnityEngine.UI;
namespace Battle.UI.CardUIAttributes
{
    public class CardUILevel :MonoBehaviour
    {
        [SerializeField]
        CardUILevelSO _artBank;
        [SerializeField] GameObject _parent;
        [SerializeField] Image _innerLevel;

        public void SetActiveState(bool state)
            => _parent.SetActive(state);
        public void SetState(CardUILevelState state) 
        {
            switch (state)
            {
                default:
                case CardUILevelState.Off:
                    _innerLevel.sprite = _artBank.OffImage;
                    break;
                case CardUILevelState.On:
                    _innerLevel.sprite = _artBank.OnImage;
                    break;
                case CardUILevelState.Missing:
                    _innerLevel.sprite = _artBank.MissingImage;
                    break;
                
            }
        }
    }
}