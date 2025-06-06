using UnityEngine;

namespace Scenes.BallStateMachine.Ball
{ 
    public class BallMover
    {
        private readonly Rigidbody _rb;
        private readonly float _moveForce;
        private readonly float _jumpForce;
        
        public BallMover(Rigidbody rb, float moveForce, float jumpForce)
        {
            _rb = rb;
            _moveForce = moveForce;
            _jumpForce = jumpForce;
        }

        public void Move(Vector3 movementDirection)
        {
            _rb.AddForce(movementDirection.normalized * _moveForce);
        }

       
        public void Jump()
        {
            _rb.AddForce(Vector3.up * _jumpForce);
        }
    }
}