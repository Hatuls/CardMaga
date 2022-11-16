using System;
using System.Collections.Generic;
using CardMaga.Tools.Pools;
using UnityEngine;

namespace CardMaga.UI.ScrollPanel
{
    public abstract class BaseScrollPanelManager<T_visual, T_data> : MonoBehaviour
        where T_visual : MonoBehaviour, IUIElement, IPoolableMB<T_visual>, IVisualAssign<T_data>, new()
    {
        public event Action<List<T_visual>> OnObjectLoaded;

        private BasePoolObjectVisualToData<T_visual, T_data> _objectVisualToDataPool;
        
        [SerializeField] private T_visual _objectPrefab;
        [SerializeField] private RectTransform _parent;
        [SerializeField] private ScrollPanelHandler _scrollPanel;
        
        public virtual void Init()
        {
            _objectVisualToDataPool = new BasePoolObjectVisualToData<T_visual, T_data>(_objectPrefab, _parent);
            _scrollPanel.Init();
        }

        public void AddObjectToPanel(List<T_data> data)
        {
            if (data.Count <= 0)
                return;

            List<T_visual> cache = _objectVisualToDataPool.PullObjects(data);

            IUIElement[] showableUis = new IUIElement[cache.Count];

            for (int i = 0; i < cache.Count; i++)
            {
                showableUis[i] = cache[i];
            }

            _scrollPanel.LoadObject(showableUis);

            OnObjectLoaded?.Invoke(cache);
        }

        public void RemoveAllObjectsFromPanel()
        {
            _scrollPanel.UnLoadAllObjects();
        }

        private void OnDestroy()
        {
            RemoveAllObjectsFromPanel();
        }
    }
}
