using Account.GeneralData;
using Battle.Data;
using Battle.MatchMaking;
using CardMaga.ValidatorSystem;
using ReiTools.TokenMachine;
using System;
using UnityEngine;
using UnityEngine.Events;
[Serializable]
public class BattleCharacterUnityEvent : UnityEvent<Battle.Characters.BattleCharacter> { }
public class MatchMakingManager : MonoBehaviour
{
    [SerializeField]
    OperationManager _lookForMatchOperation;
    private TokenMachine _tokenMachine;
    [SerializeField, EventsGroup]
    private UnityEvent OnMatchFound;
    [SerializeField, EventsGroup]
    private UnityTokenMachineEvent OnTutorialGameStarted;
    [SerializeField, EventsGroup]
    private BattleCharacterUnityEvent OnOpponentFound;
    [SerializeField, EventsGroup]
    private BattleCharacterUnityEvent OnPlayerAssign;

    private MatchMakingValidation _matchMakingValidation;


    private void Awake()
    {
        LookForOpponent.OnOpponentFound += RegisterOpponent;
        _matchMakingValidation = new MatchMakingValidation();
    }
    private void OnDestroy()
    {
        LookForOpponent.OnOpponentFound -= RegisterOpponent;
    }
    private void Start()
    {
        if (BattleData.Instance.BattleConfigSO.IsTutorial)
            StartTutorialMatchMaking();
        else
            StartOnlineLooking();

    }

    private void StartTutorialMatchMaking()
    {
        AssignPlayerFound();
        var opponent = BattleData.Instance.Right;
        OnOpponentFound?.Invoke(opponent);
        _tokenMachine = new TokenMachine(MatchFound);
        OnTutorialGameStarted?.Invoke(_tokenMachine);
    }

    private void RegisterOpponent(string name, CharactersData obj)
    {
#if UNITY_EDITOR
        Debug.Log(name);
#endif
        obj.IsValid();

        BattleData.Instance.AssignOpponent(name, obj.GetMainCharacter());

        OnOpponentFound?.Invoke(BattleData.Instance.Right);
    }

    public void StartOnlineLooking()
    {
        AssignPlayerFound();
        _tokenMachine = new TokenMachine(MatchFound);
        _lookForMatchOperation.Init(_tokenMachine);
        _lookForMatchOperation.StartOperation();
    }
    private void AssignPlayerFound()
    {
        OnPlayerAssign?.Invoke(BattleData.Instance.Left);
    }
    private void MatchFound() => OnMatchFound?.Invoke();
}


