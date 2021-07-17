
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

    public   UIColorPaletteSO UIColorPalette =>  _uiColorPalette;
    public   CardIconCollectionSO IconCollection =>  _iconCollection;
    public   UIIconSO DefaultSlotSO =>  _defaultSlotSO;


}

public class EnemyIcons : ScriptableObject
{
    [SerializeField]
    private Sprite _backgroundImage;

    [SerializeField]
    private Sprite _decorateImage;


    public Sprite DecorateImage => _decorateImage;
    public Sprite BackGroundImage => _backgroundImage;
}