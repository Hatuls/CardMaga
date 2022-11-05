using System;
using System.Collections.Generic;
using UnityEngine;

public class ViewWindowHandler : MonoBehaviour
{

    [SerializeField] private BaseView[] _viewWindows;

    [SerializeField] private BaseView _firstViewWindow;

    private BaseView _currentView;

    private readonly Stack<BaseView> _viewHistory = new Stack<BaseView>();

    public static ViewWindowHandler Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        for (int i = 0; i < _viewWindows.Length; i++)
        { 
            _viewWindows[i].Init();
            _viewWindows[i].Hide();
        }

        _currentView = _firstViewWindow;
        _currentView.Show();
    }

    private T GetView<T>() where T : BaseView
    {
        for (int i = 0; i < _viewWindows.Length; i++)
        {
            if (_viewWindows[i] is T view)
            {
                return view;
            } 
        }

        return null;
    }

    public void Show<T>(bool save = true) where T : BaseView
    {
        BaseView view = GetView<T>();

        if (view == null)
        {
            Debug.LogError("Can not Find ViewWindow in ViewWindowHandler");
            return;
        }

        if (_currentView != null)
        {
            if (save)
            {
                _viewHistory.Push(_currentView);
            }
            
            _currentView.Hide();

            _currentView = view;
            
            _currentView.Show();
        }
        else//first time Show
        {
            _currentView = view;
            
            _currentView.Show();
        }
    }
    
    public void Show(BaseView view,bool save = true)
    {
        if (view == null)
        {
            Debug.LogError("Invalid BaseView Parameter");
            return;
        }

        if (_currentView != null)
        {
            if (save)
            {
                _viewHistory.Push(_currentView);
            }
            
            _currentView.Hide();

            _currentView = view;
            
            _currentView.Show();
        }
        else//first time Show
        {
            _currentView = view;
            
            _currentView.Show();
        }
    }

    public void ShowLast()
    {
        if (_viewHistory.Count != 0)
        {
            Show(_viewHistory.Pop());
        }
    }
}
