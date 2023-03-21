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
            if (Children != null)
            {
                for (int i = 0; i < Children.Length; i++)
                    Children[i].Attach(this);
            }
        }
    }

    #endregion

    #region Tree
    [Serializable]
    public abstract class Tree<T> : IEvaluator<T>

    {
        public IEvaluator<T> Parent { get; set; } = null;

        public IEvaluator<T>[] Children { get; set; }

        public void Attach(IEvaluator<T> parent)
        {
            try
            {
                Parent = parent;
                SetupTree();
                if (Children != null)
                {
                    for (int i = 0; i < Children.Length; i++)
                        Children[i].Attach(this);
                }

            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError(this.ToString());
                throw e;
            }
        }
        public abstract void SetupTree();

        public NodeState Evaluate(T evaluateObject) => Children[0]?.Evaluate(evaluateObject) ?? NodeState.Failure;
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