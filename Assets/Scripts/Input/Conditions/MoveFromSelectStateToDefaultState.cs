using CardMaga.UI;

namespace CardMaga.Input
{

    public class MoveFromSelectStateToDefaultState : BaseCondition
    {
        public override bool CheckCondition()
        {
            return _moveCondition;
        }

        public override void InitCondition()
        {
            HandUI.OnInputCardReturnToHand += ChangeState;
        }

        private void ChangeState()
        {
            HandUI.OnInputCardReturnToHand -= ChangeState;
            _moveCondition = true;
        }
    }
}