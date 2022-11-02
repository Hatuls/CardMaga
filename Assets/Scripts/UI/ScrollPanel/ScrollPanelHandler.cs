using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardMaga.UI.ScrollPanel
{
    public class ScrollPanelHandler : MonoBehaviour
    {
        #region Fields
    
        private List<IShowableUI> _loadedObjects;
    
        #endregion

        public List<IShowableUI> LoadedObject
        {
            get => _loadedObjects;
        }

        public void Init()
        {
            _loadedObjects = new List<IShowableUI>();
        }

        #region PublicFunction
    
        internal void LoadObject(params IShowableUI[] objects)
        {
            foreach (var obj in objects)
            {
                _loadedObjects.Add(obj);
                obj.Show();
            }
        }
    
        internal void UnLoadAllObjects()
        {
            foreach (var loadedObject in _loadedObjects)
            {
                RemoveLoadObject(loadedObject);
            }
            
            _loadedObjects.Clear();
        }
        
        internal void UnLoadObjects(params IShowableUI[] objects)
        {
            foreach (var obj in objects)
            {
                if (FindObjectInLoadedObjects(obj))
                {
                    RemoveLoadObject(obj);
                }
            }
        }
    
        #endregion
        
        #region PrivateFunction
    
        private void RemoveLoadObject(IShowableUI obj)
        {
            obj.Dispose();
        }
    
        private bool FindObjectInLoadedObjects(IShowableUI obj)
        {
            foreach (var loadedObject in _loadedObjects)
            {
                if (obj.Equals(loadedObject))
                {
                    return true;
                }
            }
    
            return false;
        }
    
        #endregion
        
    }
    
    public interface IShowableUI : IDisposable
    {
        void Show();
    }
}

