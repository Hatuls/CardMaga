using CardMaga.Battle.Visual;
using CardMaga.Keywords;
using CardMaga.Tools.Pools;
using CardMaga.UI.Buff;
using CardMaga.UI.Visuals;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CardMaga.Battle.UI
{

    public class BuffIconsHandler : MonoBehaviour
    {
        #region Fields

        [ReadOnly]
        Dictionary<BuffVisualData, BuffVisualHandler> _activeBuffs;
        [SerializeField] BuffCollectionVisualSO _buffCollectionVisualSO;

        private IPoolObject<BuffVisualData> _dataPool;
        private IPoolMBObject<BuffVisualHandler> _visualPool;

        private VisualStatHandler _visualStatHandler;
        #endregion

        private void Awake()
        {

            _activeBuffs = new Dictionary<BuffVisualData, BuffVisualHandler>();
        }

        private void OnDestroy()
        {
            if (_visualStatHandler == null)
                return;
            foreach (var element in _visualStatHandler.VisualStatsDictionary)
            {
                var visualStat = element.Value;
                if (_buffCollectionVisualSO.IsBuffSOExists(visualStat.KeywordType))
                    visualStat.OnKeywordValueChanged -= VisualStatUpdated;
            }
            ResetBuffs();
        }
        private void ResetBuffs()
        {
            BuffVisualHandler visualBuffHandler;
            BuffVisualData buffDataVisual;
            foreach (var buff in _activeBuffs)
            {
                visualBuffHandler = buff.Value;
                buffDataVisual = buff.Key;

                visualBuffHandler.Dispose();
                buffDataVisual.Dispose();
            }
        }

        public void Init(VisualStatHandler visualStatHandler, IPoolObject<BuffVisualData> dataPool, IPoolMBObject<BuffVisualHandler> visualPool)
        {

            _visualStatHandler = visualStatHandler;
            foreach (var element in visualStatHandler.VisualStatsDictionary)
            {
                var visualStat = element.Value;
                if (_buffCollectionVisualSO.IsBuffSOExists(visualStat.KeywordType))
                    visualStat.OnKeywordValueChanged += VisualStatUpdated;
            }
            _visualPool = visualPool;
            _dataPool = dataPool;
            ResetBuffs();
        }
        private void VisualStatUpdated(KeywordType keywordType, int amount)
        {
            if (!IsBuffExists(keywordType, out BuffVisualData buffVisualData))
                CreateNewBuffVisualData(keywordType, amount);
            else
                UpdateActiveBuff(buffVisualData, amount);
        }
        bool IsBuffExists(KeywordType keywordType, out BuffVisualData buffVisualData)
        {
            foreach (var activeBuff in _activeBuffs)
            {
                if (activeBuff.Key.KeywordType == keywordType)
                {
                    buffVisualData = activeBuff.Key;
                    return true;
                }
            }
            buffVisualData = null;
            return false;
        }
        void UpdateActiveBuff(BuffVisualData buffVisualData, int amount)
        {
            if (amount == 0)
            {
                //if went down to 0 doesnt need to be active
                RemoveBuff(buffVisualData);
                return;
            }
            //need to update amount
            UpdateValue(buffVisualData, amount);
        }

        private void UpdateValue(BuffVisualData buffVisualData, int amount)
        {
            if (!_activeBuffs.TryGetValue(buffVisualData, out BuffVisualHandler visual))
                throw new System.Exception("Cannot update BuffVisualData because it is not found in dictionary");

            buffVisualData.AssignValues(buffVisualData.KeywordType, amount);
            visual.Init(buffVisualData);
        }

        private void RemoveBuff(BuffVisualData buffVisualData)
        {
            if (!_activeBuffs.TryGetValue(buffVisualData, out BuffVisualHandler visual))
                throw new System.Exception("Cannot remove BuffVisualData because it is not found in dictionary");

            visual.Dispose();
            buffVisualData.Dispose();
            _activeBuffs.Remove(buffVisualData);
        }

        void CreateNewBuffVisualData(KeywordType keywordType, int amount)
        {
            if (amount == 0)
                return;

            var buffVisualData = _dataPool.Pull();
            buffVisualData.AssignValues(keywordType, amount);
            var visualBuffHandler = _visualPool.Pull();
            visualBuffHandler.transform.SetParent(this.transform);
            visualBuffHandler.Init(buffVisualData);
            _activeBuffs.Add(buffVisualData, visualBuffHandler);
        }
    }

}