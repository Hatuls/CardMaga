using ReiTools.TokenMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkSceneManager : MonoBehaviour
{
    TokenFactory.TokenMachine _tokenMachine;

    public static event Action<IRecieveOnlyTokenMachine> 
    private void Awake()
    {
        _tokenMachine = new TokenFactory.TokenMachine()
    }
}
