using UnityEngine;
using UnityEngine.Events;

public class VictoryAndDefeatHandler : MonoBehaviour
{
    [SerializeField] private UnityEvent OnLeftPlayerWin;
    [SerializeField] private UnityEvent OnRightPlayerWin;
 
    public void OpenScreen(bool isLeftPlayerWon)
    {
        if (isLeftPlayerWon)
        {
            OnLeftPlayerWin?.Invoke();
        }
        else
        {
            OnRightPlayerWin?.Invoke();
        }
    }
}
