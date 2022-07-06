
using UnityEngine;


namespace CardMaga
{
    [CreateAssetMenu(fileName = "Question Point", menuName = "ScriptableObjects/Map/Points/Question Mark")]
    public class QuestionMarkPoint : NodePointAbstSO
    {
        public override NodeType PointType =>  NodeType.QuestionMark;

        public override void ActivatePoint()
        {
            throw new System.NotImplementedException();
        }
    }
}