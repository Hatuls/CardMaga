using Battle;
using Battle.Turns;
using CardMaga.Card;
using Keywords;
using CardMaga.SequenceOperation;
using ReiTools.TokenMachine;
using System.Collections.Generic;
using UnityEngine;
using CardMaga.Battle.UI;
using CardMaga.Keywords;

//Need REMAKE
public class VFXController : MonoBehaviour
{

    [SerializeField] bool _isPlayer;
    [SerializeField] VFXSO _defensingVFX;
    [SerializeField] VFXSO _recieivingDamageVFX;
     private AvatarHandler _avatarHandler;

    public AvatarHandler AvatarHandler { get => _avatarHandler; set => _avatarHandler = value; }

    public int Priority => 99;

    List<KeywordData> keywordDatas = new List<KeywordData>();

    private Queue<VFXSO> VFXQueue = new Queue<VFXSO>();

    private TurnHandler _turnHandler;



    //public void ExecuteTask(ITokenReciever tokenMachine, IBattleManager data)
    //{
    //    _turnHandler = data.TurnHandler;
    //    data.OnBattleManagerDestroyed += BeforeExitGame;

    //}


    public void RegisterVFXQueue(KeywordSO keyword)
    {
        var vfx = keyword.GetVFX();
        if (vfx)
            VFXQueue.Enqueue(vfx);
    }

    private VFXSO DeQueue() => VFXQueue.Dequeue();

    private void ExecuteAllKeywords(BattleCardData battleCard)
    {

        while (VFXQueue.Count > 0)
        {
            CreateVFX(VFXQueue.Peek().DefaultBodyPart);
        };
    }
    private void RecieveSortingKeywordsData(IReadOnlyList<KeywordData> keywords)
    {
        if (_isPlayer == _turnHandler.IsLeftCharacterTurn)
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
    private void CreateVFXFromAnimation(BodyPartEnum bodyPartEnum)
    {
        if (VFXQueue.Count == 0)
            return;
        if (VFXQueue.Peek().IsFromAnimation)
            CreateVFX(bodyPartEnum);
        else
            VFXQueue.Dequeue();
    }
    private void CreateVFX(BodyPartEnum bodyPartEnum)
    {

        if (VFXQueue.Count > 0)
        {
            var vfx = DeQueue();

            ActivateParticle(_avatarHandler.GetBodyPart(bodyPartEnum), vfx);
        }
    }

    private void ActivateParticle(Transform transform, VFXSO vfx)
    { return; } //VFXManager.Instance.RecieveParticleSystemVFX(transform, vfx).Item1.StartVFX(vfx,transform);
    #region Animation Event Callbacks
    #region Attack
    public void ApplyAttackHeadVFX()
    {
        CreateVFXFromAnimation(BodyPartEnum.Head);
    }
    public void ApplyAttackPivotBottomVFX()
    {
        CreateVFXFromAnimation(BodyPartEnum.BottomBody);
    }
    public void ApplyAttackChestVFX()
    {
        CreateVFXFromAnimation(BodyPartEnum.Chest);
    }
    public void ApplyAttackLeftLegVFX()
    {
        CreateVFXFromAnimation(BodyPartEnum.LeftLeg);
    }
    public void ApplyAttackRightLegVFX()
    {
        CreateVFXFromAnimation(BodyPartEnum.RightLeg);
    }
    public void ApplyAttackLeftArmVFX()
    {
        CreateVFXFromAnimation(BodyPartEnum.LeftArm);
    }
    public void ApplyAttackRightArmVFX()
    {
        CreateVFXFromAnimation(BodyPartEnum.RightArm);
    }
    public void ApplyRightKneeArmVFX()
    { CreateVFXFromAnimation(BodyPartEnum.RightKnee); }
    public void ApplyLeftKneeArmVFX()
    { CreateVFXFromAnimation(BodyPartEnum.LeftKnee); }
    public void ApplyRightElbowArmVFX()
    { CreateVFXFromAnimation(BodyPartEnum.RightElbow); }
    public void ApplyLeftElbowArmVFX()
    { CreateVFXFromAnimation(BodyPartEnum.LeftElbow); }

   
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

