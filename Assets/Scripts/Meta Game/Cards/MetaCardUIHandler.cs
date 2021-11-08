using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Meta.Laboratory
{
    public class MetaCardUIHandler : MonoBehaviour
    {
        public static event Action OnCardClickedEvent;
        [SerializeField]
        GameObject _dropList;

        private void Awake()
        {
            OnCardClickedEvent += CloseDropList;
        }
        public void CloseDropList()
        {
            _dropList.SetActive(false);

        }
        public void OpenDropList()
        {
            _dropList.SetActive(true);
        }
        public void OnCardClicked()
        {
            //There can be only one open Droplist from all of the cards...
            //any action other than pressing a button on the drop list will be closeing the drop list
            //Can be a static class?


            Debug.Log("Changeing Drop List State");
            if (_dropList.activeSelf)
            {
                CloseDropList();
            }
            else
            {
                OnCardClickedEvent.Invoke();
                OpenDropList();
            }
            


        }
    }
}
