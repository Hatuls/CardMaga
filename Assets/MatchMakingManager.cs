
using ReiTools.TokenMachine;
using UnityEngine;
using UnityEngine.Events;

public class MatchMakingManager : MonoBehaviour
{


    [SerializeField] OperationManager _lookForMatchOperation;
    private TokenMachine _tokenMachine;
    [SerializeField, EventsGroup]
    private UnityEvent OnMatchFound;
    public void StartLooking()
    {
        _tokenMachine = new TokenMachine(MatchFound);
        _lookForMatchOperation.Init(_tokenMachine);
        _lookForMatchOperation.StartOperation();
    }

    private void MatchFound() => OnMatchFound?.Invoke();
}
