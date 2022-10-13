using System.Collections.Generic;
using UnityEngine;

public abstract class BaseFilterSystem<T> : MonoBehaviour
{
    private List<BaseFilter<T>> _filters;
    private List<BaseFilter<T>> _activeFilters;
    

    public void AddFilter(BaseFilter<T> filter)
    {
        if (filter == null)
        {
            Debug.LogError("You are trying to add a NULL Filter");
            return;
        }
        
        if (!_filters.Contains(filter))
        {
            Debug.LogError( filter.name + " Not Found in the filter list");
            return;
        }
        
        if (_activeFilters.Contains(filter))
        {
            Debug.LogWarning( filter.name + " is already is the active filter list and you trying to add it again");
            return;
        }
        
        _activeFilters.Add(filter);
    }
    
    public void RemoveFilter(BaseFilter<T> filter)
    {
        if (filter == null)
        {
            Debug.LogError("You are trying to add a NULL Filter");
            return;
        }
        
        if (!_filters.Contains(filter))
        {
            Debug.LogError( filter.name + " Not Found in the filter list");
            return;
        }
        
        if (!_activeFilters.Contains(filter))
        {
            Debug.LogWarning( filter.name + " is not in the active filter list and you trying to remove it");
            return;
        }

        _activeFilters.Remove(filter);
    }

    public List<T> Filter(List<T> objects)
    {
        List<T> output = new List<T>(objects.Count);

        foreach (var obj in objects)
        {
            foreach (var filter in _activeFilters)
            {
                if (filter.Filter(obj))
                {
                    output.Add(obj);
                    break;
                }
            }
        }

        return output;
    }

    public void Reset()
    {
        _activeFilters.Clear();
    }

    private void LoadFilters()
    {

    }
}
