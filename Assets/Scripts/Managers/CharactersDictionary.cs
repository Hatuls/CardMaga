﻿using UnityEngine;
namespace Battles
{
    [CreateAssetMenu (fileName = "CharactersDictionary",menuName ="ScriptableObjects/Character_Dictionary") ]
    public class CharactersDictionary : ScriptableObject
    {
        [SerializeField] CharacterSO[] _allGameCharacter;

      //  static  System.Collections.Generic.Dictionary<CharactersEnum, CharacterAbstSO> _characterDict = new System.Collections.Generic.Dictionary<CharactersEnum, CharacterAbstSO>();
        public CharacterSO GetCharacter(CharacterTypeEnum charactersEnum)
        {
            if (_allGameCharacter.Length > 0)
            {
                //if (_characterDict.ContainsKey(charactersEnum))
                //    return _characterDict[charactersEnum];

                for (int i = 0; i < _allGameCharacter.Length; i++)
                {
                    if (_allGameCharacter[i].GetOpponent == charactersEnum)
                    {
                     //   _characterDict.Add(charactersEnum, _allGameCharacter[i]);
                        return _allGameCharacter[i];
                    }
                }

                Debug.LogError("Character Stats was not found");
            }

            return null;
        }



        public CharacterSO GetRandomOpponent()
        {

            Debug.LogError("Need To Change The Get RandomOpponent!");
            if (_allGameCharacter.Length > 0)
            {
                int x = 0;

                //do
                //{
                //    x = Random.Range(0, _allGameCharacter.Length);

                //} while (_allGameCharacter[x].GetOpponent == CharacterTypeEnum.Player);


                return _allGameCharacter[x];

            }


            return null;
        }
    }



}
