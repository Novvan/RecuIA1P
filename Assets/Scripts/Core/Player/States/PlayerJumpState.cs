using System;
using RecuIA1P.Core.AI.Finite_State_Machine;
using UnityEngine;

namespace RecuIA1P.Core.Player.States
{
    public class PlayerJumpState<T> : State<T>
    {
        private readonly T _transitionToIdle;
        private readonly T _transitionToMove;
        private readonly Action _onJumpCommand;
        private readonly Func<bool> _isGrounded;
        private Action<bool> _isRunning;
        private readonly Action<Vector3> _onMoveCommand;
        public PlayerJumpState (T transitionToIdle, T transitionToMove, Action onJumpCommand, Action <Vector3> onMoveCommand,  Func <bool> isGrounded, Action <bool> isRunning)
        {
            _transitionToIdle = transitionToIdle;
            _transitionToMove = transitionToMove;
            
            _onJumpCommand = onJumpCommand;
            _onMoveCommand = onMoveCommand;
            
            _isGrounded = isGrounded;
            _isRunning  = isRunning;
        }

        public override void Awake()
        {
            _onJumpCommand?.Invoke();
        }

        public override void Execute()
        {
            var horizontalInput = Input.GetAxisRaw("Horizontal");
            var verticalInput = Input.GetAxisRaw("Vertical");

            var moveDirection = new Vector3(horizontalInput, 0, verticalInput);

            _onMoveCommand?.Invoke(moveDirection);

            if (_isGrounded())
                ParentStateMachine.Transition(moveDirection != Vector3.zero ? _transitionToMove : _transitionToIdle);
        }
    }
}