using CardMaga.Battle.Players;
using CardMaga.ObjectPool;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace CardMaga.UI.PopUp
{

    public class PopUpManager : MonoSingleton<PopUpManager>, ITaggable
    {
        #region Pre-arranged screen locations
        [SerializeField] private PopUpScreenLocation[] _screenLocation;

        private PoolHandler<BasePoolSO<PopUp>, PopUp> _poolHandler;

        public override void Awake()
        {
            base.Awake();
            _poolHandler = new PoolHandler<BasePoolSO<PopUp>, PopUp>(transform);
        }
        public IPool<BasePoolSO<PopUp>, PopUp> PopUpPool => _poolHandler;

        public IEnumerable<TagSO> Tags
        {
            get
            {
                for (int i = 0; i < _screenLocation.Length; i++)
                    foreach (TagSO tag in _screenLocation[i].Tags)
                        yield return tag;
            }
        }

        public Vector2 GetPosition(TagSO tagSO)
        {
            for (int i = 0; i < _screenLocation.Length; i++)
            {
                if (_screenLocation[i].ContainTag(tagSO))
                    return _screenLocation[i].Location;
            }

            throw new Exception($"Popup Manager: Tag was not found\nPlease check if PopupManager has a location with this tag");
        }

        #endregion

    }
    [Serializable]
    public class PopUpScreenLocation : ITaggable
    {
        [SerializeField]
        private TagSO _tag;
        [SerializeField]
        private RectTransform _rectTransform;

        public Vector2 Location => _rectTransform.position;

        public IEnumerable<TagSO> Tags { get { yield return _tag; } }
    }
}