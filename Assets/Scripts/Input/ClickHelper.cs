using System;
using System.Collections.Generic;
using CardMaga.Input;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickHelper : MonoBehaviour
{
    #region Fields

    [SerializeField] private Clicker _clicker;
    [SerializeField] private RectTransform _panel;
    [SerializeField] private RectTransform _canvas;
    
    private Action _action;
    
    private List<RectTransform> _loadedObjects;
    private List<RectTransform> _loadedObjectParents;

    private bool _closeOnClick = false;

    #endregion

    #region UnityCallBack

    private void Awake()
    {
        _loadedObjects = new List<RectTransform>();
        _loadedObjectParents = new List<RectTransform>();
        _clicker.OnClick += Click;
    }

    private void OnDestroy()
    {
        _clicker.OnClick -= Click;
    }

    #endregion
    
    #region PublicFunction
    /// <summary>
    /// A function that returns all loaded objects to their original position in the hierarchy, and closes the panel
    /// </summary>
    public void Close()
    {
        if(!_canvas.gameObject.activeSelf)
            return;
        
        for (int i = 0; i < _loadedObjects.Count; i++)
        {
            _loadedObjects[i].SetParent(_loadedObjectParents[i]);
        }

        _loadedObjects.Clear();
        _loadedObjectParents.Clear();
        
        _canvas.gameObject.SetActive(false);
    }

    public void Open()
    {
        _canvas.gameObject.SetActive(true);
    }
    /// <summary>
    /// A function that loads the objects in the list and initializes the panel
    /// </summary>
    /// <param name="openOnLoad">a bool to open the panel at the end of loading objects</param>
    /// <param name="closeOnClick">Close the panel right after the first click</param>
    /// <param name="action">An action or function to be performed at the moment of a click</param>
    /// <param name="objects">Objects to load into the panel</param>
    public void LoadObject(bool openOnLoad, bool closeOnClick, Action action, params RectTransform[] objects)
    {
        if (openOnLoad)
        {
            Open();
        }
        
        for (int i = 0; i <  objects.Length; i++)
        {
            _loadedObjectParents.Add(objects[i].parent as RectTransform);
            _loadedObjects.Add(objects[i]);
                
            objects[i].SetParent(_panel.transform);
        }

        _action = action;
        _closeOnClick = closeOnClick;
    }

    #endregion

    #region ClickEvent

    private void Click(Clicker clicker)
    {
        if (_action != null)
            _action?.Invoke();
        
        if (_closeOnClick)
            Close();
    }
    #endregion
}
