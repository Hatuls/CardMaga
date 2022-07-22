using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;
using System.Collections;

namespace CardMaga.UI.Bars
{
    public class HealthBarUI : BaseBarUI
    {
        //So with all the dotween curves
        //So with the colors
        //Init Health Bar
        //Change health bar
        //Change inner bar
        [SerializeField] HealthBarSO _healthBarSO;
        [SerializeField] Slider _healthBarSlider;
        [SerializeField] Slider _healthBarInnerSlider;
        [SerializeField] Image _healthImage;
        [SerializeField] Image _healthInnerImage;
        [SerializeField] TextMeshProUGUI _currentHealthText;
        [SerializeField] TextMeshProUGUI _maxHealthText;
        float _counter;
        int _currentHealth;
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
        }
        private void Start()
        {
            SetBarColor(_healthImage, _healthBarSO.HealthColor);
            SetBarColor(_healthInnerImage, _healthBarSO.FillDownColor);
        }
        public void InitHealthBar(int currentHealth, int maxHealth)
        {
            InitText(currentHealth, maxHealth);
            //Set max health
            SetMaxValue(_healthBarSlider,maxHealth);
            SetMaxValue(_healthBarInnerSlider,maxHealth);
            //Reset Sliders to 0
            ResetSliderFill(_healthBarSlider);
            ResetSliderFill(_healthBarInnerSlider);
            //move slider at start of game from 0 to current health;
            DoMoveSlider(_healthBarSlider,currentHealth,_healthBarSO.HealthStartLength,_healthBarSO.HealthStartCurve);
            DoMoveSlider(_healthBarInnerSlider,currentHealth,_healthBarSO.HealthStartLength,_healthBarSO.HealthStartCurve);
        }
        private void InitText(int currentHealth, int maxHealth)
        {
            _maxHealthText.text = maxHealth.ToString();
            _currentHealthText.text = currentHealth.ToString();
        }
        public void ChangeHealth(int currentHealth)
        {
            _currentHealth = currentHealth;
            if (_currentHealth < _healthBarSlider.value)
            {
                Debug.Log("Lowering Health");
                _healthInnerImage.color = _healthBarSO.FillDownColor;
                DoMoveSlider(_healthBarSlider, _currentHealth, _healthBarSO.HealthTransitionLength,_healthBarSO.ChangeHealthCurve);

            }
            else if (_currentHealth > _healthBarSlider.value)
            {
                Debug.Log("Increacing Health");
                _healthInnerImage.color = _healthBarSO.FillUpColor;
                DoMoveSlider(_healthBarInnerSlider, _currentHealth, _healthBarSO.HealthTransitionLength,_healthBarSO.ChangeHealthCurve);
            }
            else
            {
                Debug.Log("Health stayes the same, No need for change");
            }
            //Check amount of health, if it went down from the last time we checked we lowering the health slider to the 
            //Recive change
            //if we lower health
            //set color to lowered down health
            //change the health bar with transition to the current health
            //start delay for the fill
            //when the timer stops make a transition of the fill to the current health
            //if the health is increacing
            //change the fill above the health
            //change the color of the fill to increaced up health
            //create a delay, when the timer stopes increace the health to the fill amount
        }
        private void CompleteBarsTransition()
        {
            if (_currentHealth < _healthBarSlider.value)
            {
                //health was Increaced
            }
            else if (_currentHealth == _healthBarSlider.value)
            {
                //health was decreaced
            }
        }
        IEnumerator StartTimer()
        {
            while (_healthBarSO.DelayTillReturn < _counter)
            {
                _counter += Time.deltaTime;
                yield return null;
            }
            CompleteBarsTransition();
        }
        private void ResetCounter()
        {
            _counter = 0;
        }
        private void StopCounter() => StopCoroutine(StartTimer());
    }
}
