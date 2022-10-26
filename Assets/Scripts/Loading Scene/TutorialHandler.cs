using Account;
using CardMaga.Playfab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.Events;

public class TutorialHandler : MonoBehaviour
{
   [SerializeField] private UnityEvent OnFirstLogin;
   [SerializeField] private UnityEvent OnNotFirstLogin;

   private LoginResult _loginResult;

   private void Awake()
   {
      PlayfabLogin.OnSuccessfullLogin += GetLoginResult;
   }

   private void OnDestroy()
   {
      PlayfabLogin.OnSuccessfullLogin -= GetLoginResult;
   }

   private void GetLoginResult(LoginResult loginResult)
   {
      if (loginResult == null)
      {
         Debug.LogError("Login Result is null");  
      }

      _loginResult = loginResult;
   }
   
   public void CheckIfFirstLogin()
   {
      if (true)//!AccountManager.Instance.Data.AccountTutorialData.IsCompletedTutorial || _loginResult.NewlyCreated
      {
         OnFirstLogin?.Invoke();
         Debug.Log("FirstLogin");
      }
      else
      {
         OnNotFirstLogin?.Invoke();
         Debug.Log("NotFirstLogin");
      }
   }
}
