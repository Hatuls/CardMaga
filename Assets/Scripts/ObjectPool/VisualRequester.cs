using System;
using System.Collections.Generic;
using CardMaga.Tools.Pools;
using CardMaga.UI;

namespace CardMaga.ObjectPool
{
    public class VisualRequester<TVisual, TData> where TVisual :  BaseUIElement, IPoolableMB<TVisual>, IVisualAssign<TData>, new()
    {
        private MBPool<TVisual> _objectPool;

        public VisualRequester(TVisual objectRef)
        {
            _objectPool = new MBPool<TVisual>(objectRef);
        }

        public List<TVisual> GetVisual(List<TData> data)
        {
            if (data.Count <= 0)
                return null;

            List<TVisual> visuals = new List<TVisual>(data.Count);

            for (int i = 0; i < data.Count; i++)
            {
                var cache = _objectPool.Pull();
                cache.AssignVisual(data[i]);
                visuals.Add(cache);
            }
            
            return visuals;
        }
        
        public TVisual GetVisual(TData data)
        {
            var cache = _objectPool.Pull();
            cache.AssignVisual(data);
            
            return cache;
        }
    }
}