using System;
using UnityEngine;

namespace CardMaga.UI.PopUp
{
    public class PlayButtonPopUpTerminal : BasePopUpTerminal
    {
        [SerializeField] private string _title;
        [SerializeField] private string _message;
        
        [SerializeField] private TempPlayButton _playButton;
        
        private void OnEnable()
        {
            _playButton.OnFailedToStartBattle += ShowPopUp;
        }

        private void OnDisable()
        {
            _playButton.OnFailedToStartBattle -= ShowPopUp;
        }

        protected override Vector2 GetStartLocation()
        {
            return PopUpManager.Instance.GetPosition(_startLocation);
        }

        protected override void ShowPopUp()
        {
            base.ShowPopUp();
            var messagePopUpHandler = _currentActivePopUp.transform.GetComponent<MessagePopUpHandler>();
            messagePopUpHandler.Init(_title,_message,HidePopUp);
        }
    }
}