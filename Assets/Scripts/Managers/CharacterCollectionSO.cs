﻿using Battle;
using UnityEngine;

namespace Collections
{
    [CreateAssetMenu(fileName = "CharacterCollection", menuName = "ScriptableObjects/Collection")]
    public class CharacterCollectionSO : ScriptableObject
    {
        [SerializeField]
        private CharacterSO[] _charactersSO;
        public CharacterSO[] CharactersSO { get => _charactersSO; }



        public void Init(CharacterSO[] characterSOs)
            => _charactersSO = characterSOs;


    }
}
