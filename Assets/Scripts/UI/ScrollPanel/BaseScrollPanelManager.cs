using System.Collections.Generic;
using CardMaga.Tools.Pools;
using CardMaga.UI.ScrollPanel;
using UnityEngine;

public abstract class BaseScrollPanelManager<T_visual,T_data> : MonoBehaviour where T_visual : MonoBehaviour , IShowableUI , IPoolableMB<T_visual> , IVisualAssign<T_data> ,new()
{
    [SerializeField] private ScrollPanelHandler _scrollPanel;

    protected abstract BasePoolObject<T_visual,T_data> ObjectPool { get; }

    public virtual void Init()
    {
        _scrollPanel.Init();
    }
 
    public void AddObjectToPanel(List<T_data> data)
    {
        if (data.Count <= 0)
            return;
        
        List<T_visual> cache = ObjectPool.PullObjects(data);
        
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
