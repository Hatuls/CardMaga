
using Cards;
using Rei.Utilities;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] Animation _animationController;

    [SerializeField] Transform targetToLookAt;

    AnimationBundle _currentAnimation;
    Queue<AnimationBundle> _animationQueue = new Queue<AnimationBundle>();
    [SerializeField] Vector3 startPos;
    bool IsMyTurn;
    public void SetAnimationQueue(Card card)
    {
        // When we finish the planning turn we get the cards and start the animation 
        if (card == null)
            return;

        SetAnimationQueue(card.CardSO.AnimationBundle);

    }
    public void SetAnimationQueue(AnimationBundle animationBundle)
    {
        if (animationBundle == null)
            return;

        _animationQueue.Enqueue(animationBundle);


        IsMyTurn = true;

        if (_animationQueue.Count == 1) //&& isFirst
        {
            TranstionToNextAnimation();
        }
    }
    public void TranstionToNextAnimation()
    {
        string name = _animationQueue.Dequeue()._attackAnimation.ToString();
        if (_animationQueue.Count == 0)
            name ="Idle_1";

        _animationController.CrossFadeQueued(name, 1f, QueueMode.CompleteOthers);

    }
}
