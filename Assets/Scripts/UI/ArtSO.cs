
using UnityEngine;

[CreateAssetMenu(fileName = "ART BLACKBOARD" , menuName = "ScriptableObjects/ART/UI Card Blackboard")]
public class ArtSO : ScriptableObject
{ 

    [SerializeField]
    [Tooltip("Color pallete for UI")]
    UIColorPaletteSO _uiColorPalette;

    [SerializeField]
    [Tooltip("Icon Collection for UI")]
    CardIconCollectionSO _iconCollection;

    [SerializeField]
    [Tooltip("Deafult Slot SO")]
    UIIconSO _defaultSlotSO;

    [SerializeField]
 

    public   UIColorPaletteSO UIColorPalette =>  _uiColorPalette;
    public   CardIconCollectionSO IconCollection =>  _iconCollection;
    public   UIIconSO DefaultSlotSO =>  _defaultSlotSO;

}
