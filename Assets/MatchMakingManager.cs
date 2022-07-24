using Account.GeneralData;
using Battle.Data;
using Battle.MatchMaking;
using ReiTools.TokenMachine;
using UnityEngine;
using UnityEngine.Events;

public class MatchMakingManager : MonoBehaviour
{
    [SerializeField] private OperationManager _lookForMatchOperation;

    [SerializeField] [EventsGroup] private UnityEvent OnMatchFound;

    private TokenMachine _tokenMachine;

    private void Awake()
    {
        LookForOpponent.OnOpponentFound += RegisterOpponent;
    }

    private void OnDestroy()
    {
        LookForOpponent.OnOpponentFound -= RegisterOpponent;
    }

    private void RegisterOpponent(string name, CharactersData obj)
    {
        var isPlayer = false;
        BattleData.Instance.AssginCharacter(isPlayer, name, obj.GetMainCharacter);
    }

    public void StartLooking()
    {
        _tokenMachine = new TokenMachine(MatchFound);
        _lookForMatchOperation.Init(_tokenMachine);
        _lookForMatchOperation.StartOperation();
    }

    private void MatchFound()
    {
        OnMatchFound?.Invoke();
    }
}