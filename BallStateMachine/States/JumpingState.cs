namespace Scenes.BallStateMachine.States
{
    public class JumpingState : IState
    {
        private readonly BallController _ball;

        public JumpingState(BallController ball)
        {
            _ball = ball;
        }

        public void Execute()
        {
            var input = _ball.Input.MovementInput;
            if (input.magnitude > 0.1f)
            {
                _ball.Motor.Move(input);
            }

            if (_ball.IsGrounded)
            {
                _ball.ChangeState(input.magnitude > 0.1f ?
                    _ball.MovingState : 
                    _ball.IdleState);
            }
        }
    }
}