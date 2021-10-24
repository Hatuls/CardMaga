
using UnityEngine;


namespace Map
{
    public abstract class EventPointAbstSO : ScriptableObject
    {
        [SerializeField]
        private Color _clr;
        public Color PointColor => _clr;
        [SerializeField]
        private Sprite _icon;
        public abstract EventPointType PointType {get;}
        public string Name; 
        public abstract void ActivatePoint();
        public Sprite Icon => _icon;
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