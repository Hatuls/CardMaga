using CardMaga.LoadingScene;
using UnityEngine;
namespace CardMaga.Managers.GameManager
{

    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private LoadingSceneManager _loader;


        private void Start()
        {
            _loader.UnLoadAndThenLoad(null, 1);
        }
    }
}
