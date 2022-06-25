using Battles;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CardMaga.ActDifficultySO;

namespace Collections
{
    [CreateAssetMenu(fileName = "CharacterCollection", menuName = "ScriptableObjects/Collection")]
    public class CharacterCollectionSO : ScriptableObject
    {
        [SerializeField]
        private CharacterSO[] _charactersSO;
        public CharacterSO[] CharactersSO { get => _charactersSO; }

        private Dictionary<CharacterEnum, CharacterSO> _characterDict;


        public void Init(CharacterSO[] characterSOs)
            => _charactersSO = characterSOs;

 
    }
}
