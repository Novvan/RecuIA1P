using System;
using RecuIA1P.Core.AI.Finite_State_Machine;
using UnityEngine;

namespace RecuIA1P.Core.Player.States
{
	public class PlayerShootState<T> : State<T>
	{
		private readonly T _transitionToIdle;
		private readonly T _transitionToMove;
		private readonly Action _onShootCommand;
		private readonly Func<bool> _isGrounded;
		private Action<bool> _isRunning;
		private readonly Action<Vector3> _onMoveCommand;

		public PlayerShootState(T transitionToIdle, T transitionToMove, Action onShootCommand, Action<Vector3> onMoveCommand, Func<bool> isGrounded,
			Action<bool> isRunning)
		{
			_transitionToIdle = transitionToIdle;
			_transitionToMove = transitionToMove;

			_onShootCommand = onShootCommand;
			_onMoveCommand = onMoveCommand;

			_isGrounded = isGrounded;
			_isRunning = isRunning;
		}

		public override void Awake()
		{
			if (_isGrounded())
				_onShootCommand?.Invoke();
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
