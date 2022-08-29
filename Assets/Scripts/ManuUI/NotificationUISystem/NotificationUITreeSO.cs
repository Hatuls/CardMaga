using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NotificationUITree", menuName = "ScriptableObjects/UI/Notification System/New NotificationUITree")]
public class NotificationUITreeSO : ScriptableObject
{
    [SerializeField] private NotificationUIElement _notificationUIElement;
}
