using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardMaga.UI.PopUp
{
    public interface IPopUpPool
    {
        BasePopUp Pull(PopUpSO visualEffectSO);
    }

    public class PopUpPool 
    {

        private readonly List<BasePopUp> _allPoolObjects;
        private readonly List<BasePopUp> _reservedList;

        private Transform _parent;

        public PopUpPool(Transform defaultParent)
        {
            _parent = defaultParent;
            _allPoolObjects = new List<BasePopUp>();
            _reservedList = new List<BasePopUp>();
        }

    //    public void PopulatePool(BasePopUp type, int amount)
    //    {
    //        for (int i = 0; i < amount; i++)
    //            InstantiateObject(type).Dispose();

    //    }

    //    public BasePopUp Pull(PopUpSO popUpSO)
    //    {
    //        BaseVisualEffect effect = null;
    //        if (_reservedList.Count > 0)
    //        {
    //            for (int i = _reservedList.Count - 1; i >= 0; i--)
    //            {
    //                if (_reservedList[i].ContainTag(visualEffectSO))
    //                {
    //                    effect = _reservedList[i];
    //                    _reservedList.RemoveAt(i);
    //                    break;
    //                }
    //            }
    //        }

    //        if (effect == null)
    //            effect = InstantiateObject(visualEffectSO);


    //        return effect;
    //    }

    //    private BasePopUp InstantiateObject(PopUpSO popUpSO)
    //    {

    //        var cache = MonoBehaviour.Instantiate(popUpSO.PopUpPrefab, _parent);
    //        _allPoolObjects.Add(cache);
    //        cache.OnDisposed += ReturnBack;
    //        return cache;
    //    }

    //    private void ReturnBack(BaseVisualEffect returningEffect)
    //    {
    //        returningEffect.transform.SetParent(_parent);
    //        _reservedList.Add(returningEffect);
    //        returningEffect.gameObject.SetActive(false);
    //    }

    //    public void Dispose()
    //    {
    //        for (int i = 0; i < _allPoolObjects.Count; i++)
    //        {
    //            _allPoolObjects[i].Dispose();
    //            _allPoolObjects[i].OnDisposed -= ReturnBack;
    //        }
    //        _allPoolObjects.Clear();
    //        _reservedList.Clear();
    //    }
    }

}
