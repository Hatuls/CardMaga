using System.Collections.Generic;
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
    [SerializeField] Transform _pivot;
    [SerializeField] Transform _chestPart;
    [SerializeField] Transform _hipsPart;

    private Transform _currentActiveBodyPart;

    private Dictionary<BodyPartEnum, Transform> _bodypartDictionary = new Dictionary<BodyPartEnum, Transform>();

    [Header("Avatar:")]
    [SerializeField] Avatar avatar;
    [SerializeField] SkinnedMeshRenderer _mesh;





    public SkinnedMeshRenderer Mesh { get => _mesh; }
    public Avatar Avatar { get => avatar; }

    public Transform GetCurrentActiveBodyPart() => _currentActiveBodyPart;
    public void UpdateCurrentActiveBodyPart(BodyPartEnum bodyPartEnum)
    => _currentActiveBodyPart = GetBodyPart(bodyPartEnum);



    private void Awake()
    {
        _bodypartDictionary.Add(BodyPartEnum.LeftArm, _leftHandPart);
        _bodypartDictionary.Add(BodyPartEnum.RightArm, _rightHandPart);
        _bodypartDictionary.Add(BodyPartEnum.Head, _headPart);
        _bodypartDictionary.Add(BodyPartEnum.LeftLeg, _leftLegPart);
        _bodypartDictionary.Add(BodyPartEnum.RightLeg, _rightLegPart);
        _bodypartDictionary.Add(BodyPartEnum.Pivot, _pivot);
        _bodypartDictionary.Add(BodyPartEnum.Chest, _chestPart);
        _bodypartDictionary.Add(BodyPartEnum.LeftKnee, _leftKneePart);
        _bodypartDictionary.Add(BodyPartEnum.RightKnee, _rightKneePart);
        _bodypartDictionary.Add(BodyPartEnum.LeftElbow, _leftElbowPart);
        _bodypartDictionary.Add(BodyPartEnum.RightElbow, _rightElbowPart);
        _bodypartDictionary.Add(BodyPartEnum.Hips, _hipsPart);
    }

    public Transform GetBodyPart(BodyPartEnum bodyPartEnum)
    {

        if (_bodypartDictionary.TryGetValue(bodyPartEnum, out Transform bodypart))
            return bodypart;
        else
            throw new System.Exception($"AvatarHandler:  Body Part is not valid or null {bodyPartEnum}");
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
        _hipsPart =      FindBodyPart(transform,"Hips");
        _chestPart= FindBodyPart(transform,"Spine2");
        _pivot = FindBodyPart(transform, "Pivot");
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