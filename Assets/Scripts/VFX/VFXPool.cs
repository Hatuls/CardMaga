
using System;
using System.Collections.Generic;
using UnityEngine;
using static CardMaga.Battle.Players.TagHelper;
namespace CardMaga.VFX
{
    public interface IVFXPool
    {
        BaseVisualEffect Pull(BattleVisualEffectSO visualEffectSO);
    }

    public class VFXPool : IDisposable, IVFXPool
    {

        private readonly List<BaseVisualEffect> _allPoolObjects;
        private readonly List<BaseVisualEffect> _reservedList;

        private Transform _parent;

        public VFXPool(Transform defaultParent)
        {
            _parent = defaultParent;
            _allPoolObjects = new List<BaseVisualEffect>();
            _reservedList = new List<BaseVisualEffect>();
        }

        public void PopulatePool(BattleVisualEffectSO type, int amount)
        {
            for (int i = 0; i < amount; i++)
                InstantiateObject(type).Dispose();

        }

        public BaseVisualEffect Pull(BattleVisualEffectSO visualEffectSO)
        {
            BaseVisualEffect effect = null;
            if (_reservedList.Count > 0)
            {
                for (int i = _reservedList.Count - 1; i >= 0; i--)
                {
                    if (_reservedList[i].ContainTag(visualEffectSO))
                    {
                        effect = _reservedList[i];
                        _reservedList.RemoveAt(i);
                        break;
                    }
                }
            }

            if (effect == null)
                effect = InstantiateObject(visualEffectSO);

           
            return effect;
        }

        private BaseVisualEffect InstantiateObject(BattleVisualEffectSO visualEffectSO)
        {

                var cache = MonoBehaviour.Instantiate(visualEffectSO.VFXPrefab, _parent);
                _allPoolObjects.Add(cache);
                cache.OnDisposed += ReturnBack;
                return cache;
        }

        private void ReturnBack(BaseVisualEffect returningEffect)
        {
            returningEffect.transform.SetParent(_parent);
            _reservedList.Add(returningEffect);
            returningEffect.gameObject.SetActive(false);
        }

        public void Dispose()
        {
            for (int i = 0; i < _allPoolObjects.Count; i++)
            {
                _allPoolObjects[i].Dispose();
                _allPoolObjects[i].OnDisposed -= ReturnBack;
            }
            _allPoolObjects.Clear();
            _reservedList.Clear();
        }
    }
}