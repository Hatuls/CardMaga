
using UnityEngine;
namespace CardMaga.AI
{

    [CreateAssetMenu(fileName = "New AI Tree", menuName ="ScriptableObjects/AI/Tree")]
    public class EnemyTree : Tree<AICard>
    {
        [SerializeField]
        private AIBrain _aIBrain;
        public AIBrain AIBrain => _aIBrain;

        [SerializeField]
        private Node _parent;


        public void AttachTree()
        {
            _parent.Attach(null);
        }
    }

}