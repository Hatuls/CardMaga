using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New ScreenTransition", menuName = "ScriptableObjects/ScreenTransition")]
public class SceneTransitionSO : ScriptableObject
{
    [SerializeField]
    UnityEvent _onTransition;
    #region Fields
    [SerializeField]
    SceneHandler.ScenesEnum _fromScene;
    [SerializeField]
    SceneHandler.ScenesEnum _toScene;
    #endregion
    #region Properties
    public SceneHandler.ScenesEnum FromScene => _fromScene;
    public SceneHandler.ScenesEnum ToScene => _toScene;
    #endregion
    public void InvokeTransition()
    {
        _onTransition.Invoke();
    }
}
