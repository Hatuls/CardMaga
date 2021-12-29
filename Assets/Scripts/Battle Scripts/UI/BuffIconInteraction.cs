using Keywords;
using System;
using UnityEngine;

[RequireComponent(typeof(BuffIcon))]
public class BuffIconInteraction : MonoBehaviour
{

    #region Events
    public static event Action<KeywordTypeEnum> OnBuffIconHold;
    public static event Action OnRelease;
    #endregion
    [SerializeField] BuffIcon _buffIcon;



    private void Start()
    {
        if (_buffIcon == null)
            _buffIcon = GetComponent<BuffIcon>();
    }

    #region Interaction

    public void OnBuffFinishInteraction() => OnRelease?.Invoke();
    public void OnBuffInteraction() => OnBuffIconHold?.Invoke(_buffIcon.KeywordType);


    #endregion
}