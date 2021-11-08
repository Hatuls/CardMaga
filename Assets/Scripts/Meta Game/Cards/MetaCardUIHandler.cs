using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Meta.Laboratory
{
    enum CardPositionEnum
    {
        CardCollection = 0,
        Deck = 1,
        Fuse = 2,
        Upgrade = 3,
    }

    public class MetaCardUIHandler : MonoBehaviour
    {
        public static event Action OnCardClickedEvent;
        [SerializeField]
        GameObject _dropList;
        [SerializeField]
        GameObject _useButton;
        [SerializeField]
        GameObject _dismantleButton;
        [SerializeField]
        CardPositionEnum _currentPosition;

        private void Awake()
        {
            OnCardClickedEvent += CloseDropList;
        }
        public void CloseDropList()
        {
            _dropList.SetActive(false);
            _useButton.SetActive(false);
            _dismantleButton.SetActive(false);

        }
        public void OpenDropList()
        {
            _dropList.SetActive(true);
            switch (_currentPosition)
            {
                case CardPositionEnum.CardCollection:
                    _useButton.SetActive(true);
                    break;
                case CardPositionEnum.Deck:
                    break;
                case CardPositionEnum.Fuse:
                    break;
                case CardPositionEnum.Upgrade:
                    _dismantleButton.SetActive(true);
                    break;
                default:
                    break;
            }
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
