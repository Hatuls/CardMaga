using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUINew : MonoBehaviour
{
    [SerializeField] private CardStateMachineNew _stateMachine;
    [SerializeField] private CardUIInputHandler _input;
    [SerializeField] private CardLocoMotion _locoMotion;
}
