using Battles.Deck;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Battles.UI
{

    public class MiddleBoxDrop : MonoBehaviour { 
        [SerializeField] BoxCollider2D boxCollider2D;
        [SerializeField] PlaceholderUI _playerPlaceHolder;
        public void OnDrop()
        {
            Debug.Log("<a>*******************</a>");
            if (CardUIManager.Instance.GetClickedCardUI != null)
            {

                var placeHolder = _playerPlaceHolder.TryGetEmptyPlaceHolderSlotUI();

                if (placeHolder != null)
                {
                    DeckManager.Instance.TransferCard(DeckEnum.Hand, DeckEnum.Selected, CardUIManager.Instance.GetClickedCardUI.GetCardReference, placeHolder.GetSlotID);

                    PlaceHolderHandler.Instance.PlaceOnPlaceHolder(placeHolder, CardUIManager.Instance.GetClickedCardUI.GetCardReference);
                }
            }
         
        }


    }
}

