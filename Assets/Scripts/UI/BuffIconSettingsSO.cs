using UnityEngine;

[CreateAssetMenu(fileName = "BuffIconSettingsSO", menuName = "ScriptableObjects/Settings/Tweens/Buff Icons")]
public class BuffIconSettingsSO : ScriptableObject {
    [SerializeField] float _scaleExitTime;
    [SerializeField] float _scaleEntranceTime;
    [SerializeField] float _scaleAmount;
    [SerializeField] float _alphaExitTime;
    [SerializeField] float _alphaEntranceTime;
    [SerializeField] LeanTweenType _exitType;
    [SerializeField] LeanTweenType _entranceType;

    public float ScaleExitTime => _scaleExitTime;
    public float ScaleEntranceTime => _scaleEntranceTime;
    public float ScaleAmount => _scaleAmount;
    public float AlphaExitTime => _alphaExitTime;
    public float AlphaEntranceTime => _alphaEntranceTime;
    public LeanTweenType ExitTypeTweenType => _exitType;
    public LeanTweenType EntranceTypeTweenType => _entranceType;
}