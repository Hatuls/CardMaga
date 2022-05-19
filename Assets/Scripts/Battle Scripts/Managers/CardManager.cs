

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
            BattleSceneManager.OnBattleSceneLoaded += Init;
        }

      

        public void OnDestroy()
        {
            BattleSceneManager.OnBattleSceneLoaded -= Init;
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
