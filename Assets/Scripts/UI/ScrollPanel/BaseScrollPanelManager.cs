using System.Collections.Generic;
using CardMaga.ObjectPool;
using CardMaga.Tools.Pools;
using UnityEngine;

namespace CardMaga.UI.ScrollPanel
{
    public abstract class BaseScrollPanelManager<TVisual, TData> : MonoBehaviour
        where TVisual :  BaseUIElement, IPoolableMB<TVisual>, IVisualAssign<TData>, new()
    {
        [SerializeField] private TVisual _objectRef;
        [SerializeField] private ScrollPanelHandler _scrollPanel;

        private VisualRequester<TVisual, TData> _visualRequester;

        public virtual void Init()
        {
            _visualRequester = new VisualRequester<TVisual, TData>(_objectRef);
            _scrollPanel.Init();
        }

        public void AddObjectToPanel(List<TData> data)
        {
            List<TVisual> visuals = _visualRequester.GetVisual(data);
            
            IUIElement[] uiElements = new IUIElement[visuals.Count];

            Transform holder = _scrollPanel.Holder;
            
            for (int i = 0; i < visuals.Count; i++)
            {
                visuals[i].transform.SetParent(holder);
                visuals[i].transform.localScale = Vector3.one;
                uiElements[i] = visuals[i];
            }

            _scrollPanel.LoadObject(uiElements);
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
