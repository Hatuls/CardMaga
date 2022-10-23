using Battle;
using Managers;
using UnityEngine;
namespace CardMaga.Battle.Visual
{
    public class VisualCharacter : MonoBehaviour
    {
        #region Fields
        [SerializeField]
        private AnimatorController _animatorController;
        [SerializeField]
        private VFXController _vfxController;
        [SerializeField]
        private AnimationBodyPartSoundsHandler _animationSound;
        [Sirenix.OdinInspector.ShowInInspector, Sirenix.OdinInspector.ReadOnly]
        private AvatarHandler _avatarHandler;
        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private Transform _visual;
        private bool _isLeft;
        private VisualStatHandler _visualStats;
        #endregion


        #region Properties
        public AvatarHandler AvatarHandler { get => _avatarHandler; private set => _avatarHandler = value; }
        public AnimatorController AnimatorController => _animatorController;
        public Animator Animator => _animator;
        public VFXController VfxController => _vfxController;
        public AnimationBodyPartSoundsHandler AnimationSound { get => _animationSound; }
        public bool IsLeft { get => _isLeft; private set => _isLeft = value; }
        public VisualStatHandler VisualStats { get => _visualStats; }
        #endregion

        public void InitVisuals(IPlayer player, CharacterSO characterSO, bool isTinted)
        {
            IsLeft = player.IsLeft;
            AnimatorController.Init(this, player);

            // Instantiate Model
            ModelSO modelSO = characterSO.CharacterAvatar;
            AvatarHandler = Instantiate(modelSO.Model, _visual.position, Quaternion.identity, _visual);
            if (isTinted)
                AvatarHandler.Mesh.material = modelSO.GetRandomTintedMaterials();


            VfxController.AvatarHandler = AvatarHandler;
            Animator.avatar = AvatarHandler.Avatar;
            AnimationSound.CurrentCharacter = characterSO;

            //Visual Stats
            _visualStats = new VisualStatHandler(player);

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
}