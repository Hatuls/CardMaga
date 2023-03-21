using CardMaga.Keywords;
using System;
using UnityEngine;

[RequireComponent(typeof(BuffIcon))]
public class BuffIconInteraction : MonoBehaviour
{

    #region Events
    public static event Action<KeywordType> OnBuffIconHold;
    public static event Action OnRelease;
    #endregion
    [SerializeField] BuffIcon _buffIcon;



    private void Start()
    {
        if (_buffIcon == null)
            _buffIcon = GetComponent<BuffIcon>();
    }

    #region Interaction

    bool _flag = false;
    public void OnBuffFinishInteraction()
    {
        if (_flag)
        {
            _flag = false;
            OnRelease?.Invoke();
        }
    }
    public void OnBuffInteraction()
    {
        if (!_flag)
        {
            _flag = true;
            OnBuffIconHold?.Invoke(_buffIcon.KeywordType);
        }
    }

    #endregion
}