using UnityEngine;

public class AvatarHandler : MonoBehaviour
{
    [SerializeField] Transform _headPart;
    [SerializeField] Transform _leftHandPart;
    [SerializeField] Transform _rightHandPart;
    [SerializeField] Transform _leftLegPart;
    [SerializeField] Transform _rightLegPart;
    [SerializeField] Transform _bottomBody;
    [SerializeField] Transform _chestPart;
    [SerializeField] Avatar avatar;
    public Transform HeadPart { get => _headPart; }
    public Transform LeftHandPart { get => _leftHandPart; }
    public Transform RightHandPart { get => _rightHandPart; }
    public Transform LeftLegPart { get => _leftLegPart; }
    public Transform RightLegPart { get => _rightLegPart; }
    public Transform ChestPart { get => _chestPart; }
    public Transform BottomBody { get => _bottomBody; }
    public Avatar Avatar { get => avatar; }
    private void Start()
    {
        if (transform.root.TryGetComponent(out Animator animator))
        {
            animator.avatar = avatar;
        }
        else
            throw new System.Exception($"AvatarHandler : Could Not Find An Animator");
       
    }
    public Transform GetBodyPart(BodyPartEnum bodyPartEnum)
    {

        Transform transformOfBodyPart = null;
        switch (bodyPartEnum)
        {
            case BodyPartEnum.RightArm:
                transformOfBodyPart = _rightHandPart;
                break;
            case BodyPartEnum.LeftArm:
                transformOfBodyPart = _leftHandPart;
                break;
            case BodyPartEnum.Head:
                transformOfBodyPart = _headPart;
                break;
            case BodyPartEnum.LeftLeg:
                transformOfBodyPart = _leftLegPart;
                break;
            case BodyPartEnum.RightLeg:
                transformOfBodyPart = _rightLegPart;
                break;
            case BodyPartEnum.BottomBody:
                transformOfBodyPart = _bottomBody;
                break;
            case BodyPartEnum.Chest:
                transformOfBodyPart = _chestPart;
                break;

        }
        if (transformOfBodyPart == null)
            throw new System.Exception($"AvatarHandler:  Body Part is not valid or null {bodyPartEnum}");
        return transformOfBodyPart;
    }
}
