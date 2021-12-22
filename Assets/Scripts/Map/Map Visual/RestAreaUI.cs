using Battles.UI;
using DesignPattern;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Map.UI
{


    public class RestAreaUI : MonoBehaviour ,IObserver
    {
        [SerializeField] ObserverSO _observer;
        public static RestAreaUI Instance;
        [SerializeField] GameObject _RestAreaContainer;
        [SerializeField] GameObject _backgroundPanel;
        [SerializeField]
        RestArea _restArea;

        [SerializeField]
        RemoveCardPanelScreen _removeCardPanelScreen;
        [SerializeField] Image _topOptionLeftImage;
        [SerializeField] Image _topOptionRightImage;

        [SerializeField] TextMeshProUGUI _topOptionLeftText;
        [SerializeField] TextMeshProUGUI _topOptionRightText;



        [SerializeField] Image _bottomOptionLeftImage;
        [SerializeField] Image _bottomOptionRightImage;

        [SerializeField] TextMeshProUGUI _bottomOptionLeftText;
        [SerializeField] TextMeshProUGUI _bottomOptionRightText;

        const string HealOptionText = "Heal ";
        const string StaminaShardText = "Add Stamina Shard";
        const string MaxHpAdditonText = " Max HP";
        const string RemoveCardText ="Remove Card";

        [SerializeField] Color _optionTakenColor;
        [SerializeField] Color _optionNotTakeColor;
        [SerializeField] Color _resetColor;

        bool _firstOptionFlag;
        bool _secondOptionFlag;


        private void Start()
        {
            Instance = this;
            if (_restArea == null)
                throw new System.Exception("Rest Area Data is null!");


            ResetRestArea();

        }

        private void ResetRestArea()
        {
            ResetTopQuestions();
            ResetBottomQuestions();
        }

        private void ResetBottomQuestions()
        {
            if (_bottomOptionLeftText.gameObject.activeSelf == false)
                _bottomOptionLeftText.gameObject.SetActive(true);
            if (_bottomOptionRightText.gameObject.activeSelf == false)
                _bottomOptionRightText.gameObject.SetActive(true);


            _secondOptionFlag = true;

            _bottomOptionLeftText.text = string.Concat(RemoveCardText);
            _bottomOptionRightText.text = string.Concat(StaminaShardText);

            _bottomOptionLeftImage.color = _resetColor;
            _bottomOptionRightImage.color = _resetColor;
        }

        private void ResetTopQuestions()
        {

            if (_topOptionLeftText.gameObject.activeSelf == false)
                _topOptionLeftText.gameObject.SetActive(true);
            if (_topOptionRightText.gameObject.activeSelf == false)
                _topOptionRightText.gameObject.SetActive(true);

            _topOptionLeftText.text = string.Concat(HealOptionText, _restArea.HPHeal);
            _topOptionRightText.text = string.Concat(_restArea.MaxHPAddition, MaxHpAdditonText);
            _topOptionLeftImage.color = _resetColor;
            _topOptionRightImage.color = _resetColor;
            _firstOptionFlag = true;

        }

        private void SetActiveRestAreaContainer(bool state) => _RestAreaContainer.SetActive(state);

        public void EnterRestArea()
        {
            _observer.Notify(this);
            ResetRestArea();
            SetActiveRestAreaContainer(true);
            _backgroundPanel.SetActive(true);
        }

        public void ExitRestArea()
        {
            MapView.Instance.SetAttainableNodes();
            SetActiveRestAreaContainer(false);
            _backgroundPanel.SetActive(false);
            _observer.Notify(null);
        }

        public void AddMaxHealth()
        {

            if (_firstOptionFlag == false)
                return;
            _firstOptionFlag = false;
            _restArea.AddMaxHealth();
            _topOptionRightImage.color = _optionTakenColor;

            _topOptionLeftImage.color = _optionNotTakeColor;
            _topOptionLeftText.gameObject.SetActive(false);
            CheckForBothDecisionsAssigned();
        }
        public void Heal()
        {
            if (_firstOptionFlag == false)
                return;
            _firstOptionFlag = false;
            _restArea.AddHealth();

            _topOptionLeftImage.color = _optionTakenColor;

            _topOptionRightImage.color = _optionNotTakeColor;
            _topOptionRightText.gameObject.SetActive(false);
            CheckForBothDecisionsAssigned();
        }
        public void StamiaShard()
        {
            if (_secondOptionFlag == false)
                return;
            _secondOptionFlag = false;

            _restArea.AddStaminaShard();


            _bottomOptionRightImage.color = _optionTakenColor;

            _bottomOptionLeftImage.color = _optionNotTakeColor;
            _bottomOptionLeftText.gameObject.SetActive(false);

            CheckForBothDecisionsAssigned();
        }
        public void RemoveCard()
        {
            if (_secondOptionFlag == false)
                return;

            _removeCardPanelScreen.OpenRemoveCardScreen();
            SetActiveRestAreaContainer(false);


        }
        public void CancelRemoveCardUI()
        => SetActiveRestAreaContainer(true);
        public void RemoveCardUI(CardUI card)
        {
            _bottomOptionLeftImage.color = _optionTakenColor;
            _bottomOptionRightImage.color = _optionNotTakeColor;
            _bottomOptionRightText.gameObject.SetActive(false);
            _secondOptionFlag = false;
            _restArea.RemoveCard(card.GFX.GetCardReference.CardInstanceID);
            SetActiveRestAreaContainer(true);
            CheckForBothDecisionsAssigned();
        }



        private void CheckForBothDecisionsAssigned()
        {
            if (!_secondOptionFlag && !_firstOptionFlag)
                ExitRestArea();
        }

        public void OnNotify(IObserver Myself)
        {
   
        }
    }
    [Serializable]
    public class RestArea
    {
        [SerializeField] byte _staminaShardAmount = 1;
        [SerializeField] ushort HPPrecentage;
        [SerializeField] ushort _MaxHPAddition;

        public byte StaminaShard => _staminaShardAmount;
        public ushort HPHeal => (ushort)(Account.AccountManager.Instance.BattleData.Player.CharacterData.CharacterStats.MaxHealth * HPPrecentage / 100);
        public ushort MaxHPAddition => _MaxHPAddition;
        public void AddHealth()
        {
            var stats = Account.AccountManager.Instance.BattleData.Player.CharacterData.CharacterStats;
            int currentHP = stats.Health;
            int maxHealth = stats.MaxHealth;
            int amount = HPHeal;
            if (currentHP == maxHealth)
                return;
            else if (currentHP + amount >= maxHealth)
                currentHP = maxHealth;
            else
                currentHP += amount;

            Account.AccountManager.Instance.BattleData.Player.CharacterData.CharacterStats.Health = currentHP;
            UpperInfoUIHandler.Instance.UpdateUpperInfoHandler(ref Account.AccountManager.Instance.BattleData.Player.CharacterData.CharacterStats);
        }

        public void AddMaxHealth()
        {
            var stats = Account.AccountManager.Instance.BattleData.Player.CharacterData.CharacterStats;
            int maxHealth = stats.MaxHealth;
            int currentHP = stats.Health;

            maxHealth += _MaxHPAddition;
            currentHP += _MaxHPAddition;

            var player = Account.AccountManager.Instance.BattleData.Player;
           player.CharacterData.CharacterStats.MaxHealth = maxHealth;
            player.CharacterData.CharacterStats.Health = currentHP;
            UpperInfoUIHandler.Instance.UpdateUpperInfoHandler(ref player.CharacterData.CharacterStats);
        }

        internal void AddStaminaShard()
        {
            Account.AccountManager.Instance.BattleData.Player.CharacterData.CharacterStats.StaminaShard += _staminaShardAmount;
            Debug.Log("Added Stamina Shard From Rest Area\nCurrent Amount: " + Account.AccountManager.Instance.BattleData.Player.CharacterData.CharacterStats.StaminaShard);
        }

        public void RemoveCard(ushort instanceID)
        {
            Account.AccountManager.Instance.BattleData.Player.RemoveCardFromDeck(instanceID);
        }
    }
}