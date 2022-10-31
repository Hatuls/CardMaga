using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSwitch : MonoBehaviour
{
    [SerializeField]
    GameObject _onButtonSwitchObject;
    public void OnSwitch()
    {
        if(_onButtonSwitchObject.activeSelf == true)
        {
            _onButtonSwitchObject.SetActive(false);
        }
        else
        {
            _onButtonSwitchObject.SetActive(true);
        }
    }
}
