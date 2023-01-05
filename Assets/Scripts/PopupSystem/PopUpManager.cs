using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardMaga.ObjectPool;
using CardMaga.UI.PopUp;

public class PopUpManager : MonoSingleton<PopUpManager>
{
    #region Pre-arranged screen locations
    [SerializeField] private RectTransform _screenLeftLocation;
    [SerializeField] private RectTransform _screenRightLocation;
    [SerializeField] private RectTransform _screenUpLocation;
    [SerializeField] private RectTransform _screenDownLocation;
    [SerializeField] private RectTransform _screenMiddleLocation;
    [SerializeField] private PoolHandler<BasePoolSO<BasePopUp>, BasePopUp> _poolHandler;

    public PoolHandler<BasePoolSO<BasePopUp>, BasePopUp> PoolHandler => _poolHandler;

    public Vector2 ScreenLeftLocation => _screenLeftLocation.position;
    public Vector2 ScreenRightLocation => _screenRightLocation.position;
    public Vector2 ScreenUpLocation => _screenUpLocation.position;
    public Vector2 ScreenDownLocation => _screenDownLocation.position;
    public Vector2 ScreenMiddleLocation => _screenMiddleLocation.position;
    #endregion

    public override void Awake()
    {
        base.Awake();
        _poolHandler = new PoolHandler<BasePoolSO<BasePopUp>, BasePopUp>(transform);
    }
}
