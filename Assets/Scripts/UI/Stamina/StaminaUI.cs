using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Characters.Stats;

public class StaminaUI : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI _text;
    [SerializeField] Image _backgroundImage;
    [SerializeField] Image _decorate;
    [SerializeField] Animator _animator;

    int ScaleAnimation = Animator.StringToHash("Scale");
    int RejectAnimation = Animator.StringToHash("StaminaIcon_Reject");
    private void Start()
    {
        //StaminaHandler.StaminaUI = this;

        if (_animator == null)
            _animator = GetComponent<Animator>();
    }

    public void PlayRejectAnimation()
    {
        _animator.Play(RejectAnimation);
    }
    public void SetText(int stamina) {
        _animator.Play("Scale");
        _text.text = (stamina).ToString(); 
    }
}
