using System;
using System.Collections.Generic;
using UnityEngine;

public class ClickHelper : MonoBehaviour
{
    static ClickHelper _instance;
    public static ClickHelper Instance { get { return _instance; } }

    #region Fields

    [SerializeField] private Clicker _clicker;
    [SerializeField] public RectTransform _panel;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private ClickBlocker _clickBlocker;
    [SerializeField] private bool _closeOnClick = false;
    
    private Action _action;
    
    private List<RectTransform> _loadedObjects;
    private List<RectTransform> _loadedObjectParents;

    
    #endregion

    #region Properties

    public ClickBlocker ClickBlocker { get {return _clickBlocker; } }
    public Clicker ZoomInClicker { get {return _clicker; } }

    #endregion

    #region UnityCallBack

    private void Awake()
    {
        _loadedObjects = new List<RectTransform>();
        _loadedObjectParents = new List<RectTransform>();
        _clicker.OnClickValue += Click;
        _instance = this;
        _clickBlocker.InitClickHelper(this);
    }

    private void OnDestroy()
    {
        _clicker.OnClickValue -= Click;
    }

    #endregion

    #region PublicFunction
    /// <summary>
    /// A function that returns all loaded objects to their original position in the hierarchy, and closes the panel
    /// </summary>
    /// 

    public void LoadAction(Action action)
    {
        _action = action;
    }

    public void Close()
    {
        if(!_canvas.gameObject.activeSelf)
            return;

        ReturnObjects();

        
        _canvas.gameObject.SetActive(false);
    }

    public void ReturnObjects()
    {
        for (int i = 0; i < _loadedObjects.Count; i++)
        {
            _loadedObjects[i].SetParent(_loadedObjectParents[i]);
        }
        
        _loadedObjects.Clear();
        _loadedObjectParents.Clear();
    }

    public void Open(GameObject canvas)
    {
        canvas.SetActive(true);
    }

    public void Open()
    => Open(_canvas.gameObject);
    /// <summary>
    /// Open The ClickHelper and load a action when click
    /// </summary>
    /// <param name="action">The action that will Invoke when click</param>
    public void Open(Action action)
    {
        Open();
        _action = action;
    }

    /// <summary>
    /// A function that loads the objects in the list and initializes the panel
    /// </summary>
    /// <param name="openOnLoad">a bool to open the panel at the end of loading objects</param>
    /// <param name="closeOnClick">Close the panel right after the first click</param>
    /// <param name="action">An action or function to be performed at the moment of a click</param>
    /// <param name="objects">Objects to load into the panel</param>
    public void LoadObject(bool openOnLoad, bool closeOnClick , Action action, params RectTransform[] objects)
    {
        if (openOnLoad)
        {
            Open(_canvas.gameObject);
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
