using Battle;
using Battle.Turns;
using Managers;
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
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private Transform _visual;
    private bool _isLeft;
    public AvatarHandler AvatarHandler { get => _avatarHandler; private set => _avatarHandler = value; }
    public AnimatorController AnimatorController => _animatorController;
    public Animator Animator => _animator;
    public VFXController VfxController => _vfxController;
    public AnimationBodyPartSoundsHandler AnimationSound { get => _animationSound; }
    public bool IsLeft { get => _isLeft;private set => _isLeft = value; }

    public void InitVisuals(IPlayer player,CharacterSO characterSO, bool isTinted, GameTurn gameTurn)
    {
        IsLeft = player.IsLeft;

        ModelSO modelSO = characterSO.CharacterAvatar;
        AvatarHandler = Instantiate(modelSO.Model, _visual.position, Quaternion.identity, _visual);
        if (isTinted)
            AvatarHandler.Mesh.material = modelSO.GetRandomTintedMaterials();


        VfxController.AvatarHandler = AvatarHandler;
        Animator.avatar = AvatarHandler.Avatar;
        AnimatorController.Init(this, gameTurn);
        AnimationSound.CurrentCharacter = characterSO;

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