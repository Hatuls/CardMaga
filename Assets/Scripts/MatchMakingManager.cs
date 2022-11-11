using System;
using Account.GeneralData;
using Battle.Data;
using Battle.MatchMaking;
using ReiTools.TokenMachine;
using UnityEngine;
using UnityEngine.Events;

public class MatchMakingManager : MonoBehaviour
{
    public static event Action<Battle.Characters.BattleCharacter> OnOpponentAssign;

    [SerializeField] OperationManager _lookForMatchOperation;
    private TokenMachine _tokenMachine;
    [SerializeField, EventsGroup]
    private UnityEvent OnMatchFound;
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
        bool isPlayer = false;
          BattleData.Instance.AssginCharacter(isPlayer, name, obj.GetMainCharacter());
          
          OnOpponentAssign?.Invoke(BattleData.Instance.Right);
    }

    public void StartLooking()
    {
        _tokenMachine = new TokenMachine(MatchFound);
        _lookForMatchOperation.Init(_tokenMachine);
        _lookForMatchOperation.StartOperation();
    }

    private void MatchFound() => OnMatchFound?.Invoke();
}
