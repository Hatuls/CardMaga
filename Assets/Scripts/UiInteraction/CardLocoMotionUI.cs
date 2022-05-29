using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CardLocoMotionUI : LocoMotionUI
{
    [SerializeField] private Vector2 _vector2 = new Vector2(0, 0);
    [SerializeField] private TransitionsSO _motion;
    [ContextMenu("Move")]
    private void MoveTo()
    {
        Transition(_vector2,_motion.LocoMotionSo);
    }
}
