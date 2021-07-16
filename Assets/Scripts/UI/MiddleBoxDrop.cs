
using UnityEngine;
using Characters.Stats;
using Battles.Deck;
using UnityEngine.EventSystems;
namespace Battles.UI
{
    [RequireComponent(typeof(EventTrigger))]
    public class MiddleBoxDrop : MonoBehaviour { 
        [SerializeField] BoxCollider2D boxCollider2D;
        [SerializeField] Unity.Events.SoundsEvent _playSound;

        public void OnPointerExit() => CardUIManager.Instance.IsTryingToPlace = false;
        public void OnDrop()
        {
            if (CardUIManager.Instance.GetClickedCardUI != null)
            {
                Cards.Card card = CardUIManager.Instance.GetClickedCardUI.GFX.GetCardReference;


                if (StaminaHandler.IsEnoughStamina(card) == false)
                {
                    // not enough stamina 
                    DeckManager.Instance.TransferCard(DeckEnum.Selected, DeckEnum.Hand, card);
                    CardUIManager.Instance.AddToHandUI(CardUIManager.Instance.GetClickedCardUI);
                    _playSound?.Raise(SoundsNameEnum.Reject);
                    CardUIManager.Instance.GetClickedCardUI = null;
                    return;
                }
                else
                {
                    CardUIManager.Instance.IsTryingToPlace = true;
                    // execute card
                    DeckManager.Instance.TransferCard(DeckEnum.Selected, DeckEnum.Disposal, card);
                    DeckManager.AddToCraftingSlot(card);
                    CardExecutionManager.Instance.RegisterCard(card);
                    StaminaHandler.ReduceStamina(card);
                }


                // reset the holding card
                CardUIManager.Instance.StartRemoveProcess();
            }
            
        }


    }

}

