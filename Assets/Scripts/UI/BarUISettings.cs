using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Bars Settings", menuName = "ScriptableObjects/Settings/Bars")]
public class BarUISettings : ScriptableObject
{
    [TitleGroup("Bars Settings")]


    [InfoBox("When Recieved Damage\nOr Gain Health")]
    [TabGroup("Bars Settings/Settings", "Hp Bar")]
    public float DelayTime;
    [TabGroup("Bars Settings/Settings", "Hp Bar")]
    public LeanTweenType DelayTimeLeanTweenEase;
    [Space]
    [InfoBox("When the battle starts and the bars fills")]
    [TabGroup("Bars Settings/Settings", "Hp Bar")]
    public float InitTime;
    [TabGroup("Bars Settings/Settings", "Hp Bar")]
    public LeanTweenType InitTimeLeanTweenEase;

}