using ReiTools.TokenMachine;
using UnityEngine;

public class LoadingAccount : MonoBehaviour
{
    public void Init(ITokenReciever tokenReciever)
    {
        using (tokenReciever.GetToken())
            Account.AccountManager.Instance.Init();
    }
}
