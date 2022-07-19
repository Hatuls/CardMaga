using CardMaga.UI.Card;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Meta.Laboratory
{

    public class MetaCardUIInteractionPanel : MonoBehaviour
    {

        public static Action OnOpenInteractionScreen;
        public Action<CardUI> OnRemoveEvent;
        public Action<CardUI> OnDismentalEvent;
        public Action<CardUI> OnUseEvent;
        public Action<CardUI> OnInfoEvent;
        public Action<CardUI> OnBuyEvent;
        [SerializeField]
        MetaCardUIHandler _metacardUI;

        [SerializeField]
        GameObject _container;
        [SerializeField]
        CardUIInteractionButton _selectBtn;
        [SerializeField]
        CardUIInteractionButton _infoBtn;
        [SerializeField]
        CardUIInteractionButton _dismentalBtn;
        [SerializeField]
        CardUIInteractionButton _removeBtn;
        [SerializeField]
        CardUIInteractionBuyButtom _buyBtn;

        [SerializeField]
        TextMeshProUGUI _moneyText;
        [SerializeField]
        Image _moneyIcon;
        [SerializeField]

        MetaCardUiInteractionEnum _state;

        public CardUIInteractionBuyButtom BuyBtn { get => _buyBtn; }

 
        public void ResetEnum()
        {
            _state = MetaCardUiInteractionEnum.None;
            ResetInteraction();
        }
        public void ResetInteraction()
        {
            OnRemoveEvent = null;
            OnDismentalEvent = null;
            OnUseEvent = null;
            OnInfoEvent = null;
        }

        private void OnEnable()
        {
            OnOpenInteractionScreen += ClosePanel;
        }
        private void OnDisable()
        {
            OnOpenInteractionScreen -= ClosePanel;
        }

        public void SetClickFunctionality(MetaCardUiInteractionEnum metaCardUiInteractionEnum, Action<CardUI> action = null)
        {
            switch (metaCardUiInteractionEnum)
            {

                case MetaCardUiInteractionEnum.Info:
                    if (action != null)
                        OnInfoEvent += action;

                    break;
                case MetaCardUiInteractionEnum.Remove:
                    if (action != null)
                        OnRemoveEvent += action;
                    break;
                case MetaCardUiInteractionEnum.Use:
                    if (action != null)
                        OnUseEvent += action;
                    break;
                case MetaCardUiInteractionEnum.Dismental:
                    if (action != null)
                        OnDismentalEvent += action;
                    break;
                case MetaCardUiInteractionEnum.Buy:
                    if (action != null)
                        OnBuyEvent += action;
                    break;
                case MetaCardUiInteractionEnum.None:
                default:
                    ResetEnum();
                    ClosePanel();
                    return;
            }

            _state &= ~MetaCardUiInteractionEnum.None;
            _state |= metaCardUiInteractionEnum;
        }
        public void ClosePanel()
            => _container.SetActive(false);
        public void OpenInteractionPanel()
        {
            if (_state != MetaCardUiInteractionEnum.None && !_container.activeSelf)
            {
                OnOpenInteractionScreen?.Invoke();
                bool toOpen = _state.HasFlag(MetaCardUiInteractionEnum.Info);
                _infoBtn?.SetActive(toOpen) ;
                 toOpen = _state.HasFlag(MetaCardUiInteractionEnum.Use);
                _selectBtn?.SetActive(toOpen);
                toOpen = _state.HasFlag(MetaCardUiInteractionEnum.Remove);
                _removeBtn?.SetActive(toOpen);
                toOpen = _state.HasFlag(MetaCardUiInteractionEnum.Dismental);
                _dismentalBtn?.SetActive(toOpen);
                toOpen = _state.HasFlag(MetaCardUiInteractionEnum.Buy);
                _buyBtn?.SetActive(toOpen);
                _container.SetActive(true);
            }
            else
            {
                ClosePanel();
            }
        }


        public void OnRemoveSelect()
        {
            OnRemoveEvent?.Invoke(_metacardUI.CardUI);
            ClosePanel();
        }
        public void OnUseSelect()
        {
            OnUseEvent?.Invoke(_metacardUI.CardUI);
            ClosePanel();
        }
        public void OnInfoSelect()
        {
            OnInfoEvent?.Invoke(_metacardUI.CardUI);
            ClosePanel();
        }
        public void OnDismentalSelect()
        {
            OnDismentalEvent?.Invoke(_metacardUI.CardUI);
            ClosePanel();
        }

        public void OnBuySelect()
        {
            OnBuyEvent?.Invoke(_metacardUI.CardUI);
     //       ClosePanel();
        }
    }
}
[Flags]
public enum MetaCardUiInteractionEnum
{
    None = 0,
    Info = 1 << 0,
    Remove = 1 << 1,
    Use = 1 << 2,
    Dismental = 1 << 3,
    Buy = 1 << 4,
    Upgrade = 1 << 5,
}
