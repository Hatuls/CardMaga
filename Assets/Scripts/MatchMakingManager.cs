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
    public static event Action OnOpponentValid;
    public static event Action OnOpponentCorrupted;

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
    /// <summary>
    /// Get a Potential User's Data
    /// </summary>
    /// <param name="name"></param>
    /// <param name="obj"></param>
    private void RegisterOpponent(string name, CharactersData obj)
    {
#if UNITY_EDITOR
        Debug.Log(name);
#endif


        // Check if its valid 
        if(!IsValid(obj))
        {
            OnOpponentCorrupted?.Invoke();
            return;
        }


        OnOpponentValid?.Invoke();
        BattleData.Instance.AssignOpponent(name, obj.GetMainCharacter());
        OnOpponentFound?.Invoke(BattleData.Instance.Right);
    }

    //Validate the character's data
    private bool IsValid(CharactersData obj)
    {

        //  Validator.Valid()
        return true;
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
