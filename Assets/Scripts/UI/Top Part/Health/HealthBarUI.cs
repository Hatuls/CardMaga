using Battle.Turns;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardMaga.UI.Bars
{
    public class HealthBarUI : BaseBarUI
    {
        private const string FROM_MAXVALUE = "/";
#if UNITY_EDITOR
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
#endif
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
        private Coroutine _coroutine;
        private Sequence _healthBarSequence;
        private Sequence _innerBarSequence;

        private Tween _hpBarTween;
        private Tween _secondHPBarTween;

        private HPSlider _healthBarSliderHandler;
        private HPSlider _innerHealthBarSliderHandler;
        private int _changeSecondBar;

        private WaitForSeconds _delay;
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

            _healthBarSliderHandler = new HPSlider(_healthBarSlider, _healthImage);
            _innerHealthBarSliderHandler = new HPSlider(_healthBarInnerSlider, _healthInnerImage);

          //  GameTurn.OnTurnFinished += CompleteCounter;
           // _healthBarSequence = DOTween.Sequence();
           // _innerBarSequence = DOTween.Sequence();
            _delay = new WaitForSeconds(_healthBarSO.DelayTillReturn);
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
            _healthBarSequence = DoMoveSlider(_healthBarSequence, _healthBarSlider, _currentHealth, _healthBarSO.HealthStartLength, _healthBarSO.HealthStartCurve);
            _innerBarSequence = DoMoveSlider(_innerBarSequence, _healthBarInnerSlider, _currentHealth, _healthBarSO.HealthStartLength, _healthBarSO.HealthStartCurve);
        }
        private void SetText(int currentHealth, int maxHealth)
        {
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            _maxHealthText.text = string.Concat(FROM_MAXVALUE , maxHealth);
            _currentHealthText.text = Convert.ToString(currentHealth);
        }
        private void SetText(int currentHealth)
        {
            _currentHealthText.text = Convert.ToString(currentHealth);
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
                //StartHealthChange(_healthBarSlider, _healthBarSO.FillDownHealthSprite, _healthBarSO.HealthTransitionLength, _healthBarSO.ChangeHealthCurve);
                ChangeHealthSlider(_healthBarSO.FillDownHealthSprite, _currentHealth, _healthBarSO.HealthTransitionLength, _healthBarSO.ChangeHealthCurve);
            }
            else if (_currentHealth > _healthBarSlider.value)
            {
                ChangeHealthSlider(_healthBarSO.FillUpHealthSprite, _currentHealth, _healthBarSO.HealthTransitionLength, _healthBarSO.ChangeInnerHealthCurve);
                //      Debug.Log("Increacing Health");
                //    StartHealthChange(_healthBarInnerSlider, _healthBarSO.FillUpHealthSprite, _healthBarSO.HealthTransitionLength, _healthBarSO.ChangeInnerHealthCurve);
            }
        }

        private void ChangeHealthSlider(Sprite fillDownHealthSprite, int amount, float healthTransitionLength, AnimationCurve changeHealthCurve)
        {
            if (_hpBarTween != null && _hpBarTween.IsActive())
                _hpBarTween.Kill();

            _innerHealthBarSliderHandler.SetSprite(fillDownHealthSprite);
            _hpBarTween = _healthBarSliderHandler.SlideToValue(amount, healthTransitionLength, changeHealthCurve);

            StartCoroutine(ChangeInnerHealthBar());
            _changeSecondBar += 1;
        }

        private IEnumerator ChangeInnerHealthBar()
        {
            yield return _delay;

            _changeSecondBar -= 1;
            if (_changeSecondBar > 0)
                yield break;

            if (_secondHPBarTween != null && _secondHPBarTween.IsActive())
                _secondHPBarTween.Kill();

            _secondHPBarTween = _innerHealthBarSliderHandler.SlideToValue((int)_healthBarSlider.value, _healthBarSO.HealthTransitionLength, _healthBarSO.ChangeInnerHealthCurve);
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
            SetText(_currentHealth, _maxHealth);
        }
        //private void StartHealthChange(Slider slider, Sprite sprite, float transitionLength, AnimationCurve animCurve)
        //{
        //    _healthInnerImage.sprite = sprite;

        //    _hpBarTween =

        //    _healthBarSequence = DoMoveSlider(_healthBarSequence, slider, _currentHealth, transitionLength, animCurve);
        //    StopCounter();
        //    ResetCounter();
        //    _coroutine = StartCoroutine(StartTimer());
        //}



        //private void CompleteBarsTransition()
        //{
        //    if (_currentHealth > _healthBarSlider.value)
        //    {
        //        //Need to change health bar to fill bar
        //        //     Debug.Log("Health Was Increaced Completeing transition");
        //        _healthBarSequence = DoMoveSlider(_healthBarSequence, _healthBarSlider, _currentHealth, _healthBarSO.HealthTransitionLength, _healthBarSO.ChangeHealthCurve);
        //    }
        //    else if (_currentHealth == _healthBarSlider.value)
        //    {
        //        //Need to change fill to match the health
        //        //   Debug.Log("Health Was Decreaced Completeing transition");
        //        _innerBarSequence = DoMoveSlider(_innerBarSequence, _healthBarInnerSlider, _currentHealth, _healthBarSO.HealthTransitionLength, _healthBarSO.ChangeInnerHealthCurve);
        //    }
        //    else
        //    {
        //        Debug.LogWarning("Current health is lower than the slider, we have a problem");
        //    }
        //}
        //IEnumerator StartTimer()
        //{
        //    //      Debug.Log("Started Timer");
        //    while (_healthBarSO.DelayTillReturn > _counter)
        //    {
        //        // Debug.Log($"Health Bar counter: {_counter}");
        //        yield return null;
        //        _counter += Time.deltaTime;
        //    }
        //    CompleteBarsTransition();
        //}
        //private void ResetCounter()
        //{
        //    _counter = 0;
        //}
        //public void CompleteCounter()
        //{
        //    _counter = _healthBarSO.DelayTillReturn;
        //    //CompleteBarsTransition();
        //}
        //private void StopCounter()
        //{
        //    if (_coroutine != null)
        //        StopCoroutine(_coroutine);
        //}
        //private void OnDestroy()
        //{
        //    //StopCounter();
        //    //GameTurn.OnTurnFinished -= CompleteCounter;
        //}
    }


    public class HPSlider
    {
        private readonly Slider _slider;
        private readonly Image _image;
        public HPSlider(Slider slider, Image image)
        {
            _image = image;
            _slider = slider;
        }
        public void SetSprite(Sprite sprite) => _image.sprite = sprite;
        private float GetSliderValue() => _slider.value;
        private void SetSlideValue(float val) => _slider.value = val;
        public Tween SlideToValue(int amount, float duration, AnimationCurve animationCurve)
            => DOTween.To(GetSliderValue, SetSlideValue, amount, duration).SetEase(animationCurve);
    }
}
