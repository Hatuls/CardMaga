using System;
using Unity.Events;
using UnityEngine;
using Battles.UI.CardUIAttributes;

namespace Battles.UI
{

    public class CardUI : MonoBehaviour
    {
        #region Fields
        [SerializeField]
   //     [HideInInspector]
        private CardGFX _cardGFX;

        [HideInInspector]
        [SerializeField]
        private CanvasGroup _canvasGroup;

        private CardInputs _inputs;

        private CardTranslations _cardTranslations;

        #region Enum Selection
        [Flags]
        private enum CardUISettings
        {
            [HideInInspector]
            None = 0,

            Visable = 1 << 0,

            Touchable = 1 << 1,

            Moveable = 1 << 2

                /*
                 *  0000 0
                 *  0001 2^0
                 *  0010 2^1
                 *  0100 2^2
                 *  1000 2^3
                 */
               
        };
        [SerializeField]
        [Sirenix.OdinInspector.OnValueChanged("CardUISettingsEnum")]
        [Sirenix.OdinInspector.EnumToggleButtons]

        private CardUISettings Card;

        private void CardUISettingsEnum()
        {
            //Debug.Log(Card);
            //Debug.Log((byte)Card);
            //Debug.Log(Card.HasFlag(CardUISettings.Default));
            //Debug.Log(Card.HasFlag(CardUISettings.Image));

            if ((Card & CardUISettings.Visable) != CardUISettings.Visable)
                Card |= CardUISettings.Visable;


        }
        #endregion

        #endregion


        #region Events
        [Space]
        [Header("Events")]
        [SerializeField] CardUIEvent _selectCardEvent;
        [SerializeField] CardUIEvent _removeCardEvent;
        [SerializeField] CardUIEvent _onClickedCardEvent;
        #endregion



        #region Properties
        public  CardGFX GFX =>  _cardGFX;

        public  CardInputs Inputs
        {
            get
            {
                if (_inputs == null && (Card & CardUISettings.Touchable) == CardUISettings.Touchable)
                    _inputs = new CardInputs( _canvasGroup,  _selectCardEvent,  _removeCardEvent,  _onClickedCardEvent, this);
                return  _inputs;
            }
        }

        public  CardTranslations CardTranslations {
            get
            {
                if (_cardTranslations == null &&
                    ((Card & CardUISettings.Moveable) == CardUISettings.Moveable))
                    _cardTranslations = new CardTranslations( _cardGFX.GetRectTransform);

                return  _cardTranslations;
            }
        }
        #endregion
    }
}
