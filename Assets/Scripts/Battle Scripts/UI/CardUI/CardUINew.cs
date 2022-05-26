using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CardUINew : MonoBehaviour
{
    [SerializeField] private CardStateMachineNew _stateMachine;
    [SerializeField] private CardUIInputHandler _input;
    [FormerlySerializedAs("_locoMotion")] [SerializeField] private CardLocoMotionUI locoMotionUI;
}
