using Account.GeneralData;
using Battle.Combo;
using Battle.Data;
using Battle.MatchMaking;
using ReiTools.TokenMachine;
using System;
using System.Collections.Generic;
using System.Linq;
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
        if (!IsValid(obj))
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
        return new VerifyCharactersData(obj).Validate();
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


public class VerifyCharactersData
{
    private readonly CharactersData _charactersData;
    public VerifyCharactersData(CharactersData charactersData)
    {
        _charactersData = charactersData;
    }

    public bool Validate()
    {
        bool isValid = true;

        if (_charactersData == null || _charactersData.IsNull())
            return false;


        isValid &= ValidateMainCharacters();

        if (isValid == false)
            return isValid;

        if (!ValidateMainCharacterID())
            _charactersData.MainCharacterID = _charactersData.Characters[0].ID;

        isValid &= (ValidateDecksAmount());

                   if (isValid == false)
            return isValid;

        var mainCharacter = _charactersData.GetMainCharacter();

        if (!ValidateMainDeck() || !ValidateDeck(mainCharacter.Deck[mainCharacter.MainDeck]))
        {
            mainCharacter.SetMainDeck(0);
            if (!ValidateDeck(mainCharacter.Deck[mainCharacter.MainDeck])) // try set it to the default deck
                return false;
        }


        return isValid;
    }

    private bool ValidateDeck(DeckData deckData)
    {
        var cards = deckData.Cards;
        var combos = deckData.Combos;

        bool isCardValid = true;
        Factory.GameFactory gameFactory = Factory.GameFactory.Instance;

        //Validate Deck's Cards
        var allCardsID = gameFactory.CardFactoryHandler.CardCollection.AllCardsID;
        for (int i = 0; i < cards.Length; i++)
        {
            isCardValid &= ValidateCardID(cards[i].ID, allCardsID);
            if (!isCardValid)
                break;
        }

        //Validate Combos
        if (isCardValid && combos.Length > 0)
        {
            var x = gameFactory.ComboFactoryHandler.ComboCollection.AllCombos;
            for (int i = 0; i < combos.Length; i++)
            {
                isCardValid &= ValidateComboID(combos[i].ComboSO(), x);
                if (!isCardValid)
                    break;
            }
        }

        return isCardValid;
    }
    private bool ValidateComboID(ComboSO comboSO, ComboSO[] comboCollection)
    {
        bool isContain = false;
        for (int i = 0; i < comboCollection.Length; i++)
        {
            isContain |= comboSO == comboCollection[i];

            if (isContain)
                break;
        }
        return isContain;
    }
    private bool ValidateCardID(int id, IEnumerable<int> allCardsID)
    {
        bool isValidId = false;
        foreach (var card in allCardsID) // validate CardID
        {
            isValidId |= (card == id);

            if (isValidId)
                break;
        }
        return isValidId;
    }
    private bool ValidateMainDeck()
    {
        var mainCharacter = _charactersData.GetMainCharacter();
        var decks = mainCharacter.Deck;
        int mainDeck = mainCharacter.MainDeck;

        return mainDeck >= 0 && mainDeck < decks.Count;
    }


    private bool ValidateDecksAmount()
    {
        var mainCharacter = _charactersData.GetMainCharacter();
        var decks = mainCharacter.Deck;
        return decks.Count > 0;

    }
    //validate the characters and main character id
    private bool ValidateMainCharacters()
    {
        var characters = _charactersData.Characters;
        return characters.Count > 0;
    }

    private bool ValidateMainCharacterID()
    {
        bool isContainId = false;
        var characters = _charactersData.Characters;
        int mainCharacterID = _charactersData.MainCharacterID;
        for (int i = 0; i < characters.Count; i++)
        {
            isContainId |= (mainCharacterID == characters[i].ID);
            if (isContainId)
                break;
        }
        return isContainId;
    }
}