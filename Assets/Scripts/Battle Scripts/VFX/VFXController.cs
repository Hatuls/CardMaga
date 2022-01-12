using Keywords;
using System.Collections.Generic;
using UnityEngine;
public class VFXController : MonoBehaviour
{

    [SerializeField] bool _isPlayer;
    [SerializeField] VFXSO _defensingVFX;
    [SerializeField] VFXSO _recieivingDamageVFX;

    [SerializeField] AvatarHandler _avatarHandler;
    public AvatarHandler AvatarHandler { get => _avatarHandler; set => _avatarHandler = value; }
    List<KeywordData> keywordDatas = new List<KeywordData>();

    private Queue<VFXSO> VFXQueue = new Queue<VFXSO>();
    private void Start()
    {
        Battles.CardExecutionManager.OnSortingKeywords += RecieveSortingKeywordsData;
        Battles.CardExecutionManager.OnAnimationIndexChange += SortVFXFromListByAnimationIndex;
    }
    private void OnDisable()
    {
        Battles.CardExecutionManager.OnAnimationIndexChange -= SortVFXFromListByAnimationIndex;
        Battles.CardExecutionManager.OnSortingKeywords -= RecieveSortingKeywordsData;
    }
    public void RegisterVFXQueue(KeywordSO keyword)
        => VFXQueue.Enqueue(keyword.GetVFX());

    private VFXSO DeQueue() => VFXQueue.Dequeue();

    public void ActivateParticle(BodyPartEnum bodyPart, ParticleSystemVFX vfx)
        => ActivateParticle(_avatarHandler.GetBodyPart(bodyPart), vfx.VFXID);
    public void ActivateParticle(Transform part, VFXSO vfx)
    {
        Instantiate(vfx.VFXPrefab).GetComponent<VFXBase>().StartVFX(vfx, part);
    }

    private void RecieveSortingKeywordsData(List<KeywordData> keywords)
    {
        if (_isPlayer == Battles.Turns.TurnHandler.IsPlayerTurn)
        {
            keywordDatas.AddRange(keywords);

        }
    }
    private void SortVFXFromListByAnimationIndex(int index)
    {
        for (int i = 0; i < keywordDatas.Count; i++)
        {
            if (keywordDatas[i].AnimationIndex == index)
            {
                RegisterVFXQueue(keywordDatas[i].KeywordSO);
                keywordDatas.RemoveAt(i);
                i--;
            }
        }

    }

    #region Animation Event Callbacks
    #region Attack
    public void ApplyAttackHeadVFX()
        => CreateVFX(BodyPartEnum.Head);
    public void ApplyAttackPivotBottomVFX()
        => CreateVFX(BodyPartEnum.BottomBody);
    public void ApplyAttackChestVFX()
        => CreateVFX(BodyPartEnum.Chest);
    public void ApplyAttackLeftLegVFX()
        => CreateVFX(BodyPartEnum.LeftLeg);
    public void ApplyAttackRightLegVFX()
        => CreateVFX(BodyPartEnum.RightLeg);
    public void ApplyAttackLeftArmVFX()
        => CreateVFX(BodyPartEnum.LeftArm);
    public void ApplyAttackRightArmVFX()
        => CreateVFX(BodyPartEnum.RightArm);
    public void ApplyRightKneeArmVFX()
     => CreateVFX(BodyPartEnum.RightKnee);
    public void ApplyLeftKneeArmVFX()
       => CreateVFX(BodyPartEnum.LeftKnee);
    public void ApplyRightElbowArmVFX()
 => CreateVFX(BodyPartEnum.RightElbow);
    public void ApplyLeftElbowArmVFX()
       => CreateVFX(BodyPartEnum.LeftElbow);

    private void CreateVFX(BodyPartEnum bodyPartEnum)
    {
        if (VFXQueue.Count > 0)
            VFXManager.Instance.PlayParticle(_avatarHandler.GetBodyPart(bodyPartEnum), DeQueue());
    }
    #endregion

    #region Defense

