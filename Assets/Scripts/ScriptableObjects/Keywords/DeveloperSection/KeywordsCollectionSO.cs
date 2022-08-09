
using System.Linq;
using UnityEngine;
namespace Keywords
{
    [CreateAssetMenu(fileName = "Keywords Collection", menuName = "ScriptableObjects/Collections/Keywords")]
    public class KeywordsCollectionSO : ScriptableObject
    {
        [SerializeField]
        KeywordSO[] _keywords;

        public void Init(KeywordSO[] keywords)
            => _keywords = keywords;


        public KeywordSO GetKeywordSO(KeywordTypeEnum keyword)
            => _keywords.FirstOrDefault(k => k.GetKeywordType == keyword);
    }


}
