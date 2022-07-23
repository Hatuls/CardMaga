using System;
namespace CardMaga.AI
{
    #region Nodes
    public enum NodeState
    {
        Success, Failure, Running
    }

    public class BaseNode<T> : IEvaluator<T>
    {
        public NodeState NodeState { get; set; }

        public IEvaluator<T> Parent { get; private set; }

        public IEvaluator<T>[] Children { get; set; }

        public virtual NodeState Evaluate(T evaluateObject) => NodeState.Failure;
        public void Attach(IEvaluator<T> parent)
        {
            Parent = parent;
            for (int i = 0; i < Children.Length; i++)
                Children[i].Attach(this);
        }
    }

    #endregion

    #region Tree
    [Serializable]
    public abstract class Tree<T> : IEvaluator<T>
   
    {
        public IEvaluator<T> Parent { get; set; } = null;

        public IEvaluator<T>[] Children { get => Parent.Children; set => Parent.Children = value; }

        public void Attach(IEvaluator<T> parent)
        {
            Parent = parent;
            SetupTree();
            for (int i = 0; i < Children.Length; i++)
                Children[i].Attach(this);
        }
        public abstract void SetupTree();

        public NodeState Evaluate(T evaluateObject) => Children[0].Evaluate(evaluateObject);
    }

    #endregion
    public interface IEvaluator<T>
  
    {
        IEvaluator<T> Parent { get; }
        IEvaluator<T>[] Children { get; set; }
        void Attach(IEvaluator<T> parent);
        NodeState Evaluate(T evaluateObject);
    }
    public interface IWeightable
    {
        int Weight { get; set; }
    }

}