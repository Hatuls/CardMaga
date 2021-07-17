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
        [ShowInInspector]
        public static  Palette[] _allPalette;
        [TabGroup("Arts/Palette", "Palette")]
        [Button("Load Resources"), GUIColor(0, 1f, 0)]
        private void LoadResources()
        {
            _allPalette = null;

            _allPalette = Resources.LoadAll<Palette>("Art/Palette"); 
        }

        #endregion

        [SerializeField]
        [Tooltip("Color pallete for UI")]
        UIColorPaletteSO _uiColorPalette;

        [SerializeField]
        [Tooltip("Icon Collection for UI")]
        CardIconCollectionSO _iconCollection;

        [SerializeField]
        [Tooltip("Deafult Slot SO")]
        UIIconSO _defaultSlotSO;


        public UIColorPaletteSO UIColorPalette => _uiColorPalette;
        public CardIconCollectionSO IconCollection => _iconCollection;
        public UIIconSO DefaultSlotSO => _defaultSlotSO;





        public static T GetPallette<T>() where T : Palette
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

    }

}