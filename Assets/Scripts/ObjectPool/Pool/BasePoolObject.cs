using System.Collections.Generic;
using UnityEngine;

public abstract class BasePoolObject<T_visual,T_data> : MonoBehaviour where T_visual : MonoBehaviour ,IPoolable<T_visual> , IVisualAssign<T_data>
{
    [SerializeField] private T_visual _objectPrefab;
    [SerializeField] private RectTransform _parent;

    private ObjectPool<T_visual> _objectPool;

    public virtual void Init()
    {
        _objectPool = new ObjectPool<T_visual>(_objectPrefab, _parent);
    }

    public List<T_visual> PullObjects(List<T_data> objectData)
    {
        if (objectData == null || objectData.Count == 0)
            return null;
            
        List<T_visual> output = new List<T_visual>(objectData.Count);

        for (int i = 0; i < objectData.Count; i++)
        {
            T_visual cache = _objectPool.Pull();
            
            cache.AssingVisual(objectData[i]);
            
            output.Add(cache);
        }

        return output;
    }
}
