using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempSettingButton : MonoBehaviour
{
    [SerializeField] GameObject _gameObjectToChangeState;
    public void SetGameObject()
    {
        if(_gameObjectToChangeState != null && _gameObjectToChangeState.activeSelf)
        {
            System.Console.WriteLine("Object is active");
            _gameObjectToChangeState.SetActive(false);
        }
        else
        {
            System.Console.WriteLine("Object is Not Active");
            _gameObjectToChangeState.SetActive(true);
        }
    }
}
