using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;
using Sirenix.OdinInspector;
using Battle.Turns;

namespace CardMaga.UI.Bars
{
    public class HealthBarUI : BaseBarUI
    {
        [Header("Testing")]
        public int MaxHealthTest;
        public int CurrentHealthTest;
        [Button]
        public void TestInit()
        {
            InitHealthBar(CurrentHealthTest, MaxHealthTest);
        }
        [Button]
        public void TestTransition()
        {
            ChangeHealth(CurrentHealthTest);
        }
        [Button]
        public void TestMaxHealthChange()
        {
            ChangeMaxHealth(MaxHealthTest);
        }

        [Header("Fields")]
        [SerializeField] HealthBarSO _healthBarSO;
        [SerializeField] Slider _healthBarSlider;
        [SerializeField] Slider _healthBarInnerSlider;
        [SerializeField] Image _healthImage;
        [SerializeField] Image _healthInnerImage;
        [SerializeField] TextMeshProUGUI _currentHealthText;
        [SerializeField] TextMeshProUGUI _maxHealthText;
        public float _counter = 0;
        int _currentHealth;
        int _maxHealth;
        private void Awake()
        {
            if (_healthBarSlider == null)
                throw new Exception("HealthBarUI has no health bar slider");
            if (_healthBarInnerSlider == null)
                throw new Exception("HealthBarUI has no health bar inner slider");
            if (_healthImage == null)
                throw new Exception("HealthBarUI has no health bar image");
            if (_healthInnerImage == null)
                throw new Exception("HealthBarUI has no health bar inner image");
            if (_currentHealthText == null)
                throw new Exception("HealthBarUI has no current health Text");
            if (_maxHealthText == null)
                throw new Exception("HealthBarUI has no Max health Text");

            GameTurn.OnTurnFinished += CompleteCounter;
        }
        private void Start()
        {
            _healthImage.sprite = _healthBarSO.BaseHealthSprite;
        }
        public void InitHealthBar(int currentHealth, int maxHealth)
        {
            _maxHealth = maxHealth;
            _currentHealth = currentHealth;
            SetText(_currentHealth, _maxHealth);
            //Set max health
            SetMaxValue(_healthBarSlider, _maxHealth);
            SetMaxValue(_healthBarInnerSlider, _maxHealth);
            //Reset Sliders to 0
            ResetSliderFill(_healthBarSlider);
            ResetSliderFill(_healthBarInnerSlider);
            //move slider at start of game from 0 to current health;
            DoMoveSlider(_healthBarSlider, _currentHealth, _healthBarSO.HealthStartLength,_healthBarSO.HealthStartCurve);
            DoMoveSlider(_healthBarInnerSlider, _currentHealth, _healthBarSO.HealthStartLength,_healthBarSO.HealthStartCurve);
        }
        private void SetText(int currentHealth, int maxHealth)
        {
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            _maxHealthText.text = string.Concat("/" + maxHealth);
            _currentHealthText.text = string.Concat(currentHealth);
        }
        private void SetText(int currentHealth)
        {
            _currentHealthText.text = string.Concat(currentHealth);
        }
        public void ChangeHealth(int currentHealth)
        {
            _currentHealth = currentHealth;
            SetText(_currentHealth);
            if (_currentHealth == _healthBarSlider.value)
            {
         //       Debug.LogWarning("Health is the same but you called for Change Health method");
                return;
            }
            if (_currentHealth < _healthBarSlider.value)
            {
            //    Debug.Log("Lowering Health");
                StartHealthChange(_healthBarSlider, _healthBarSO.FillDownHealthSprite, _healthBarSO.HealthTransitionLength, _healthBarSO.ChangeHealthCurve);
            }
            else if (_currentHealth > _healthBarSlider.value)
            {
          //      Debug.Log("Increacing Health");
                StartHealthChange(_healthBarInnerSlider, _healthBarSO.FillUpHealthSprite, _healthBarSO.HealthTransitionLength, _healthBarSO.ChangeInnerHealthCurve);
            }
        }
        public void ChangeMaxHealth(int maxHealth)
        {
            if (_maxHealth == maxHealth)
            {
                Debug.LogWarning("Max Health is the same but you called for Max Change Health method");
                return;
            }
            if (_maxHealth < maxHealth)
            {
          //      var healthDelta = maxHealth - _maxHealth;
                //ChangeHealth(_currentHealth += healthDelta);
        //        Debug.Log("max health was Increaced");
            }
            else
            {
        //        Debug.Log("max health was reduced");
            }
            _maxHealth = maxHealth;
            SetMaxValue(_healthBarInnerSlider, _maxHealth);
            SetMaxValue(_healthBarSlider, _maxHealth);
            SetText(_currentHealth,_maxHealth);
        }
        private void StartHealthChange(Slider slider,Sprite sprite,float transitionLength,AnimationCurve animCurve)
        {
            _healthInnerImage.sprite = sprite;
            DoMoveSlider(slider, _currentHealth, transitionLength, animCurve);
            ResetCounter();
            StartCoroutine(StartTimer());
        }
        private void CompleteBarsTransition()
        {
            if (_currentHealth > _healthBarSlider.value)
            {
                //Need to change health bar to fill bar
           //     Debug.Log("Health Was Increaced Completeing transition");
                DoMoveSlider(_healthBarSlider, _currentHealth, _healthBarSO.HealthTransitionLength, _healthBarSO.ChangeHealthCurve);
            }
            else if (_currentHealth == _healthBarSlider.value)
            {
                //Need to change fill to match the health
             //   Debug.Log("Health Was Decreaced Completeing transition");
                DoMoveSlider(_healthBarInnerSlider, _currentHealth, _healthBarSO.HealthTransitionLength, _healthBarSO.ChangeInnerHealthCurve);
            }
            else
            {
                Debug.LogWarning("Current health is lower than the slider, we have a problem");
            }
        }
        IEnumerator StartTimer()
        {
      //      Debug.Log("Started Timer");
            while (_healthBarSO.DelayTillReturn > _counter)
            {
               // Debug.Log($"Health Bar counter: {_counter}");
                _counter += Time.deltaTime;
                yield return null;
            }
            CompleteBarsTransition();
        }
        private void ResetCounter()
        {
            _counter = 0;
        }
        public void CompleteCounter()
        {
            _counter = _healthBarSO.DelayTillReturn;
        }
        private void StopCounter() => StopCoroutine(StartTimer());
        private void OnDestroy()
        {
            StopCounter();
            GameTurn.OnTurnFinished -= CompleteCounter;
        }
    }
}
