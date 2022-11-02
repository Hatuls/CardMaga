using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New NotificationUITree", menuName = "ScriptableObjects/UI/Notification System/New NotificationUITree")]
public class NotificationUITreeSO : ScriptableObject
{
    [FormerlySerializedAs("_notificationUIElement")] [SerializeField] private BaseNotificationUIElement baseNotificationUIElement;
}
