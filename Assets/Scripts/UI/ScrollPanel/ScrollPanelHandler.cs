using System.Collections.Generic;
using UnityEngine;

namespace CardMaga.UI.ScrollPanel
{
    public class ScrollPanelHandler : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Transform _holder;
        private List<IUIElement> _loadedObjects;
    
        #endregion

        #region Prop

        public Transform Holder => _holder;

        #endregion

        public IReadOnlyList<IUIElement> LoadedObjects
        {
            get => _loadedObjects;
        }

        public void Init()
        {
            _loadedObjects = new List<IUIElement>();
        }

        #region PublicFunction
    
        internal void LoadObject(params IUIElement[] objects)
        {
            foreach (var obj in objects)
            {
                _loadedObjects.Add(obj);
                obj.Show();
            }
        }
        
        internal void LoadObject(List<IUIElement> objects)
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
        
        internal void UnLoadObjects(params IUIElement[] objects)
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
    
        private void RemoveLoadObject(IUIElement obj)
        {
            obj.Hide(); 
            //obj.Dispose();
        }
    
        private bool FindObjectInLoadedObjects(IUIElement obj)
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
    
   
}
