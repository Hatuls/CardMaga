using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpManager : MonoSingleton<PopUpManager>
{
    #region Pre-arranged screen locations
    [SerializeField] private RectTransform _screenLeftLocation;
    [SerializeField] private RectTransform _screenRightLocation;
    [SerializeField] private RectTransform _screenUpLocation;
    [SerializeField] private RectTransform _screenDownLocation;
    [SerializeField] private RectTransform _screenMiddleLocation;
    

    public Vector2 ScreenLeftLocation => _screenLeftLocation.position;
    public Vector2 ScreenRightLocation => _screenRightLocation.position;
    public Vector2 ScreenUpLocation => _screenUpLocation.position;
    public Vector2 ScreenDownLocation => _screenDownLocation.position;
    public Vector2 ScreenMiddleLocation => _screenMiddleLocation.position;
    #endregion
    
}
