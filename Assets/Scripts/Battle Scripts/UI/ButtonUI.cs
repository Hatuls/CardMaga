using Unity.Events;
using UnityEngine.UI;
using UnityEngine;

using UnityEngine.Events;


[RequireComponent(typeof(Button))]
public  class ButtonUI : MonoBehaviour
{

    [SerializeField] UnityEvent _onBtnPressed;
    [SerializeField]protected StringEvent _playSound;
    [SerializeField] string _soundToPlay;
     private void OnEnable()
    {
        GetComponent<Button>().onClick.AddListener(ButtonPressed);
    }
    private void OnDisable()
    {
       GetComponent<Button>().onClick.RemoveListener(ButtonPressed);
    }

    public virtual void ButtonPressed()
    {
        _onBtnPressed?.Invoke();
        _playSound?.Raise(_soundToPlay);
    }
}
