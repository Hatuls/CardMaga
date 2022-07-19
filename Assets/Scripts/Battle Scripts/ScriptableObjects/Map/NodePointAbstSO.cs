
using UnityEngine;
using static CardMaga.ActDifficultySO;

namespace CardMaga
{
    public abstract class NodePointAbstSO : ScriptableObject
    {
        [SerializeField]
        protected SoundEventSO OnEnterNode;
        [SerializeField]
        private Color _clr;
        public Color PointColor => _clr;
        [SerializeField]
        private Sprite _backGroundImage;
        [SerializeField]
        private Sprite _icon;
        [Sirenix.OdinInspector.ShowInInspector]
        public abstract NodeType PointType {get;}
        public string Name;
        public virtual void ActivatePoint(NodeLevel act) { }
        public abstract void ActivatePoint();
        public Sprite Icon => _icon;
        public Sprite BackGroundImage =>_backGroundImage;
    }

    public enum NodeType
    {
        None = 0,
        Basic_Enemy =1,
        Elite_Enemy=2,
        Chest=3,
        QuestionMark=4,
        Rest_Area=5,
        Dojo=6,
        Boss_Enemy=7,
    }
}