using UnityEngine;

public class AvatarHandler : MonoBehaviour
{
    [Header("Body Parts:")]
    [SerializeField] Transform _headPart;
    [SerializeField] Transform _leftHandPart;
    [SerializeField] Transform _rightHandPart;
    [SerializeField] Transform _leftElbowPart;
    [SerializeField] Transform _rightElbowPart;
    [SerializeField] Transform _leftLegPart;
    [SerializeField] Transform _rightLegPart;
    [SerializeField] Transform _leftKneePart;
    [SerializeField] Transform _rightKneePart;
    [SerializeField] Transform _bottomBody;
    [SerializeField] Transform _chestPart;

    private Transform _currentActiveBodyPart;

    [Header("Avatar:")]
    [SerializeField] Avatar avatar;
    [SerializeField] SkinnedMeshRenderer _mesh;


    public Transform HeadPart { get => _headPart; }
    public Transform LeftHandPart { get => _leftHandPart; }
    public Transform RightHandPart { get => _rightHandPart; }
    public Transform LeftLegPart { get => _leftLegPart; }
    public Transform RightLegPart { get => _rightLegPart; }
    public Transform ChestPart { get => _chestPart; }
    public Transform BottomBody { get => _bottomBody; }
    public Transform LeftElbowPart { get => _leftElbowPart; }
    public Transform RightElbowPart { get => _rightElbowPart; }
    public Transform LeftKneePart { get => _leftKneePart; }
    public Transform RightKneePart { get => _rightKneePart; }


    public SkinnedMeshRenderer Mesh { get => _mesh; }
    public Avatar Avatar { get => avatar; }

    public Transform GetCurrentActiveBodyPart() => _currentActiveBodyPart;
    public void UpdateCurrentActiveBodyPart(BodyPartEnum bodyPartEnum)
    => _currentActiveBodyPart = GetBodyPart(bodyPartEnum);
    

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
            case BodyPartEnum.LeftKnee:
                transformOfBodyPart = _leftKneePart;
                break;
            case BodyPartEnum.RightKnee:
                transformOfBodyPart = _rightKneePart;
                break;
            case BodyPartEnum.LeftElbow:
                transformOfBodyPart = _leftElbowPart;
                break;
            case BodyPartEnum.RightElbow:
                transformOfBodyPart = _rightElbowPart;
                break;
        }

        if (transformOfBodyPart == null)
            throw new System.Exception($"AvatarHandler:  Body Part is not valid or null {bodyPartEnum}");
        return transformOfBodyPart;
    }

#if UNITY_EDITOR
    [Sirenix.OdinInspector.Button]
    private void AssignSkinnedMesh() { _mesh = transform.GetComponentInChildren<SkinnedMeshRenderer>(); }
    [Sirenix.OdinInspector.InfoBox("Only works for mixamo rigged models")]
    [Sirenix.OdinInspector.Button]
    private void TryAssignBodyParts()
    {
        _headPart = FindBodyPart(transform,"HeadTop_End");
        _leftHandPart = FindBodyPart(transform,"LeftHandMiddle1");
        _rightHandPart= FindBodyPart(transform,"RightHandMiddle1");
        _leftElbowPart = FindBodyPart(transform,"LeftForeArm");
        _rightElbowPart=  FindBodyPart(transform,"RightForeArm");
        _leftLegPart=     FindBodyPart(transform,"LeftToe_End");
        _rightLegPart=    FindBodyPart(transform,"RightToe_End");
        _leftKneePart=    FindBodyPart(transform,"LeftLeg");
        _rightKneePart=   FindBodyPart(transform,"RightLeg");
        _bottomBody=      FindBodyPart(transform,"Hips");
        _chestPart= FindBodyPart(transform,"Spine2");

    }
    private Transform FindBodyPart(Transform from ,string name)
    {
        if (from.name.Contains(name))
            return from;
        Transform result = null;
        for (int i = 0; i < from.childCount; i++)
        {
           result = FindBodyPart(from.GetChild(i), name);
            if (result != null) 
                break;
        }
    
        return result;
    
    }
#endif
}