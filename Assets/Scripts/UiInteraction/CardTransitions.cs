using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CardTransitions : MonoBehaviour
{
    [SerializeField] private List<TransitionPackSO> _transitionPackSos;

    public List<TransitionPackSO> TransitionPackSos
    {
        get { return _transitionPackSos; }
    }
}
