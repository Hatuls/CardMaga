using System.Collections.Generic;
using UnityEngine;

public abstract class BasePoolObject<T1,T2> : MonoBehaviour where T1 : MonoBehaviour ,IPoolable<T1> , IVisualAssign<T2>
{
    [SerializeField] private T1 _objectPrefab;
    [SerializeField] private RectTransform _parent;

    private ObjectPool<T1> _objectPool;

    private void Awake()
    {
        _objectPool = new ObjectPool<T1>(_objectPrefab, _parent);
    }

    public List<T1> PullObjects(params T2[] objectData)
    {
        if (objectData == null || objectData.Length == 0)
            return null;
            
        List<T1> output = new List<T1>(objectData.Length);

        for (int i = 0; i < objectData.Length; i++)
        {
            T1 cache = _objectPool.Pull();
            
            cache.AssingVisual(objectData[i]);
            
            output.Add(cache);
        }

        return output;
    }
}
