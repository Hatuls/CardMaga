using UnityEngine;
namespace Battles
{
    [CreateAssetMenu (fileName = "CharactersDictionary",menuName ="ScriptableObjects/Character_Dictionary") ]
    public class CharactersDictionary : ScriptableObject
    {
        [SerializeField] CharacterAbstSO[] _allGameCharacter;

      //  static  System.Collections.Generic.Dictionary<CharactersEnum, CharacterAbstSO> _characterDict = new System.Collections.Generic.Dictionary<CharactersEnum, CharacterAbstSO>();
        public CharacterAbstSO GetCharacter(CharactersEnum charactersEnum)
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



        public CharacterAbstSO GetRandomOpponent()
        {
            if (_allGameCharacter.Length > 0)
            {
                int x;

                do
                {
                    x = Random.Range(0, _allGameCharacter.Length);

                } while (_allGameCharacter[x].GetOpponent == CharactersEnum.Player);


                return _allGameCharacter[x];

            }


            return null;
        }
    }



}
