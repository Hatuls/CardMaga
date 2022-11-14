using Battle;
using Battle.Characters;
using CardMaga.Battle.Players;
using CardMaga.Battle.UI;
using CardMaga.SequenceOperation;
using ReiTools.TokenMachine;
using UnityEngine;
namespace CardMaga.Battle.Visual
{
    public interface IVisualPlayer
    {
        IPlayer PlayerData { get; }
        AnimatorController AnimatorController { get; }
        AvatarHandler AvatarHandler { get; }
        Animator Animator { get; }
        AnimationBodyPartSoundsHandler AnimationSound { get; }
        VisualStatHandler VisualStats { get; }
        VFXController VfxController { get; }
    }

    public class VisualCharacter : MonoBehaviour, IVisualPlayer, ISequenceOperation<IBattleUIManager>
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
        private IPlayer _playerData;
        #endregion


        #region Properties
        public AvatarHandler AvatarHandler { get => _avatarHandler; private set => _avatarHandler = value; }
        public AnimatorController AnimatorController => _animatorController;
        public Animator Animator => _animator;
        public VFXController VfxController => _vfxController;
        public AnimationBodyPartSoundsHandler AnimationSound { get => _animationSound; }
        public bool IsLeft { get => _isLeft; private set => _isLeft = value; }
        public VisualStatHandler VisualStats { get => _visualStats; }

        public IPlayer PlayerData => _playerData;

        public int Priority => 0;

        #endregion

        public void InitVisuals(IPlayer player, CharacterSO characterSO, BattleCharacterVisual characterSkin)
        {
            //I want to remove this later
            _playerData = player;


            IsLeft = player.IsLeft;
            // Instantiate Model

            AvatarHandler = Instantiate(characterSkin.Model, _visual.position, Quaternion.identity, _visual);

            AvatarHandler.Mesh.material = characterSkin.Material;

            //Assign Avatar
            VfxController.AvatarHandler = AvatarHandler;
            Animator.avatar = AvatarHandler.Avatar;
            AnimationSound.CurrentCharacter = characterSO;

            //Sound
            AnimationSound.CurrentCharacter = characterSO;



#if UNITY_EDITOR
            DrawMesh = false;
#endif
        }

        internal void Dispose()
        {
            AnimatorController.BeforeDestroy(this);
            _visualStats.Dispose(this);
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

        public void ExecuteTask(ITokenReciever tokenMachine, IBattleUIManager battleUIManager)
        {
            var data = battleUIManager.BattleDataManager;
            //Visual Stats
            _visualStats = new VisualStatHandler(this);
            //AnimatorController
            AnimatorController.Init(this, data.EndBattleHandler);
        }

        #endregion
    }
}