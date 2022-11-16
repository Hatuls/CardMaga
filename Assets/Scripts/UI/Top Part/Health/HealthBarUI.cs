using CardMaga.Commands;
using CardMaga.Tools.Pools;
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
        [SerializeField] HealthBarSO _healthBarSO;
        [SerializeField] Slider _healthBarSlider;
        [SerializeField] Slider _healthBarInnerSlider;
        [SerializeField] Image _healthImage;
        [SerializeField] Image _healthInnerImage;
        [SerializeField] TextMeshProUGUI _currentHealthText;
        [SerializeField] TextMeshProUGUI _maxHealthText;

        int _currentHealth;
        int _maxHealth;

 
        private HPSlider _healthBarSliderHandler;
        private HPSlider _innerHealthBarSliderHandler;

        private AddHPTransition _addHPTransition;
        private ReduceHPTransition _reduceHPTransition;
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

            _addHPTransition = new AddHPTransition(this,_healthBarSliderHandler, _innerHealthBarSliderHandler, _healthBarSO);
            _reduceHPTransition = new ReduceHPTransition(this, _healthBarSliderHandler, _innerHealthBarSliderHandler, _healthBarSO);
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
             DoMoveSlider(_healthBarSlider, _currentHealth, _healthBarSO.HealthStartLength, _healthBarSO.HealthStartCurve);
            DoMoveSlider( _healthBarInnerSlider, _currentHealth, _healthBarSO.HealthStartLength, _healthBarSO.HealthStartCurve);
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
        public void ChangeHealth(int currentHealth)
        {
            _currentHealth = currentHealth;
            SetText(_currentHealth);

            if (_currentHealth < _healthBarSlider.value) // reduce
            {
                _reduceHPTransition.StartTransition(_currentHealth);
            }
            else if (_currentHealth > _healthBarSlider.value) // add
            {
                _addHPTransition.StartTransition(_currentHealth);
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
            SetText(_currentHealth, _maxHealth);
        }
      
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

    public class AddHPTransition : IHPTransition
    {
        private readonly MonoBehaviour _sceneObject;
        private readonly HPSlider _slider;
        private readonly HPSlider _innerSlider;
        private readonly HealthBarSO _healthBarSO;
        // private readonly regular transition off slider
        // private readonly speed up transition off slider
        private Tween _hpBarTween;
        private Tween _secondHPBarTween;
        private bool _isActive;
        private Coroutine _coroutine;
        public bool IsActive => _isActive;
        public AddHPTransition(MonoBehaviour sceneObj, HPSlider hp, HPSlider inner, HealthBarSO healthBarSO)
        {
            _sceneObject = sceneObj;
            _slider = hp;
            _innerSlider = inner;
            _healthBarSO = healthBarSO;
        }


        public void StartTransition(int amount)
        {
            _isActive = true;
            if (_secondHPBarTween != null && _secondHPBarTween.IsActive())
                _secondHPBarTween.Kill();

            _innerSlider.SetSprite(_healthBarSO.FillUpHealthSprite);
            _secondHPBarTween = _innerSlider.SlideToValue(amount, _healthBarSO.DelayTillReturn, _healthBarSO.ChangeInnerHealthCurve);
            StopCoroutine();
            _coroutine = _sceneObject.StartCoroutine(ChangeHealthBar());

        }

        public void StopCoroutine()
        {
            if (_coroutine != null)
                _sceneObject.StopCoroutine(_coroutine);
        }

        private IEnumerator ChangeHealthBar()
        {
            float counter = 0;
            while (counter <= _healthBarSO.DelayTillReturn)
            {
                yield return null;
                counter += Time.deltaTime;
            }

            InnerTransition(_healthBarSO.HealthTransitionLength);
        }

        public void InnerTransition(float duration)
        {
            if (_hpBarTween != null && _hpBarTween.IsActive())
                _hpBarTween.Kill();

            _hpBarTween = _slider.SlideToValue((int)_innerSlider.GetSliderValue(), duration, _healthBarSO.ChangeInnerHealthCurve).OnComplete(Completed);
        }

        private void Completed() => _isActive = false;
    }

    public interface IHPTransition
    {
        bool IsActive { get; }

        void InnerTransition(float duration);
        void StartTransition(int amount);
        void StopCoroutine();
    }

    public class ReduceHPTransition : IHPTransition
    {
        private readonly MonoBehaviour _sceneObject;
        private readonly HPSlider _slider;
        private readonly HPSlider _innerSlider;
        private readonly HealthBarSO _healthBarSO;
        // private readonly regular transition off slider
        // private readonly speed up transition off slider
        private Tween _hpBarTween;
        private Tween _secondHPBarTween;
        private bool _isActive;
        private Coroutine _coroutine;
        public bool IsActive => _isActive;
        public ReduceHPTransition(MonoBehaviour sceneObj, HPSlider hp, HPSlider inner, HealthBarSO healthBarSO)
        {
            _sceneObject = sceneObj;
            _slider = hp;
            _innerSlider = inner;
            _healthBarSO = healthBarSO;
        }


        public void StartTransition(int amount)
        {
            _isActive = true;
            if (_hpBarTween != null && _hpBarTween.IsActive())
                _hpBarTween.Kill();

            _innerSlider.SetSprite(_healthBarSO.FillDownHealthSprite);
            _hpBarTween = _slider.SlideToValue(amount, _healthBarSO.DelayTillReturn, _healthBarSO.ChangeHealthCurve);

            StopCoroutine();
            _coroutine = _sceneObject.StartCoroutine(ChangeInnerHealthBar());

        }

        public void StopCoroutine()
        {
            if (_coroutine != null)
                _sceneObject.StopCoroutine(_coroutine);
        }

        private IEnumerator ChangeInnerHealthBar()
        {
            float counter = 0;
            while (counter <= _healthBarSO.DelayTillReturn)
            {
                yield return null;
                counter += Time.deltaTime;
            }

            InnerTransition(_healthBarSO.HealthTransitionLength);
        }

        public void InnerTransition(float duration)
        {
            if (_secondHPBarTween != null && _secondHPBarTween.IsActive())
                _secondHPBarTween.Kill();

            _secondHPBarTween = _innerSlider.SlideToValue((int)_slider.GetSliderValue(), duration, _healthBarSO.ChangeInnerHealthCurve).OnComplete(Completed);
        }

        private void Completed() => _isActive = false;
    }

    public class AddTransitionCommand : ISequenceCommand , IPoolable<AddTransitionCommand>
    {
        public event Action OnFinishExecute;
        public event Action<AddTransitionCommand> OnDisposed;

        private IHPTransition _hPTransition;
        private CommandType _commandType;
        private int _amount;
        public CommandType CommandType => _commandType;
        public bool IsReducing;

        
        public void Init(CommandType commandType, bool isReducing , int amount, IHPTransition hPTransition )
        {
            _commandType = commandType;
            IsReducing = isReducing;
            _hPTransition = hPTransition;
            _amount = amount;
        }

        public void Execute()
        {
       
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            OnDisposed?.Invoke(this);
        }
    }
}
