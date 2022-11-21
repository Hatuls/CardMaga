using Account;
using UnityEngine;
using UnityEngine.Events;

public class TutorialHandler : MonoBehaviour
{
    [SerializeField] private UnityEvent OnFirstLogin;
    [SerializeField] private UnityEvent OnNotFirstLogin;

#if UNITY_EDITOR
    [Header("Editor:")]
    [SerializeField]
    private bool _toIgnoreTutorial;
#endif

    private void Awake()
    {
        AccountManager.OnAccountDataAssigned += CheckIfTutorialFinished;
    }
    private void OnDestroy()
    {
        AccountManager.OnAccountDataAssigned -= CheckIfTutorialFinished;
        
    }
    private void CheckIfTutorialFinished()
    {
#if UNITY_EDITOR
        if (_toIgnoreTutorial)
        {
            OnNotFirstLogin?.Invoke(); 
            return;
        }
#endif

        if (!AccountManager.Instance.Data.AccountTutorialData.IsCompletedTutorial)
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
