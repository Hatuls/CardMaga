using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class ButtonUI : MonoBehaviour
{

    [SerializeField] UnityEvent _onBtnPressed;
    [SerializeField] private Button btn;

    protected Button Btn
    {
        get
        {
            if (btn == null)
            {
                btn = GetComponent<Button>();
            }
            return btn;
        }
        private set => btn = value;
    }


    private void OnEnable()
    {
        Btn.onClick.AddListener(ButtonPressed);
    }
    private void OnDisable()
    {
        Btn.onClick.RemoveListener(ButtonPressed);
    }

    public virtual void ButtonPressed()
    {
        _onBtnPressed?.Invoke();

    }
}
