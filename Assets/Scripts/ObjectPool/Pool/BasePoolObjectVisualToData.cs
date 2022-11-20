using System;
using CardMaga.Tools.Pools;
using System.Collections.Generic;
using UnityEngine;

public class BasePoolObjectVisualToData<T_visual,T_data> where T_visual : MonoBehaviour ,IPoolableMB<T_visual> , IVisualAssign<T_data> ,new()
{
    public event Action<List<T_visual>> OnObjectsPulled;
    public event Action<T_visual> OnObjectPulled;

    private T_visual _objectPrefab;
    private RectTransform _parent;

    private IPoolMBObject<T_visual> _objectPool;

    public RectTransform Parent
    {
        get => _parent;
    }

    public BasePoolObjectVisualToData(T_visual objectPrefab,RectTransform parent)
    {
        _objectPool = new MBPool<T_visual>(objectPrefab, parent);
    }
    
    public List<T_visual> PullObjects(List<T_data> objectData)
    {
        if (objectData == null || objectData.Count == 0)
            return null;
            
        List<T_visual> output = new List<T_visual>(objectData.Count);

        for (int i = 0; i < objectData.Count; i++)
        {
            T_visual cache = _objectPool.Pull();
            
            cache.transform.SetSiblingIndex(i);
            
            cache.AssignVisual(objectData[i]);
            
            output.Add(cache);
        }
        
        OnObjectsPulled?.Invoke(output);
        return output;
    }
    
    public T_visual PullObjects(T_data objectData)
    {
        if (objectData == null)
            return null;
        
        T_visual output = _objectPool.Pull();
            
        output.transform.SetSiblingIndex(0);
            
        output.AssignVisual(objectData);
        
        OnObjectPulled?.Invoke(output);
        
        return output;
    }
}
