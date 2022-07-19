using ReiTools.TokenMachine;

namespace Managers
{
    public class CardManager : MonoSingleton<CardManager>
    {
        #region Fields

        #endregion
        #region MonoBehaviour Callbacks
        public override void Awake()
        {
            base.Awake();
            SceneHandler.OnBeforeSceneShown += Init;
        }



        public void OnDestroy()
        {
            SceneHandler.OnBeforeSceneShown -= Init;
        }
        #endregion

        public override void Init(ITokenReciever token)
        {
            CardManager.Instance.ResetCards();
        }

        internal void ResetCards()
        {

        }



    }
}
