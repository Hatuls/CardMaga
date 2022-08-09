using UnityEngine;

[CreateAssetMenu(fileName = "BuffIconSettingsSO", menuName = "ScriptableObjects/Settings/Tweens/Buff Icons")]
public class BuffIconSettingsSO : ScriptableObject {
    [SerializeField] float _scaleExitTime;
    [SerializeField] float _scaleEntranceTime;
    [SerializeField] float _scaleAmount;
    [SerializeField] float _alphaExitTime;
    [SerializeField] float _alphaEntranceTime;
    [SerializeField] AnimationCurve _exitCurve;
    [SerializeField] AnimationCurve _entranceCurve;

    public float ScaleExitTime => _scaleExitTime;
    public float ScaleEntranceTime => _scaleEntranceTime;
    public float ScaleAmount => _scaleAmount;
    public float AlphaExitTime => _alphaExitTime;
    public float AlphaEntranceTime => _alphaEntranceTime;
    public AnimationCurve ExitTypeTweenType => _exitCurve;
    public AnimationCurve EntranceTypeTweenType => _entranceCurve;
}