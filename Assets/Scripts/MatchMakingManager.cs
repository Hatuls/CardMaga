using System;
using Account.GeneralData;
using Battle.Data;
using Battle.MatchMaking;
using ReiTools.TokenMachine;
using UnityEngine;
using UnityEngine.Events;
[Serializable]
public class BattleCharacterUnityEvent : UnityEvent<Battle.Characters.BattleCharacter> { }
public class MatchMakingManager : MonoBehaviour
{
    public static event Action<Battle.Characters.BattleCharacter> OnOpponentAssign;

    [SerializeField] 
    OperationManager _lookForMatchOperation;
    private TokenMachine _tokenMachine;
    [SerializeField, EventsGroup]
    private UnityEvent OnMatchFound;
    [SerializeField, EventsGroup]
    private BattleCharacterUnityEvent OnOpponentFound;
    [SerializeField, EventsGroup]
    private BattleCharacterUnityEvent OnPlayerAssign;
    private void Awake()
    {
        LookForOpponent.OnOpponentFound += RegisterOpponent;

    }
    private void OnDestroy()
    {
        LookForOpponent.OnOpponentFound -= RegisterOpponent;
    }
    private void Start()
    {
        StartLooking();
    }
    private void RegisterOpponent(string name ,CharactersData obj)
    {
          BattleData.Instance.AssignOpponent(name, obj.GetMainCharacter());

        OnOpponentFound?.Invoke(BattleData.Instance.Right);
          OnOpponentAssign?.Invoke(BattleData.Instance.Right);
    }

    public void StartLooking()
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
