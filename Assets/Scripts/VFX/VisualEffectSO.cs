using CardMaga.Battle.Players;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
namespace CardMaga.VFX
{

    [CreateAssetMenu(fileName = "New VFX SO", menuName = "ScriptableObjects/VFX/Generic VFX SO")]
    public class VisualEffectSO : TagSO//, ITaggable
    {
        [PreviewField(100f)]
        [SerializeField] 
        private BaseVisualEffect _vfxPrefab;
        //[SerializeField]
        //    private TagSO[] _tagSOs;



        public BaseVisualEffect VFXPrefab => _vfxPrefab;
      //  public IReadOnlyList<TagSO> Tags => _tagSOs;
    }


}