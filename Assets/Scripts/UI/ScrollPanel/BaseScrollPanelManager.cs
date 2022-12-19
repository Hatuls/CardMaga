using System.Collections.Generic;
using CardMaga.ObjectPool;
using CardMaga.Tools.Pools;
using UnityEngine;

namespace CardMaga.UI.ScrollPanel
{
    public abstract class BaseScrollPanelManager<TVisual> : MonoBehaviour
        where TVisual :  BaseUIElement, IPoolableMB<TVisual>, new()
    {
        [SerializeField] private ScrollPanelHandler _scrollPanel;
        
        public IReadOnlyList<IUIElement> LoadObjects => _scrollPanel.LoadedObjects;

        public virtual void Init()
        {
            _scrollPanel.Init();
        }
        
        public void AddObjectToPanel(List<IUIElement> visuals)
        {
            IUIElement[] uiElements = new IUIElement[visuals.Count];

            Transform holder = _scrollPanel.Holder;
            
            for (int i = 0; i < visuals.Count; i++)
            {
                visuals[i].RectTransform.SetParent(holder);
                visuals[i].RectTransform.localScale = Vector3.one;
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
