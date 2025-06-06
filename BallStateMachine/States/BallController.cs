using Scenes.BallStateMachine.Ball;
using UnityEngine;

namespace Scenes.BallStateMachine.States
{
    [RequireComponent(typeof(Rigidbody))]
    public class BallController : MonoBehaviour
    {
        [Header("Настройки физики")]
        [SerializeField] private float moveForce = 10f;
        [SerializeField] private float jumpForce = 300f;

        [Header("Настройка проверки земли")]
        [SerializeField] private float groundCheckDistance = 0.6f;
        [SerializeField] private LayerMask groundLayer;

        private BallMover _motor;
        private BallInput _input;
        private StateMachine _stateMachine;

        public IState IdleState { get; private set; }
        public IState MovingState { get; private set; }
        public IState JumpingState { get; private set; }
        public bool IsGrounded { get; private set; }
        public BallMover Motor => _motor;
        public BallInput Input => _input;

        private void Awake()
        {
            var rb = GetComponent<Rigidbody>();
            _motor = new BallMover(rb, moveForce, jumpForce);
            _input = new BallInput();
            _stateMachine = new StateMachine();

            IdleState    = new IdleState(this);
            MovingState  = new MovingState(this);
            JumpingState = new JumpingState(this);

            _stateMachine.ChangeState(IdleState);
        }

        private void Update()
        {
            UpdateGroundStatus();

            _stateMachine.Tick();
        }

        public void ChangeState(IState newState)
        {
            _stateMachine.ChangeState(newState);
        }

        
        private void UpdateGroundStatus()
        {
            var ray = new Ray(transform.position, Vector3.down);
            IsGrounded = Physics.Raycast(ray, groundCheckDistance, groundLayer);
        }
    }
}
