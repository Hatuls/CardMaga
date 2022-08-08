using CardMaga.UI.Card;
namespace CardMaga.Input
{
    public class CardUIInputHandler : TouchableItem<CardUI>
    {
        private IInputBehaviour _inputBehaviours;

        public IInputBehaviour InputBehaviour
        {
            get { return _inputBehaviours;}
            set { _inputBehaviours = value; }
        }
        
        private void OnEnable()
        {
            ForceChangeState(false);
        }

        protected override void Click()
        {
            base.Click();
            _inputBehaviours.Click();
        }

        protected override void Hold()
        {
            base.Hold();
            _inputBehaviours.Hold();
        }

        protected override void BeginHold()
        {
            base.BeginHold();
            _inputBehaviours.BeginHold();
        }

        protected override void PointDown()
        {
            base.PointDown();
            _inputBehaviours.PointDown();
        }

        protected override void EndHold()
        {
            base.EndHold();
            _inputBehaviours.EndHold();
        }

        protected override void PointUp()
        {
            base.PointUp();
            _inputBehaviours.PointUp();
        }
    }
}