using Sirenix.OdinInspector;
using UnityEngine;

namespace Art
{

    [CreateAssetMenu(fileName = "ART BLACKBOARD", menuName = "ScriptableObjects/ART/UI Card Blackboard")]
    public class ArtSO : ScriptableObject
    {
        [TitleGroup("Arts", "ArtSO", BoldTitle = true, Alignment = TitleAlignments.Centered)]

        #region Palette
        [TabGroup("Arts/Palette", "Palette")]

        [SerializeField] Palette[] _allPalette; 
 
        
        #endregion

        //[SerializeField]
        //[Tooltip("Color pallete for UI")]
        //UIColorPaletteSO _uiColorPalette;
        #region Sprites

        [TabGroup("Arts/Palette", "Sprites")]
        [Tooltip("Icon Collection for UI")]

        public  CardIconCollectionSO _iconCollection;



        #endregion
        [SerializeField]
        [Tooltip("Deafult Slot SO")]
        UIIconSO _defaultSlotSO;


        //public UIColorPaletteSO UIColorPalette => _uiColorPalette;
        public CardIconCollectionSO IconCollection => _iconCollection;
        public UIIconSO DefaultSlotSO => _defaultSlotSO;





        public  T GetPallette<T>() where T : Palette
        {
            if (typeof(T) == null)
                return null;

            for (int i = 0; i < _allPalette.Length; i++)
            {
                if (typeof(T) == _allPalette[i].GetType())
                    return _allPalette[i] as T;
            }

            return null;
        }
        public T GetSpriteCollections<T>() where T : CardIconCollectionSO
        {
            return _iconCollection as T;
        }
    }

}