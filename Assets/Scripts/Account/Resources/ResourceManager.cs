using System;
using System.Collections.Generic;
using UnityEngine;
namespace Account.Resources
{
    public enum ResourceType
    {
        None = 0,
        Gold = 1,
        Diamonds = 2,
        Chips = 3,
        EXP = 4,
        Energy = 5
    }
    public class ResourceManager : MonoBehaviour
    {
        #region Singleton
        private static ResourceManager _instance;
        public static ResourceManager GetInstance
        {
            get
            {
                if (_instance == null)
                    Debug.LogError("ResourceManager is null!");

                return _instance;
            }
        }
        private void Awake()
        {
            _instance = this;
        }
        #endregion
        #region Fields
        static Dictionary<ResourceType, ResourceHandler> _resourceDictionary;
        #endregion
        #region Public Methods
        public void Init()
        {
            
        }
        public ResourceHandler GerResourceHandler(ResourceType resourceType)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
