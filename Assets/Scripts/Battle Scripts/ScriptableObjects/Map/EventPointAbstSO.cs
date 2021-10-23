
using UnityEngine;


namespace Map
{

    public abstract class EventPointAbstSO : ScriptableObject
    {
        [SerializeField]
        private Sprite _icon;
        public abstract EventPointType PointType {get;}
        public abstract void ActivatePoint();
        public Sprite Icon => _icon;
    }

    [CreateAssetMenu (fileName = "Chest", menuName = "ScriptableObjects/Map/Points/Chest")]
    public class ChestPoint : EventPointAbstSO
    {
        public override EventPointType PointType =>  EventPointType.Chest;

        public override void ActivatePoint()
        {
            throw new System.NotImplementedException();
        }
    }


    [CreateAssetMenu (fileName = "Rest Area Point", menuName = "ScriptableObjects/Map/Points/Rest Area")]
    public class RestAreaPoint : EventPointAbstSO
    {
        public override EventPointType PointType => EventPointType.Rest_Area;

        public override void ActivatePoint()
        {
            throw new System.NotImplementedException();
        }
    }
    [CreateAssetMenu(fileName = "Dojo Point", menuName = "ScriptableObjects/Map/Points/Dojo")]
    public class DojoPoint : EventPointAbstSO
    {
        public override EventPointType PointType => EventPointType.Dojo;

        public override void ActivatePoint()
        {
            throw new System.NotImplementedException();
        }
    }


    [CreateAssetMenu(fileName = "Question Point", menuName = "ScriptableObjects/Map/Points/Question Mark")]
    public class QuestionMarkPoint : EventPointAbstSO
    {
        public override EventPointType PointType =>  EventPointType.QuestionMark;

        public override void ActivatePoint()
        {
            throw new System.NotImplementedException();
        }
    }


    [CreateAssetMenu(fileName = "Basic Enemy", menuName = "ScriptableObjects/Map/Points/Basic Enemy")]
    public class BasicEnemyPoint : EventPointAbstSO
    {
        public override EventPointType PointType =>  EventPointType.Basic_Enemy;

        public override void ActivatePoint()
        {
            throw new System.NotImplementedException();
        }
    }

    [CreateAssetMenu(fileName = "Elite Enemy", menuName = "ScriptableObjects/Map/Points/Elite Enemy")]
    public class EliteEnemyPoint : EventPointAbstSO
    {
        public override EventPointType PointType =>  EventPointType.Elite_Enemy;

        public override void ActivatePoint()
        {
            throw new System.NotImplementedException();
        }
    }

    [CreateAssetMenu(fileName = "Boss Enemy", menuName = "ScriptableObjects/Map/Points/Boss Enemy")]
    public class BossEnemyPoint : EventPointAbstSO
    {
        public override EventPointType PointType =>      EventPointType.Boss_Enemy;

        public override void ActivatePoint()
        {
            throw new System.NotImplementedException();
        }
    }

    public enum EventPointType
    {
        None = 0,
        Basic_Enemy =1,
        Elite_Enemy=2,
        Boss_Enemy=3,
        QuestionMark=4,
        Rest_Area=5,
        Dojo=6,
        Chest=7,
    }
}