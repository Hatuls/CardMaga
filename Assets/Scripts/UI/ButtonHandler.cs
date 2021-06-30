using Unity.Events;
using UnityEngine.UI;
using UnityEngine;
[RequireComponent(typeof(Button))]
public class ButtonHandler : MonoBehaviour
{
    [SerializeField] VoidEvent _onButtonPressed;
    [SerializeField] SoundsEvent _playSound;

    private void OnEnable()
    {
        GetComponent<Button>().onClick.AddListener(ButtonPressed);
    }
    private void OnDisable()
    {
       GetComponent<Button>().onClick.RemoveListener(ButtonPressed);
    }
    public void ButtonPressed()
    {
        _onButtonPressed?.Raise();
        _playSound?.Raise(SoundsNameEnum.ButtonTapped);
    }
}
