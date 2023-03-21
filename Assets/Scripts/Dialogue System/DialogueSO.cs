using UnityEngine;
using TMPro;
namespace CardMaga.DialogueSO
{
    [CreateAssetMenu(fileName = "New Dialogue SO", menuName = "ScriptableObjects/Dialogue System/Dialogue SO")]
    public class DialogueSO : ScriptableObject
    {
        [SerializeField] public RobotSettings RobotDialogSettings;
        [SerializeField] public string _characterText;
        [SerializeField] public float _delayTimeForClick;
    }
}

