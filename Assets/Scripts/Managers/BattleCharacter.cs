using Account.GeneralData;
using CardMaga.Battle.Players;
using CardMaga.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Battle.Characters
{
    [Serializable]
    public class BattleCharacter 
    {
        [SerializeField]
        private string _displayName;
        [SerializeField]
        private TagSO[] _tags;

        [SerializeField]
        private CharacterBattleData _characterData;

        [SerializeField]
        private BattleCharacterVisual _battleCharacterVisual;


        public CharacterBattleData CharacterData { get => _characterData; private set => _characterData = value; }
        public string DisplayName { get => _displayName; }
        public IReadOnlyList<TagSO> Tags => _tags;
        public BattleCharacterVisual BattleCharacterVisual
        {
            private set => _battleCharacterVisual = value;
            get
            {
                if (_battleCharacterVisual == null)
                    _battleCharacterVisual = new BattleCharacterVisual();
                return _battleCharacterVisual;
            }
        }

        public BattleCharacter() { }

        public BattleCharacter(string displayName, Account.GeneralData.Character data)
        {
            if (data == null)
                throw new Exception("Characters: Data Is Null");
            _displayName = displayName;
            _characterData = new CharacterBattleData(data);

            var modelSO = _characterData.CharacterSO.ModelSO;

            BattleCharacterVisual.Init(modelSO, data.CurrentModel, data.CurrentColor);
        }

        public BattleCharacter(string displayName, Character data, BattleCharacter otherCharacter) : this(displayName, data)
        {
            if (!BattleCharacterVisual.Equals(otherCharacter.BattleCharacterVisual))
                return;
            BattleCharacterVisual.TintColor();
        }

        public bool RemoveCombo(int comboID)
        {
            var combo = _characterData.ComboRecipe.ToList();
            for (int i = 0; i < combo.Count; i++)
            {
                if (combo[i].ID == comboID)
                {
                    combo.RemoveAt(i);
                    _characterData.ComboRecipe = combo.ToArray();
                    return true;
                }
            }

            return false;
        }

        public bool RemoveCardFromDeck(int InstanceID)
        {
            var deckList = _characterData.CharacterDeck.ToList();

            BattleCardData battleCard = deckList.Find((x) => x.CardInstance.InstanceID == InstanceID);

            bool check = deckList.Remove(battleCard);
            if (check)
                _characterData.CharacterDeck = deckList.ToArray();
            return check;
        }

        public bool AddCardToDeck(CardCore card) => AddCardToDeck(card.CardSO(), card.Level);

        public bool AddCardToDeck(CardSO card, int level = 0)
        {
            if (card == null)
                throw new Exception("Cannot add battleCard to deck the battleCard you tried to add is null!");

            var cardCreated = Factory.GameFactory.Instance.CardFactoryHandler.CreateCard(card, level);

            return AddCardToDeck(cardCreated);
        }

        public bool AddCardToDeck(BattleCardData battleCard)
        {
            if (battleCard == null)
                throw new Exception("Character Class : BattleCard is null");

            var deck = _characterData.CharacterDeck;
            int length = _characterData.CharacterDeck.Length;
            Array.Resize(ref deck, length + 1);
            deck[length] = battleCard;
            _characterData.CharacterDeck = deck;

            return battleCard != null;
        }

        public bool AddComboRecipe(Battle.Combo.BattleComboData battleComboData)
        {
            bool hasThisCombo = false;
            var comboRecipe = _characterData.ComboRecipe;
            for (int i = 0; i < comboRecipe.Length; i++)
            {
                hasThisCombo = comboRecipe[i].ID == battleComboData.ID;

                if (hasThisCombo)
                {
                    if (battleComboData.Level > comboRecipe[i].Level)
                        comboRecipe[i] = battleComboData;
                    else
                        return false;

                    break;
                }

            }

            if (hasThisCombo == false)
            {
                Array.Resize(ref comboRecipe, comboRecipe.Length + 1);
                comboRecipe[comboRecipe.Length - 1] = battleComboData;

                hasThisCombo = true;
            }
            _characterData.ComboRecipe = comboRecipe;

            return hasThisCombo;
        }

#if UNITY_EDITOR
        [Sirenix.OdinInspector.Button]
        private void TryAssignSkin()
        {
            var modelSO = _characterData.CharacterSO.ModelSO;
            BattleCharacterVisual.Init(modelSO, 0, 0);
        }
#endif
    }
    [Serializable]
    public class BattleCharacterVisual : IEquatable<BattleCharacterVisual>
    {
        [SerializeField]
        private BattleVisualCharacter _battleVisualCharacter;
        private ModelSkin _characterSkin;
        private int _colorID;
        private int _characterID;
        private ModelSO _modelSO;
        public AvatarHandler Model => BattleVisualCharacter.Model;
        public Material Material => BattleVisualCharacter.Material;
        public Sprite Portrait => BattleVisualCharacter.Portrait;

        public int ColorID { get => _colorID; private set => _colorID = value; }
        public int CharacterID { get => _characterID; private set => _characterID = value; }
        public BattleVisualCharacter BattleVisualCharacter
        {
            get
            {
                if (_battleVisualCharacter == null)
                    _battleVisualCharacter = new BattleVisualCharacter();
                return _battleVisualCharacter;
            }
            private set => _battleVisualCharacter = value;
        }

        public bool Equals(BattleCharacterVisual other)
        => other.ColorID == ColorID && other.CharacterID == CharacterID;

        public void Init(ModelSO modelSO, int characterModel, int colorID)
        {
            _modelSO = modelSO;
            CharacterID = characterModel;
            _characterSkin = modelSO.GetCharacterSkin(CharacterID);
            ColorID = colorID;
            BattleVisualCharacter.Init(_characterSkin, _characterSkin.GetSkin(ColorID));
        }
        public void TintColor()
        {
            BattleVisualCharacter.Init(_characterSkin, _characterSkin.GetRandomSkin(_colorID));
        }
    }
}
