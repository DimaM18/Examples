namespace Scenes.BallStateMachine.States
{
    public class IdleState : IState
    {
        private readonly BallController _ball;

        public IdleState(BallController ball)
        {
            _ball = ball;
        }

        public void Execute()
        {
            if (!_ball.IsGrounded)
            {
                _ball.ChangeState(_ball.JumpingState);
                return;
            }

            var input = _ball.Input.MovementInput;
            if (input.magnitude > 0.1f)
            {
                _ball.ChangeState(_ball.MovingState);
                return;
            }

            if (!_ball.Input.JumpPressed)
            {
                return;
            }
            
            _ball.Motor.Jump();
            _ball.ChangeState(_ball.JumpingState);

        }
    }
}