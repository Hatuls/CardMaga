using Battle;
using CardMaga.SequenceOperation;
using Keywords;
using ReiTools.TokenMachine;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace CardMaga.Battle.UI
{
    [Serializable]
    public class BuffIconManager :ISequenceOperation<IBattleManager>
    {
        [SerializeField]
        private BuffIconsHandler _rightBuffIconHandler;
        [SerializeField]
        private BuffIconsHandler _leftBuffIconHandler;

        public int Priority => 0;

        public void ExecuteTask(ITokenReciever tokenMachine, IBattleManager data)
        {
            // if need to be initalize
        }

        public BuffIconsHandler GetBuffIconsHandler(bool isLeft) => isLeft ? _leftBuffIconHandler : _rightBuffIconHandler;


#if UNITY_EDITOR
        public void AssignBuffsFields()
        {
            var allBuffs = MonoBehaviour.FindObjectsOfType<BuffIconsHandler>();

            for (int i = 0; i < allBuffs.Length; i++)
            {
                if (allBuffs[i].gameObject.name.Contains("Player"))
                    _leftBuffIconHandler = allBuffs[i];
                else if (allBuffs[i].gameObject.name.Contains("Enemy"))
                    _rightBuffIconHandler = allBuffs[i];
                else
                    throw new Exception("More than 2 BuffIconHandler found in scene!\nCheck this object -> " + allBuffs[i].gameObject.name);
            }
        }
#endif
    }
    public class RemoveUIIcon : UnityEvent<bool, KeywordTypeEnum> { }
    public class BuffIconsHandler : MonoBehaviour
    {
#region Fields


        [SerializeField] BuffIcon armourIcon;
        [SerializeField]
        BuffIcon[] _buffSlots;

        [SerializeField] BuffIcon _enemyOpponentActionUI;

        [SerializeField] bool isPlayer;

        [SerializeField] GameObject _buffIconSlotPrefab;

#endregion

        private void Awake()
        {
            Init();
        }


        private void Init()//check for bugs
        {
            if (_buffSlots != null && _buffSlots.Length > 0)
            {
                for (int i = 0; i < _buffSlots.Length; i++)
                {
                    if (_buffSlots[i] != null && _buffSlots[i].gameObject.activeSelf)
                    {
                        _buffSlots[i].ResetEnumType();
                        if (_buffSlots[i].gameObject.activeSelf)
                            _buffSlots[i].gameObject.SetActive(false);
                    }
                }
            }

        }
        BuffIcon GetFreeSlot()
        {
            if (_buffSlots != null && _buffSlots.Length > 0)
            {
                for (int i = 0; i < _buffSlots.Length; i++)
                {
                    if (_buffSlots[i] != null && !_buffSlots[i].gameObject.activeSelf)
                    {
                        return _buffSlots[i];
                    }
                }
            }

            return CreateBuffIconSlot();
        }


        private BuffIcon CreateBuffIconSlot()
        {
            Array.Resize(ref _buffSlots, _buffSlots.Length + 1);
            int lastIndex = _buffSlots.Length - 1;
            _buffSlots[lastIndex] = GameObject.Instantiate(_buffIconSlotPrefab, this.transform).GetComponent<BuffIcon>();
            return _buffSlots[lastIndex];
        }
        public void SetBuffIcon(int amount, KeywordTypeEnum icon)
        {


            if (amount == 0)
            {
                RemoveBuffIcon(icon);
                return;
            }


            if (CheckForDuplicates(icon))
            {
                GetDuplicate(icon).SetAmount(amount);
                //set text
                return;
            }
            var buffSlot = GetFreeSlot();
            buffSlot.KeywordType = icon;
        }

        internal void UpdateArmour(int amount)
        {
            armourIcon.SetAmount(amount);
        }

        public void RemoveBuffIcon(KeywordTypeEnum icon)
        {
            if (_buffSlots != null && _buffSlots.Length > 0)
            {
                for (int i = 0; i < _buffSlots.Length; i++)
                {
                    if (_buffSlots[i].KeywordType == icon)
                    {
                        _buffSlots[i].ResetEnumType();
                        OrderArray();
                        return;
                    }
                }
            }
        }
        private bool CheckForDuplicates(KeywordTypeEnum icon)
        {
            if (_buffSlots != null && _buffSlots.Length > 0)
            {
                for (int i = 0; i < _buffSlots.Length; i++)
                {
                    if (_buffSlots[i].KeywordType == icon)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        void OrderArray()
        {
            for (int i = 0; i < _buffSlots.Length - 1; i++)
            {
                if (_buffSlots[i].gameObject.activeSelf)
                    continue;

                for (int j = i + 1; j < _buffSlots.Length; j++)
                {
                    if (_buffSlots[j].gameObject.activeSelf)
                    {

                        // _buffSlots[j].transform.SetParent(null);

                        _buffSlots[j].transform.SetAsFirstSibling();
                        //  _buffSlots[i].InitIconData(_buffCollection.GetIconData(_buffSlots[j].GetSetName.GetValueOrDefault()));
                        //  _buffSlots[j].ResetEnumType();
                        break;
                    }

                    if (j == _buffSlots.Length - 1)
                        return;

                }

            }
        }


        BuffIcon GetDuplicate(KeywordTypeEnum icon)
        {
            if (_buffSlots != null && _buffSlots.Length > 0)
            {
                for (int i = 0; i < _buffSlots.Length; i++)
                {
                    if (_buffSlots[i].KeywordType == icon)
                    {
                        return _buffSlots[i];
                    }
                }
            }
            Debug.LogError("Error in GetDuplicate");
            return null;
        }
        //accses to all icons and colors
        //Array of Icons
        //void(Enum)add
        //void(Enum)remove
    }

}