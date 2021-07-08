using System.Collections.Generic;
using UnityEngine;
using Keywords;
using System.Threading;
using Battles.UI;
using Collections.RelicsSO;
using ThreadsHandler;
namespace Relics
{
    public class RelicManager : MonoSingleton<RelicManager>
    {
        #region Events
        [SerializeField] Unity.Events.VoidEvent _resetDetectingCards;
        #endregion
        #region Fields
        [SerializeField] RelicSO[] _playerRelicsArr;
        [SerializeField] RelicCollectionSO _totalRelics;
        [SerializeField] int _maxAmountOfRelics;
        Dictionary<RelicNameEnum, RelicSO> _relicDict;
        List<RelicFound> _relicFoundList;

        ThreadList _thread;
        bool _threadFinished;
        #endregion

        #region Properties
        public List<RelicFound> GetRelicFounds => _relicFoundList;
        public RelicSO[] GetPlayerRelicArr => _playerRelicsArr;
        public RelicCollectionSO GetTotalRelics => _totalRelics;
        public int GetMaxAmountOfRelics => _maxAmountOfRelics;
        public Dictionary<RelicNameEnum, RelicSO> GetRelicDict => _relicDict;
        #endregion

        public void AddRelic(RelicSO relicAbst)
        {
            // Needs to Add Logic
        }
        public void RemoveRelic(RelicSO relicAbst)
        {
            // Needs to Add Logic
        }
        public string GetRelicName(RelicNameEnum relicNameEnum)
        {
            return relicNameEnum.ToString();
        }
        public RelicSO GetRelicScript(RelicNameEnum relicNameEnum)
        {
            if (GetRelicDict != null
                && GetRelicDict.TryGetValue(relicNameEnum, out RelicSO myRelic))
                return myRelic;

            Debug.LogError("Error in GetRelicScript Func");
            return null;
        }
        public void ResetPlayerRelic()
        {
            if (_playerRelicsArr == null && _playerRelicsArr.Length == 0)
            {
                Debug.LogError("Error in ResetPlayerRelic Func");
                return;
            }
            for (int i = 0; i < _playerRelicsArr.Length; i++)
            {
                if (_playerRelicsArr[i] != null)
                    _playerRelicsArr[i] = null;
            }
        }
        public KeywordData[] GetRelicsEffect(RelicSO relicAbst)
        {
            if (relicAbst != null && relicAbst.GetKeywordEffect != null)
                return relicAbst.GetKeywordEffect;

            Debug.LogError("Error in GetRelicEffect Func");
            return null;

        }
        public Animation GetAnimationFromRelic(RelicSO relic)
        {
            return null;
        }
        public override void Init()
        {
            if (_relicFoundList == null)
                _relicFoundList = new List<RelicFound>();

            _relicFoundList.Clear();

            _thread = new ThreadList(ThreadHandler.GetNewID, CheckForRelics);

        }
        public void DetectRelics()
        {
            if (_thread == null)
            {
                _thread = new ThreadList(ThreadHandler.GetNewID, CheckForRelics);
            }

            ThreadHandler.StartThread(_thread);

        }

        private void CheckForRelics()
        {
            //   var placementCards = Battles.Deck.DeckManager.Instance.GetCardsFromDeck(Battles.Deck.DeckEnum.Selected);

            //    if (placementCards == null)
            //        return;

            //    _relicFoundList.Clear();

            //    if (_playerRelicsArr != null && _playerRelicsArr.Length >0)
            //    { 
            //        int counter;

            //        var placeHolderUI = PlaceHolderHandler.Instance.PlayerPlaceHolder.GetPlaceHolderSlots;
            //        foreach (var relic in _playerRelicsArr)
            //        {
            //             counter = 0;

            //            for (int i = 0; i < placementCards.Length; i++)
            //            {
            //                if (placementCards[i] != null && placementCards[i].GetSetCard.GetBodyPartEnum == relic.GetCombo[counter])
            //                {
            //                    counter++;
            //                    if (counter >= relic.GetCombo.Length)
            //                    {

            //                        _relicFoundList.Add(new RelicFound( relic,i));
            //                        Debug.Log(relic.GetRelicName.ToString());

            //                         counter = 0;
            //                    }
            //                }
            //                else
            //                {
            //                    counter = 0;
            //                    continue;
            //                }
            //            }
            //        }
            //    }

            //    Debug.Log("Thread finished and Found the amount of " + _relicFoundList.Count);

            //}
        }

        public class RelicFound
        {
            public RelicSO _relic;
            public int _lastIndex;
            public int _firstIndex;
            public RelicFound(RelicSO relic, int index)
            {
                _relic = relic;
                _lastIndex = index;
                _firstIndex = _lastIndex - (_relic.GetCombo.Length - 1);
            }
        }
    }
}


