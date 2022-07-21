using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;

using UnityEngine;
namespace CardMaga.AI
{
    #region Nodes
    public enum NodeState
    {
        Success, Failure, Running
    }


    public class Node
    {
        [Tooltip("To set the weight to this node's weight value or to add this weight to the previous nodes's values")]
        [NonSerialized, OdinSerialize]
        private bool _isSetValue;
        [Tooltip("The Weight Of This Node\n")]
        [NonSerialized, OdinSerialize]
        protected int _weight = 0;
        [NonSerialized, OdinSerialize]
        protected BaseDecisionLogic _evaluator;
        [NonSerialized, OdinSerialize]
        private Node[] _children;


        public Node Parent { get; private set; } = null;
        public Node[] Children => _children;
        public NodeState NodeState { get; set; }



        public void Attach(Node parent)
        {
            Parent = parent;
            for (int i = 0; i < _children.Length; i++)
                _children[i].Attach(this);
        }

        public NodeState Evaluate(AICard evaluateObject) => _evaluator?.Evaluate(this, evaluateObject) ?? NodeState.Failure;
        public void AssignWeight(IWeightable evaluateObject)
            => evaluateObject.Weight = (_isSetValue) ? _weight : (evaluateObject.Weight + _weight);


    }
    public interface IWeightable
    {
        int Weight { get; set; }
    }

    #region Nodes Logic
    public abstract class BaseDecisionLogic : ScriptableObject
    {

        public abstract NodeState Evaluate(Node currentNode, AICard basedEvaluationObject);
    }
    #endregion
    #endregion

    #region Tree
    [Serializable]
    public abstract class Tree<T> : SerializedScriptableObject
        where T : IWeightable
    {

    }
    #endregion

}