using Battles.UI;
using System;
using UnityEngine;

namespace UI.Meta.Laboratory
{
    public enum CardPanelLocationEnum
    {
        Collection = 0,
        Deck = 1,
        Fuse = 2,
        Upgrade = 3,
    }

    public class MetaCardUIHandler : MonoBehaviour
    {
        public static event Action OnCardClickedEvent;
        [SerializeField]
        GameObject _dropList;
        [SerializeField]
        GameObject _useButton;
        [SerializeField]
        GameObject _dismantleButton;
        [SerializeField]
        CardPanelLocationEnum _cardPanelLocation;
        [SerializeField]
        CardUI _cardUI;

        CardPanelState _currentState;

        public CardUI CardUI => _cardUI;

        private void Awake()
        {
            OnCardClickedEvent += CloseDropList;
            InitState();
        }
        private void InitState()
        {
            switch (_cardPanelLocation)
            {
                case CardPanelLocationEnum.Collection:
                    _currentState = new CardUICollectionState();
                    break;
                case CardPanelLocationEnum.Deck:
                    _currentState = new CardUIDeckState();
                    break;
                case CardPanelLocationEnum.Fuse:
                    _currentState = new CardUIFuseState();
                    break;
                case CardPanelLocationEnum.Upgrade:
                    _currentState = new CardUIUpgradeState();
                    break;
                default:
                    throw new Exception("MetaCardUIHandler Unknown State");
            }
        }
        public void CloseDropList()
        {
            _dropList.SetActive(false);
            _currentState.OnReset();
        }
        public void OpenDropList()
        {
            _dropList.SetActive(true);
            switch (_cardPanelLocation)
            {
                case CardPanelLocationEnum.Collection:

                    _useButton.SetActive(true);
                    _dismantleButton.SetActive(false);
                    break;
                case CardPanelLocationEnum.Deck:
                    _dismantleButton.SetActive(false);
                    break;
                case CardPanelLocationEnum.Fuse:
                    _dismantleButton.SetActive(false);
                    break;
                case CardPanelLocationEnum.Upgrade:
                    _dismantleButton.SetActive(true);
                    _useButton.SetActive(false);

                    break;
                default:
                    break;
            }
        }
        public void OnCardClicked()
        {
            Debug.Log("Changeing Drop List State");
            if (_dropList.activeSelf)
            {
                CloseDropList();
            }
            else
            {
                OnCardClickedEvent.Invoke();
                _currentState.OnClick();
            }
        }
        private void OnDestroy()
        {
            OnCardClickedEvent -= CloseDropList;
        }
    }
    public abstract class CardPanelState : ICardPanelState
    {
        public virtual void OnReset()
        {
            
        }
        public abstract void OnClick();
    }
    public class CardUICollectionState : CardPanelState
    {
        public override void OnClick()
        {
            throw new NotImplementedException();
        }
    }
    public class CardUIDeckState : CardPanelState
    {
        public override void OnClick()
        {
            throw new NotImplementedException();
        }
    }
    public class CardUIUpgradeState : CardPanelState
    {
        public override void OnClick()
        {
            throw new NotImplementedException();
        }
    }
    public class CardUIFuseState : CardPanelState
    {
        public override void OnClick()
        {
            throw new NotImplementedException();
        }
    }
    public interface ICardPanelState
    {
        void OnClick();
        void OnReset();
    }

}
