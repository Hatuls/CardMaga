using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaCardUIHandler : MonoBehaviour
{
    [SerializeField]
    GameObject _dropList;
    public void OnCardClicked()
    {
        //There can be only one open Droplist from all of the cards...
        //any action other than pressing a button on the drop list will be closeing the drop list
        //Can be a static class?
        Debug.Log("Changeing Drop List State");
        if(_dropList.activeSelf)
        {
            _dropList.SetActive(false);
        }
        else
        {
            _dropList.SetActive(true);
        }
    }
}
