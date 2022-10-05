using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;
using CardMaga.DialogueSO;
using TMPro;
using UnityEngine.UI;
using System;
using ReiTools.TokenMachine;

public class DialoguesFlow : MonoBehaviour
{
    #region Fields

    [SerializeField] private RectTransform _dialoguesFlow;
    [SerializeField] private List<DialogueSO> _dialoguesList;
    [SerializeField] private Image _currentCharacterImage;
    [SerializeField] private TextMeshProUGUI _currentCharacterText;
    [SerializeField] private bool ClosePanelAtEnding;
    private ClickHelper _clickHelper;
    private int _currentDialogue;
    private IDisposable _token;

    #endregion
    #region Events
    [SerializeField] UnityEvent OnFlowStart;
    [SerializeField] UnityEvent OnDialoguesUpdate;
    [SerializeField] UnityEvent OnAfterDelay;
    [SerializeField] UnityEvent OnFlowEnd;
    #endregion

    #region PrivateFunction

    public void StartFlow(ITokenReciever tokenReciever)
    {
        _token = tokenReciever.GetToken();
        FirstDialogue();
    }

    public void FirstDialogue()
    {
        _currentDialogue = 0;
        _clickHelper = ClickHelper.Instance;
        UpdateDialogues(_currentDialogue);
        SendDialogue();
    }

    private void UpdateDialogues(int position)
    {
        OnDialoguesUpdate.Invoke();
        _currentCharacterImage.sprite = _dialoguesList[position]._characterSprite;
        _currentCharacterText.text = _dialoguesList[position]._characterText;
    }

    private void SendDialogue()
    {
        StartDelay();
        _clickHelper.LoadObject(true, false, MoveNextDialogues, _dialoguesFlow);
    }

    private void StartDelay()
    {
        _clickHelper.ClickBlocker.BlockInputForSeconds(_dialoguesList[_currentDialogue]._delayTimeForClick, AfterDelay);
    }

    private void MoveNextDialogues()
    {
        _currentDialogue++;
        if (_currentDialogue <= _dialoguesList.Count - 1)
        {
            UpdateDialogues(_currentDialogue);
            StartDelay();
        }

        else
        {
            if (ClosePanelAtEnding)
                _clickHelper.Close();

            ReleaseToken();
        }
    }

    private void ReleaseToken()
    {
        if (_token != null)
            _token.Dispose();

        else
            Debug.LogError("No token to release");
    }

    private void AfterDelay()
    {
        if(OnAfterDelay!=null)
        OnAfterDelay.Invoke();
    }

    #endregion
}
