using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Enumerable = System.Linq.Enumerable;
using Object = UnityEngine.Object;

public abstract class BaseFilterSystem<T_Filter,T_FilterRef> : MonoBehaviour where T_FilterRef :  Object , IFilter<T_Filter>
{
    public event Action OnCycleFilter;
    
    private const string Resources_Path = "FilterSO";
    
    [SerializeField] protected List<T_FilterRef> _filters;

    private List<T_FilterRef> _activeFilters;

    private int _currentFilterIndex;

   // protected abstract List<BaseFilter<T_Filter>> Filters { get; }
    
    protected virtual void Awake()
    {
        _activeFilters = new List<T_FilterRef>();
        _currentFilterIndex = 0;
    }

    public void AddFilter(T_FilterRef filter)
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
    
    public void RemoveFilter(T_FilterRef filter)
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

    public IEnumerable<T_Filter> Filter(IEnumerable<T_Filter> objects)
    {
        if (_activeFilters == null || _activeFilters.Count == 0)
            return objects;

        List<T_Filter> output = new List<T_Filter>();
        
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

    public void CycleFilter()
    {
        if (_activeFilters.Count == 0)
        {
            _activeFilters.Add(_filters[_currentFilterIndex]);
            OnCycleFilter?.Invoke();
            return;
        }

        _activeFilters.Remove(_filters[_currentFilterIndex]);

        if (_currentFilterIndex == _filters.Count - 1)
        {
            ResetFilters();
            OnCycleFilter?.Invoke();
            return;
        }
        
        _currentFilterIndex++;
        _activeFilters.Add(_filters[_currentFilterIndex]);
        OnCycleFilter?.Invoke();
    }

    public void ResetFilters()
    {
        _currentFilterIndex = 0;
        _activeFilters.Clear();
    }
    
    [Button("Load Filters")]
    private void LoadFilters()
    {
       _filters = Enumerable.ToList(Resources.LoadAll<T_FilterRef>(Resources_Path));
    }
}
