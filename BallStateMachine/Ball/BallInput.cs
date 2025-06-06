using UnityEngine;

namespace Scenes.BallStateMachine.Ball
{
    public class BallInput
    {
        public Vector3 MovementInput
        {
            get
            {
                var horizontalInput = Input.GetAxis("Horizontal");
                var verticalInput = Input.GetAxis("Vertical");
                
                return new Vector3(horizontalInput, 0f, verticalInput);
            }
        }

        public bool JumpPressed => Input.GetKeyDown(KeyCode.Space);
    }

}