using CardMaga.Battle.Visual;
using CardMaga.Keywords;
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
        [ReadOnly] Dictionary<BuffVisualData,BuffVisualHandler> _buffs = new Dictionary<BuffVisualData,BuffVisualHandler>();

        List<BuffVisualData> _activeBuffs;

        //BuffIcon[] _buffSlots;

        //[SerializeField] BuffIcon _enemyOpponentActionUI;

        [SerializeField] GameObject _buffPrefab;
        #endregion

        private void Awake()
        {
            _activeBuffs = new List<BuffVisualData>();
            //ResetBuffs();
        }
        private void ResetBuffs()//check for bugs
        {
            if (_activeBuffs.Count > 0)
            {
                foreach (var buff in _activeBuffs)
                {
                    buff.Dispose();
                }
                _activeBuffs.Clear();
            }
        }

        public void Init(VisualStatHandler visualStatHandler)
        {
            foreach (var element in visualStatHandler.VisualStatsDictionary)
            {
                var visualStat = element.Value;
                visualStat.OnKeywordValueChanged += VisualStatUpdated;
            }
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
                if (activeBuff.KeywordType == keywordType)
                {
                    buffVisualData = activeBuff;
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
                buffVisualData.Dispose();//is this true?
                _activeBuffs.Remove(buffVisualData);
                return;
            }
            //need to update amount
            buffVisualData.AssignValues(buffVisualData.KeywordType, amount);
        }
        void CreateNewBuffVisualData(KeywordType keywordType, int amount)
        {
            if (amount == 0)
                return;

            var buffVisualData = new BuffVisualData();
            buffVisualData.AssignValues(keywordType, amount);
            _activeBuffs.Add(buffVisualData);
        }
        void UpdateVisuals()
        {

        }
        //BuffIcon GetFreeSlot()
        //{
        //    if (_buffSlots != null && _buffSlots.Length > 0)
        //    {
        //        for (int i = 0; i < _buffSlots.Length; i++)
        //        {
        //            if (_buffSlots[i] != null && !_buffSlots[i].gameObject.activeSelf)
        //            {
        //                return _buffSlots[i];
        //            }
        //        }
        //    }

        //    return CreateBuffIconSlot();
        //}


        //private BuffIcon CreateBuffIconSlot()
        //{
        //    Array.Resize(ref _buffSlots, _buffSlots.Length + 1);
        //    int lastIndex = _buffSlots.Length - 1;
        //    _buffSlots[lastIndex] = GameObject.Instantiate(_buffIconSlotPrefab, this.transform).GetComponent<BuffIcon>();
        //    return _buffSlots[lastIndex];
        //}
        //public void SetBuffIcon(int amount, KeywordType icon)
        //{


        //    if (amount == 0)
        //    {
        //        RemoveBuffIcon(icon);
        //        return;
        //    }


        //    if (CheckForDuplicates(icon))
        //    {
        //        GetDuplicate(icon).SetAmount(amount);
        //        //set text
        //        return;
        //    }
        //    var buffSlot = GetFreeSlot();
        //    buffSlot.KeywordType = icon;
        //}

        //internal void UpdateArmour(int amount)
        //{
        //    armourIcon.SetAmount(amount);
        //}

        //public void RemoveBuffIcon(KeywordType icon)
        //{
        //    if (_buffSlots != null && _buffSlots.Length > 0)
        //    {
        //        for (int i = 0; i < _buffSlots.Length; i++)
        //        {
        //            if (_buffSlots[i].KeywordType == icon)
        //            {
        //                _buffSlots[i].ResetEnumType();
        //                OrderArray();
        //                return;
        //            }
        //        }
        //    }
        //}
        //private bool CheckForDuplicates(KeywordType icon)
        //{
        //    if (_buffSlots != null && _buffSlots.Length > 0)
        //    {
        //        for (int i = 0; i < _buffSlots.Length; i++)
        //        {
        //            if (_buffSlots[i].KeywordType == icon)
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}
        //void OrderArray()
        //{
        //    for (int i = 0; i < _buffSlots.Length - 1; i++)
        //    {
        //        if (_buffSlots[i].gameObject.activeSelf)
        //            continue;

        //        for (int j = i + 1; j < _buffSlots.Length; j++)
        //        {
        //            if (_buffSlots[j].gameObject.activeSelf)
        //            {

        //                // _buffSlots[j].transform.SetParent(null);

        //                _buffSlots[j].transform.SetAsFirstSibling();
        //                //  _buffSlots[i].InitIconData(_buffCollection.GetIconData(_buffSlots[j].GetSetName.GetValueOrDefault()));
        //                //  _buffSlots[j].ResetEnumType();
        //                break;
        //            }

        //            if (j == _buffSlots.Length - 1)
        //                return;

        //        }

        //    }
        //}


        //BuffIcon GetDuplicate(KeywordType icon)
        //{
        //    if (_buffSlots != null && _buffSlots.Length > 0)
        //    {
        //        for (int i = 0; i < _buffSlots.Length; i++)
        //        {
        //            if (_buffSlots[i].KeywordType == icon)
        //            {
        //                return _buffSlots[i];
        //            }
        //        }
        //    }
        //    Debug.LogError("Error in GetDuplicate");
        //    return null;
        //}
        //accses to all icons and colors
        //Array of Icons
        //void(Enum)add
        //void(Enum)remove
    }

}