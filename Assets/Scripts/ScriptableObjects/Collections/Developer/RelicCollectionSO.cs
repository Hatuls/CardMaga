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
