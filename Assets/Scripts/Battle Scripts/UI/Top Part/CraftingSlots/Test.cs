using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private RectTransform me;
    [SerializeField] private RectTransform dis;
    private RectTransitionManager _rectTransitionManager;
    [SerializeField] private TransitionPackSO _transitionPackSo;
    void Start()
    {
        _rectTransitionManager = new RectTransitionManager(me);
    }

    [Sirenix.OdinInspector.Button]
    private void TestTest()
    {
        _rectTransitionManager.Transition(_transitionPackSo);
    }
}
