using UnityEngine;

namespace UI.LoadingScreen
{
    public class LoadingSceneManager : MonoBehaviour
    {
        #region Singleton
        static LoadingSceneManager _instance;
        public static LoadingSceneManager Instance
        {
            get
            {
                if (_instance == null)
                    Debug.Log("LoadingSceneManagerIsNull");

                return _instance;
            }
        }
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
        #endregion

        #region Fields
        [SerializeField]
        SceneTransitionSO[] _sceneTransitions;
        [SerializeField]
        GameObject[] _objectHolders;
        #endregion
        public void StartTransition(SceneHandler.ScenesEnum fromScene, SceneHandler.ScenesEnum toScene)
        {
            //for (int i = 0; i < _objectHolders.Length; i++)
            //{
            //    _objectHolders[i].SetActive(false);
            //}

            for (int i = 0; i < _sceneTransitions.Length; i++)
            {
                if(_sceneTransitions[i].FromScene == fromScene && _sceneTransitions[i].ToScene == toScene)
                {
                    _sceneTransitions[i].InvokeTransition();
                    return;
                }
            }
            throw new System.Exception("LoadingSceneManager No Correct Transition Was Found");
        }

        public void ScreenTransitionCompleted()
        {
            //SceneHandler.Instance.
        }
    }
}