    public void ApplyDefenseHeadVFX()
     => ActivateParticle(_avatarHandler.GetBodyPart(BodyPartEnum.Head), _defensingVFX);
    public void ApplyDefensePivotBottomVFX()
        => ActivateParticle(_avatarHandler.GetBodyPart(BodyPartEnum.BottomBody), _defensingVFX);
    public void ApplyDefenseChestVFX()
        => ActivateParticle(_avatarHandler.GetBodyPart(BodyPartEnum.Chest), _defensingVFX);
    public void ApplyDefenseLeftLegVFX()
         => ActivateParticle(_avatarHandler.GetBodyPart(BodyPartEnum.LeftLeg), _defensingVFX);
    public void ApplyDefenseRightLegVFX()
          => ActivateParticle(_avatarHandler.GetBodyPart(BodyPartEnum.RightLeg), _defensingVFX);
    public void ApplyDefenseLeftArmVFX()
       => ActivateParticle(_avatarHandler.GetBodyPart(BodyPartEnum.LeftArm), _defensingVFX);
    public void ApplyDefenseRightArmVFX()
          => ActivateParticle(_avatarHandler.GetBodyPart(BodyPartEnum.RightArm), _defensingVFX);

    public void ApplyDefenseRightKneeArmVFX()
 => ActivateParticle(_avatarHandler.GetBodyPart(BodyPartEnum.RightKnee), _defensingVFX);
    public void ApplyDefenseLeftKneeArmVFX()
=> ActivateParticle(_avatarHandler.GetBodyPart(BodyPartEnum.LeftKnee), _defensingVFX);
    public void ApplyDefenseRightElbowArmVFX()
=> ActivateParticle(_avatarHandler.GetBodyPart(BodyPartEnum.RightElbow), _defensingVFX);
    public void ApplyDefenseLeftElbowArmVFX()
=> ActivateParticle(_avatarHandler.GetBodyPart(BodyPartEnum.LeftElbow), _defensingVFX);
    #endregion

    #region Hit VFX 
    public void ApplyRecieveDamageHeadVFX()
 => ActivateParticle(_avatarHandler.GetBodyPart(BodyPartEnum.Head), _recieivingDamageVFX);
    public void ApplyRecieveDamagePivotBottomVFX()
        => ActivateParticle(_avatarHandler.GetBodyPart(BodyPartEnum.BottomBody), _recieivingDamageVFX);
    public void ApplyRecieveDamageChestVFX()
        => ActivateParticle(_avatarHandler.GetBodyPart(BodyPartEnum.Chest), _recieivingDamageVFX);
    public void ApplyRecieveDamageLeftLegVFX()
         => ActivateParticle(_avatarHandler.GetBodyPart(BodyPartEnum.LeftLeg), _recieivingDamageVFX);
    public void ApplyRecieveDamageRightLegVFX()
          => ActivateParticle(_avatarHandler.GetBodyPart(BodyPartEnum.RightLeg), _recieivingDamageVFX);
    public void ApplyRecieveDamageLeftArmVFX()
       => ActivateParticle(_avatarHandler.GetBodyPart(BodyPartEnum.LeftArm), _recieivingDamageVFX);
    public void ApplyRecieveDamageRightArmVFX()
          => ActivateParticle(_avatarHandler.GetBodyPart(BodyPartEnum.RightArm), _recieivingDamageVFX);


    public void ApplyRecieveDamageRightKneeArmVFX()
 => ActivateParticle(_avatarHandler.GetBodyPart(BodyPartEnum.RightKnee), _recieivingDamageVFX);
    public void ApplyRecieveDamageLeftKneeArmVFX()
=> ActivateParticle(_avatarHandler.GetBodyPart(BodyPartEnum.LeftKnee), _recieivingDamageVFX);
    public void ApplyRecieveDamageRightElbowArmVFX()
=> ActivateParticle(_avatarHandler.GetBodyPart(BodyPartEnum.RightElbow), _recieivingDamageVFX);
    public void ApplyRecieveDamageLeftElbowArmVFX()
=> ActivateParticle(_avatarHandler.GetBodyPart(BodyPartEnum.LeftElbow), _recieivingDamageVFX);
    #endregion
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
    LeftKnee = 8,
    RightKnee = 9,
    LeftElbow = 10,
    RightElbow = 11,
};

