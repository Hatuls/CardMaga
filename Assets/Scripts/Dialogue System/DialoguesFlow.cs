
﻿using System.Collections;
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
    [SerializeField] private bool _closePanelAtEnding;
    [SerializeField] private bool _loadOnTutorialPanel;
    private TutorialClickHelper _tutorialClickHelper;
    private int _currentDialogue;
    private IDisposable _token;

    #endregion
    #region Events
    [SerializeField] private UnityEvent OnFlowStart;
    [SerializeField] private UnityEvent OnDialoguesUpdate;
    [SerializeField] private UnityEvent OnAfterDelay;
    [SerializeField] private UnityEvent OnFlowEnd;
    #endregion

    #region PrivateFunction

    private void Awake()
    {
        _tutorialClickHelper = TutorialClickHelper.Instance;
    }


    private void UpdateDialogues(int position)
    {
        OnDialoguesUpdate.Invoke();
        _currentCharacterImage.sprite = _dialoguesList[position]._characterSprite;
        _currentCharacterText.text = _dialoguesList[position]._characterText;
    }

    private void SendDialogue()
    {
        //StartDelay();
            _tutorialClickHelper.LoadObject(true, false, MoveNextDialogues, _dialoguesFlow);

    }

    private void StartDelay()
    {
        OnAfterDelay.Invoke();
    }

    private void MoveNextDialogues()
    {
        _currentDialogue++;
        if (_currentDialogue <= _dialoguesList.Count - 1)
        {
            UpdateDialogues(_currentDialogue);
            //StartDelay();
        }

        else
            EndFlow();
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

    private void ClosePanel()
    {
        if (_closePanelAtEnding)
        {
                _tutorialClickHelper.Close();
        }
    }

    private void CheckActivation()
    {
            if (!_tutorialClickHelper.gameObject.activeSelf)
                _tutorialClickHelper.gameObject.SetActive(true);

    }

    #endregion

    #region PublicFunctions
    public void StartFlow(ITokenReciever tokenReciever)
    {
        gameObject.SetActive(true);
        _token = tokenReciever.GetToken();
        FirstDialogue();
    }
    public void FirstDialogue()
    {
        _currentDialogue = 0;
        OnFlowStart.Invoke();
        UpdateDialogues(_currentDialogue);
        SendDialogue();
    }

    public void EndFlow()
    {
        CheckActivation();
        OnFlowEnd.Invoke();
        ClosePanel();
        ReleaseToken();
        gameObject.SetActive(false);
    }

    #endregion
}