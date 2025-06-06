namespace Scenes.BallStateMachine.States
{
    public class MovingState : IState
    {
        private readonly BallController _ball;

        public MovingState(BallController ball)
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
                _ball.Motor.Move(input);
            }
            else
            {
                _ball.ChangeState(_ball.IdleState);
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