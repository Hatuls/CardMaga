using UnityEngine;

public class VFXController : MonoBehaviour
{

    [SerializeField] bool _isPlayer;


    [SerializeField] AvatarHandler _avatarHandler;
    public AvatarHandler AvatarHandler { set => _avatarHandler = value; }


    #region Animation Event Callbacks
    public void ApplyHeadVFX()
        => CreateVFX(BodyPartEnum.Head);
    public void ApplyPivotBottomVFX()
        => CreateVFX(BodyPartEnum.BottomBody);
    public void ApplyChestVFX()
        => CreateVFX(BodyPartEnum.Chest);
    public void ApplyLeftLegVFX()
        => CreateVFX(BodyPartEnum.LeftLeg);
    public void ApplyRightLegVFX()
        => CreateVFX(BodyPartEnum.RightLeg);
    public void ApplyLeftArmVFX()
        => CreateVFX(BodyPartEnum.LeftArm);
    public void ApplyRightArmVFX()
        => CreateVFX(BodyPartEnum.RightArm);

    private void CreateVFX(BodyPartEnum bodyPartEnum,VFXSO vfx)
    {
        VFXManager.Instance.PlayParticle(_isPlayer, bodyPartEnum,);
    }
    #endregion

}

public enum BodyPartEnum
{
    None = 0,
    RightArm = 1,
    LeftArm = 2,
    Head = 3,
    LeftLeg = 4,
    RightLeg = 5,
    BottomBody = 6,
    Chest = 7,
    Joker = 8,
};

