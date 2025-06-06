namespace Scenes.BallStateMachine.States
{
    public class StateMachine
    {
        private IState _currentState;

        public void ChangeState(IState newState)
        {
            _currentState = newState;
        }

        public void Tick()
        {
            _currentState?.Execute();
        }
    }
}