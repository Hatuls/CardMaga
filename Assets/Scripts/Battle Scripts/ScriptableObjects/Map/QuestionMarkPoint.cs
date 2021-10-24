
using UnityEngine;


namespace Map
{
    [CreateAssetMenu(fileName = "Question Point", menuName = "ScriptableObjects/Map/Points/Question Mark")]
    public class QuestionMarkPoint : EventPointAbstSO
    {
        public override EventPointType PointType =>  EventPointType.QuestionMark;

        public override void ActivatePoint()
        {
            throw new System.NotImplementedException();
        }
    }
}