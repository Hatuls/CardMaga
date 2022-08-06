using TMPro;
using UnityEngine;
public class DiamondRewardUI : MonoBehaviour
{
    [SerializeField]
    Animator _animator;
    [SerializeField]
    [HideInInspector]
    int _hashParameterName;

    [SerializeField]
    TextMeshProUGUI _text;


#if UNITY_EDITOR
    [Sirenix.OdinInspector.OnValueChanged("SetHash")]
    [SerializeField]
    public string AniamtionTriggerParameter;

    private void SetHash()
    {
        _hashParameterName = Animator.StringToHash(AniamtionTriggerParameter);
    }
#endif
    public void SetText(string text) => _text.text = text;
    public void PlayAnimation()
        => _animator.SetTrigger(_hashParameterName);
}
