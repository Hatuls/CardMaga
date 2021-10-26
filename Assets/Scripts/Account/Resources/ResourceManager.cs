using System;
using System.Collections.Generic;
using UnityEngine;
namespace Meta.Resources
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
    public class ResourceManager
    {
        #region Singleton
        private static ResourceManager _instance;
        public static ResourceManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ResourceManager();

                return _instance;
            }
        }
        public ResourceManager()
        {
            Init();
        }
        #endregion
        #region Fields
        static Dictionary<ResourceType, object> _resourceDictionary;
        #endregion
        #region Public Methods
        public void Init()
        {
            const byte resourceAmount = 5;
            _resourceDictionary = new Dictionary<ResourceType, object>(resourceAmount)
            {   {ResourceType.Gold,new GoldHandler()}
                ,{ResourceType.Chips, new ChipsHandler()}
                ,{ResourceType.Diamonds, new DiamondsHandler()}
                ,{ResourceType.Energy,new EnergyHandler()}
                ,{ResourceType.EXP, new ExpierenceHandler()}
            }; 
        }
        public ResourceHandler<T> GetResourceHandler<T>(ResourceType resourceType) where T : struct
        {
            if (_resourceDictionary.TryGetValue(resourceType, out object value))
                return (ResourceHandler<T>)value;

            throw new Exception("ResourceManager resource type not found");
        }
        #endregion
    }
}
