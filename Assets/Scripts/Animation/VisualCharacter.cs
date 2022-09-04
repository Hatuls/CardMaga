using UnityEngine;
#if UNITY_EDITOR
#endif
public class VisualCharacter : MonoBehaviour
{
    [SerializeField]
    private AnimatorController _animatorController;
    [SerializeField]
    private VFXController _vfxController;
    [SerializeField]
    private AnimationBodyPartSoundsHandler _animationSound;
    [Sirenix.OdinInspector.ShowInInspector,Sirenix.OdinInspector.ReadOnly]
    private AvatarHandler _avatarHandler;
    [Sirenix.OdinInspector.ShowInInspector,Sirenix.OdinInspector.ReadOnly]
    private Animator _characterAnimator;
    [SerializeField]
    private Transform _visual;
    public AvatarHandler AvatarHandler { get => _avatarHandler; private set => _avatarHandler = value; }
    public AnimatorController AnimatorController => _animatorController;
    public Animator CharacterAnimator { get => _characterAnimator;private set => _characterAnimator = value; }

    public VFXController VfxController => _vfxController;
    public AnimationBodyPartSoundsHandler AnimationSound { get => _animationSound; }

    public void SpawnModel(ModelSO modelSO, bool isTinted)
    {
        AvatarHandler = Instantiate(modelSO.Model, _visual.position, Quaternion.identity, _visual);
        if (isTinted)
            AvatarHandler.Mesh.material = modelSO.GetRandomTintedMaterials();
        VfxController.AvatarHandler = AvatarHandler;
        _characterAnimator = AvatarHandler.Animator;
        AnimatorController.Init(AvatarHandler);
        AnimatorController.ResetAnimator();

#if UNITY_EDITOR
        DrawMesh = false;
#endif
    }







    #region Editor
    [Header("Editor")]
    [SerializeField, Tooltip("The Mesh that will be seen in the scene view")]
    private Mesh _mesh;
    [SerializeField]
    private Color _gizmoColor;
    [SerializeField]
    private Vector3 _meshScale = Vector3.one;
    [SerializeField]
    private Vector3 _rotation = Vector3.one;
    public bool DrawMesh;


    private void OnDrawGizmos()
    {
        if (DrawMesh)
        {
            Gizmos.color = _gizmoColor;
            Gizmos.DrawWireMesh(_mesh, _visual.position, Quaternion.Euler(_rotation), _meshScale);
        }
    }
    #endregion
}