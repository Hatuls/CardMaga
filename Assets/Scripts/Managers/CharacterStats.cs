using System;
using UnityEngine;
namespace Characters.Stats
{
    [Serializable]
    public struct CharacterStats
    {
        #region Fields
        [Header("Character Stats: ")]
        [Tooltip("Character Max Health:")]
        [SerializeField] int _maxHealth;

        [Tooltip("Character Health:")]
        [SerializeField] int _health;

        [Tooltip("Character Defense:")]
        [SerializeField] int _defense;

        [Tooltip("Character Gold:")]
        [SerializeField] int _gold;

        [Tooltip("Character Strength:")]
        [SerializeField] int _strengthPoint;

        [Tooltip("Character Bleed:")]
        [SerializeField] int _bleedPoints;

        [Tooltip("Character Cards Draw")]
        [SerializeField] int _cardDraw;
        #endregion

        #region Properties
        public int DrawCardsAmount { get => _cardDraw; set => _cardDraw = value; }
        public int Health {  get => _health; set => _health = value; }
        public int MaxHealth { get => _maxHealth; set =>_maxHealth =value; }
        public int Shield { get => _defense; set => _defense = value; }
        public int Gold { get => _gold; set => _gold = value; }
        public int Strength { get => _strengthPoint; set => _strengthPoint = value; }
        public int Bleed { get => _bleedPoints; set => _bleedPoints = value; }

        #endregion


    }
}