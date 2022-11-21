using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public void ReduceTransition()
        {
            ChangeHealth(_currentHealth - 5);
        }
        [Button]
        public void AddTransition()
        {
            ChangeHealth(_currentHealth + 5);
        }
        [Button]
        public void TestMaxHealthChange()
        {
            ChangeMaxHealth(MaxHealthTest);
        }
#endif
        [Header("Fields")]
        [SerializeField]
        HealthBarSO _healthBarSO;

        [SerializeField]
        Slider _healthBarSlider;

        [SerializeField]
        Slider _healthBarInnerSlider;

        [SerializeField]
        Image _healthImage;

        [SerializeField]
        Image _healthInnerImage;

        [SerializeField]
        TextMeshProUGUI _currentHealthText;

        [SerializeField]
        TextMeshProUGUI _maxHealthText;

        [ShowInInspector]

        public List<int> Queue => _healthQueue?.ToList();

        private Queue<int> _healthQueue;
        int _currentHealth;
        int _maxHealth;


        private HPSlider _healthBarSliderHandler;
        private HPSlider _innerHealthBarSliderHandler;

        private bool _isActive;
        private bool _isChangingTransition;
        private float _counter;
        private int _previousPositiveValue;
        private Sequence _currentSequence;
        private Coroutine _coroutine;
        public bool IsQueueEmpty => _healthQueue.Count == 0;

        public bool IsActive { get => _isActive;private set => _isActive = value; }

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
   
            _healthImage.sprite = _healthBarSO.BaseHealthSprite;
            _healthQueue = new Queue<int>(5);
        }

        public void InitHealthBar(int currentHealth, int maxHealth)
        {
            _maxHealth = maxHealth;
            _currentHealth = currentHealth;
            _previousPositiveValue =0;
            SetText(_currentHealth, _maxHealth);
            //Set max health
            SetMaxValue(_healthBarSlider, _maxHealth);
            SetMaxValue(_healthBarInnerSlider, _maxHealth);
            //Reset Sliders to 0
            ResetSliderFill(_healthBarSlider);
            ResetSliderFill(_healthBarInnerSlider);
            //move slider at start of game from 0 to current health;
            DoMoveSlider(_healthBarSlider, _currentHealth, _healthBarSO.HealthStartLength, _healthBarSO.HealthStartCurve);
            DoMoveSlider(_healthBarInnerSlider, _currentHealth, _healthBarSO.HealthStartLength, _healthBarSO.HealthStartCurve);

        }
        private void SetText(int currentHealth, int maxHealth)
        {
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            _maxHealthText.text = string.Concat(FROM_MAXVALUE, maxHealth);
            _currentHealthText.text = Convert.ToString(currentHealth);
        }
        private void SetText(int currentHealth)
        {
            _currentHealthText.text = Convert.ToString(currentHealth);
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




        public void ChangeHealth(int currentHealth)
        {

            SetText(currentHealth);
            _healthQueue.Enqueue(currentHealth);
            if (_healthQueue.Count == 1)
                MoveNext();
        }

        private void MoveNext()
        {
            if (IsQueueEmpty)
                return;

            int nextValue = _healthQueue.Peek();


            // Checking if it is negative or positive
            int currentFlow = (nextValue - _currentHealth);

            //If they are the same then we are looking for the next different value
            if (currentFlow == 0)
            {
                Dequeue();
                MoveNext();
                return;
            }


            if (!IsActive)
            {
                _previousPositiveValue = currentFlow;
                ActivatingTransition();
            }
            else
                CheckSameTransitionType(currentFlow);

        }


        private void CheckSameTransitionType(int isPositive)
        {
            bool isBothNegative = isPositive < 0 && _previousPositiveValue < 0; // both going down
            bool isBothPositive = isPositive > 0 && _previousPositiveValue > 0; // both going up



            if ((isBothNegative || isBothPositive) && !_isChangingTransition) // the same type of transition
            {

                ActivatingTransition();

            }
            else
            {
                _isChangingTransition = true;
                SkipTimer();
            }


        }
        private void Dequeue() => _currentHealth = _healthQueue.Dequeue();
        private void ActivatingTransition()
        {
            if (IsQueueEmpty)
                return;

            IsActive = true;
            //Checking the isPositiveValue
         
            Dequeue();
            StartTransition();

        }

        private void StartTransition()
        {
            StopCoroutine();

            FirstSliderTransition();

            // Second Slider

            _coroutine = StartCoroutine(Delay(SecondSliderTransition));
        }

        private void FirstSliderTransition()
        {
            bool isPositive = _previousPositiveValue > 0;


            //Set Sprite
            SetInnerSprite(isPositive);
            //First Slider
            HPSlider firstSlider = isPositive ? _innerHealthBarSliderHandler : _healthBarSliderHandler;
            AnimationCurve firstCurve = isPositive ? _healthBarSO.ChangeInnerHealthCurve : _healthBarSO.ChangeHealthCurve;
            KillTweens();
            _currentSequence = DOTween.Sequence(this);
            _currentSequence.Append(SliderTransition(firstSlider, _healthBarSO.HealthTransitionLength, _currentHealth, firstCurve, MoveNext));
        }

        private void SecondSliderTransition()
        {
            HPSlider secondSlider;
            AnimationCurve secondCurve;
            bool isPositive = _previousPositiveValue > 0;
            secondSlider = !isPositive ? _innerHealthBarSliderHandler : _healthBarSliderHandler;
            secondCurve = !isPositive ? _healthBarSO.ChangeInnerHealthCurve : _healthBarSO.ChangeHealthCurve;
            KillTweens();
            _currentSequence = DOTween.Sequence(this);
            _currentSequence.Append(SliderTransition(secondSlider, _healthBarSO.HealthTransitionLength,_currentHealth, secondCurve, CompleteTransitions));
        }


        private void KillTweens()
        {
            if (_currentSequence != null)
            {
                _currentSequence.Kill(this);
            }
        }

        private void SetInnerSprite(bool isPositive)
            => _innerHealthBarSliderHandler.SetSprite(isPositive ? _healthBarSO.FillUpHealthSprite : _healthBarSO.FillDownHealthSprite);


  
        private IEnumerator Delay(Action action)
        {
            _counter = 0;
            float delayTillReturn = _healthBarSO.DelayTillReturn;
            while (_counter <= delayTillReturn)
            {
                yield return null;
                _counter += Time.deltaTime;
            }

            action?.Invoke();
        }



        private Tween SliderTransition(HPSlider slider, float duration, int finalAmount, AnimationCurve animationCurve,Action onComplete )
        => slider.SlideToValue(finalAmount, duration, animationCurve).OnComplete(onComplete.Invoke);

        private void CompleteTransitions()
        {
            IsActive = false;
            _isChangingTransition = false;
            KillTweens();
            StopCoroutine();
            MoveNext();
        }
        private void StopCoroutine()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
        }
        private void ResetTimer() => _counter = 0;
        private void SkipTimer() => _counter = _healthBarSO.DelayTillReturn;

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
        public float GetSliderValue() => _slider.value;
        private void SetSlideValue(float val) => _slider.value = val;
        public Tween SlideToValue(int amount, float duration, AnimationCurve animationCurve)
            => DOTween.To(GetSliderValue, SetSlideValue, amount, duration).SetEase(animationCurve);
    }

    //public class AddHPTransition : IHPTransition
    //{
    //    private readonly MonoBehaviour _sceneObject;
    //    private readonly HPSlider _slider;
    //    private readonly HPSlider _innerSlider;
    //    private readonly HealthBarSO _healthBarSO;
    //    // private readonly regular transition off slider
    //    // private readonly speed up transition off slider
    //    private Tween _hpBarTween;
    //    private Tween _secondHPBarTween;
    //    private bool _isActive;
    //    private Coroutine _coroutine;
    //    public bool IsActive => _isActive;
    //    public AddHPTransition(MonoBehaviour sceneObj, HPSlider hp, HPSlider inner, HealthBarSO healthBarSO)
    //    {
    //        _sceneObject = sceneObj;
    //        _slider = hp;
    //        _innerSlider = inner;
    //        _healthBarSO = healthBarSO;
    //    }


    //    public void StartTransition(int amount)
    //    {
    //        _isActive = true;
    //        if (_secondHPBarTween != null && _secondHPBarTween.IsActive())
    //            _secondHPBarTween.Kill();

    //        _innerSlider.SetSprite(_healthBarSO.FillUpHealthSprite);
    //        _secondHPBarTween = _innerSlider.SlideToValue(amount, _healthBarSO.DelayTillReturn, _healthBarSO.ChangeInnerHealthCurve);
    //        StopCoroutine();
    //        _coroutine = _sceneObject.StartCoroutine(ChangeHealthBar());

    //    }

    //    public void StopCoroutine()
    //    {
    //        if (_coroutine != null)
    //            _sceneObject.StopCoroutine(_coroutine);
    //    }

    //    private IEnumerator ChangeHealthBar()
    //    {
    //        float counter = 0;
    //        while (counter <= _healthBarSO.DelayTillReturn)
    //        {
    //            yield return null;
    //            counter += Time.deltaTime;
    //        }

    //        InnerTransition(_healthBarSO.HealthTransitionLength);
    //    }

    //    public void InnerTransition(float duration)
    //    {
    //        if (_hpBarTween != null && _hpBarTween.IsActive())
    //            _hpBarTween.Kill();

    //        _hpBarTween = _slider.SlideToValue((int)_innerSlider.GetSliderValue(), duration, _healthBarSO.ChangeInnerHealthCurve).OnComplete(Completed);
    //    }

    //    private void Completed() => _isActive = false;
    //}

    //public interface IHPTransition
    //{
    //    bool IsActive { get; }

    //    void InnerTransition(float duration);
    //    void StartTransition(int amount);
    //    void StopCoroutine();
    //}

    //public class ReduceHPTransition : IHPTransition
    //{
    //    private readonly MonoBehaviour _sceneObject;
    //    private readonly HPSlider _slider;
    //    private readonly HPSlider _innerSlider;
    //    private readonly HealthBarSO _healthBarSO;
    //    // private readonly regular transition off slider
    //    // private readonly speed up transition off slider
    //    private Tween _hpBarTween;
    //    private Tween _secondHPBarTween;
    //    private bool _isActive;
    //    private Coroutine _coroutine;
    //    public bool IsActive => _isActive;
    //    public ReduceHPTransition(MonoBehaviour sceneObj, HPSlider hp, HPSlider inner, HealthBarSO healthBarSO)
    //    {
    //        _sceneObject = sceneObj;
    //        _slider = hp;
    //        _innerSlider = inner;
    //        _healthBarSO = healthBarSO;
    //    }


    //    public void StartTransition(int amount)
    //    {
    //        _isActive = true;
    //        if (_hpBarTween != null && _hpBarTween.IsActive())
    //            _hpBarTween.Kill();

    //        _innerSlider.SetSprite(_healthBarSO.FillDownHealthSprite);
    //        _hpBarTween = _slider.SlideToValue(amount, _healthBarSO.DelayTillReturn, _healthBarSO.ChangeHealthCurve);

    //        StopCoroutine();
    //        _coroutine = _sceneObject.StartCoroutine(ChangeInnerHealthBar());

    //    }

    //    public void StopCoroutine()
    //    {
    //        if (_coroutine != null)
    //            _sceneObject.StopCoroutine(_coroutine);
    //    }

    //    private IEnumerator ChangeInnerHealthBar()
    //    {
    //        float counter = 0;
    //        while (counter <= _healthBarSO.DelayTillReturn)
    //        {
    //            yield return null;
    //            counter += Time.deltaTime;
    //        }

    //        InnerTransition(_healthBarSO.HealthTransitionLength);
    //    }

    //    public void InnerTransition(float duration)
    //    {
    //        if (_secondHPBarTween != null && _secondHPBarTween.IsActive())
    //            _secondHPBarTween.Kill();

    //        _secondHPBarTween = _innerSlider.SlideToValue((int)_slider.GetSliderValue(), duration, _healthBarSO.ChangeInnerHealthCurve).OnComplete(Completed);
    //    }

    //    private void Completed() => _isActive = false;
    //}

    //public class AddTransitionCommand : ISequenceCommand , IPoolable<AddTransitionCommand>
    //{
    //    public event Action OnFinishExecute;
    //    public event Action<AddTransitionCommand> OnDisposed;

    //    private IHPTransition _hPTransition;
    //    private CommandType _commandType;
    //    private int _amount;
    //    public CommandType CommandType => _commandType;
    //    public bool IsReducing;


    //    public void Init(CommandType commandType, bool isReducing , int amount, IHPTransition hPTransition )
    //    {
    //        _commandType = commandType;
    //        IsReducing = isReducing;
    //        _hPTransition = hPTransition;
    //        _amount = amount;
    //    }

    //    public void Execute()
    //    {

    //    }

    //    public void Undo()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void Dispose()
    //    {
    //        OnDisposed?.Invoke(this);
    //    }
    //}
}
