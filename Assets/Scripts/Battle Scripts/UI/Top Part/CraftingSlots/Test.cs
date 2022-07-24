using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private RectTransform me;
    [SerializeField] private RectTransform dis;
  
    [SerializeField] private TransitionPackSO _transitionPackSo;

    [Sirenix.OdinInspector.Button]
    private void TestTest()
    {
        me.Transition(_transitionPackSo);
    }
}
