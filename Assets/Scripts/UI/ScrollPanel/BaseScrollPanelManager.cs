using System.Collections.Generic;
using CardMaga.UI.ScrollPanel;
using UnityEngine;

public abstract class BaseScrollPanelManager<T1,T2> : MonoBehaviour where T1 : MonoBehaviour , IShowableUI , IPoolable<T1> , IVisualAssign<T2>
{
    [SerializeField] private ScrollPanelHandler _scrollPanel;

    public abstract BasePoolObject<T1,T2> ObjectPool { get; }

    public void AddObjectToPanel(params T2[] data)
    {
        List<T1> cache = ObjectPool.PullObjects(data);
        
        IShowableUI[] showableUis = new IShowableUI[cache.Count];

        for (int i = 0; i < cache.Count; i++)
        {
            showableUis[i] = cache[i];
        }
        
        _scrollPanel.LoadObject(showableUis);
    }
    
    public void RemoveAllObjectsFromPanel()
    {
        _scrollPanel.UnLoadAllObjects();
    }
}
