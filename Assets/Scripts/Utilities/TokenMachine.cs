
using System;
using System.Collections.Generic;
using UnityEngine;

public class TokenMachine : MonoBehaviour
{
    private List<WeakReference<Token>> _tokens = new List<WeakReference<Token>>(1);

    public TokenMachine(Action OnStart = null, Action OnFinish = null)
    {

    }

    //public Token GetToken()
    //{

    //}


}

public sealed class Token
{
    Action<Token> _onDispose;
    public Token(Action<Token> onDispose)
    {
        _onDispose = onDispose;
    }
    ~Token()
=> Dispose();

    public void Dispose()
    {
        _onDispose?.Invoke(this);
    }
}