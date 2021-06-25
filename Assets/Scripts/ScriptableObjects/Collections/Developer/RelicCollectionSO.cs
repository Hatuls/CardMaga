using UnityEngine;
namespace Collections.RelicsSO
{
    [CreateAssetMenu(fileName = "RelicCollections", menuName = "ScriptableObjects/Collections/RelicCollections")]
    public class RelicCollectionSO : ScriptableObject
    {
        #region Fields
        [Tooltip("List of all relics in game")]
        [SerializeField] Relics.RelicSO[] _allRelicsArr;
        #endregion

        #region properties
        public Relics.RelicSO[] GetRelicSO => _allRelicsArr;
        #endregion
    }
}


namespace Collections.Animations
{
    [CreateAssetMenu(fileName = "AnimationsCollection", menuName = "ScriptableObjects/Collections/Animations")]
  public class CardsModelAnimations : ScriptableObject 
    {
        public static CardsModelAnimations Instance;
        [Header("Animations: - Model")]
        [Tooltip("Model Animation, animation from cards")]

        [SerializeField]  AnimationClip[] _animationsArr;

        private void OnEnable()
        {
          //  Debug.Log("hi");
         //   _animationsArr = Resources.FindObjectsOfTypeAll<AnimationClip>();
            Instance = this;
        }
        public static AnimationClip[] GetAllAnimations
        {
            get => Instance._animationsArr;
        }

        public static bool IsAnimationInArray(AnimationClip animation)
        {

            if (animation != null || GetAllAnimations.Length == 0)
                return false;



            bool answer = false;
            for (int i = 0; i < GetAllAnimations.Length; i++)
            {
                if (ReferenceEquals(GetAllAnimations[i], animation))
                {
                    answer = true;

                    if (answer)
                        break;
                }
            }

            return answer;
        }
    }
}