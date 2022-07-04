using System;
using RecuIA1P.Core.AI.Finite_State_Machine;
using UnityEngine;

namespace RecuIA1P.Core.Player.States
{
	public class PlayerMoveState<T> : State<T>
	{
		private readonly T _transitionToIdle, _transitionToJump, _transitionToShoot;

		private readonly Action<Vector3> _onMoveCommand;
		private readonly Action<bool> _isRunning;

		public PlayerMoveState(T transitionToIdle, T transitionToJump, T transitionToShoot, Action<Vector3> onMoveCommand, Action<bool> isRunning)
		{
			_transitionToIdle = transitionToIdle;
			_transitionToJump = transitionToJump;
			_transitionToShoot = transitionToShoot;

			_onMoveCommand = onMoveCommand;

			_isRunning = isRunning;
		}

		public override void Execute()
		{
			var horizontalInput = Input.GetAxisRaw("Horizontal");
			var verticalInput = Input.GetAxisRaw("Vertical");

			_isRunning?.Invoke(Input.GetKey(KeyCode.LeftShift));

			var moveVector = new Vector3(horizontalInput, 0, verticalInput);

			_onMoveCommand?.Invoke(moveVector);

			if (horizontalInput == 0 && verticalInput == 0)
			{
				ParentStateMachine.Transition(_transitionToIdle);
				return;
			}

			if (Input.GetKeyDown(KeyCode.Space))
				ParentStateMachine.Transition(_transitionToJump);
			
			if (Input.GetKeyDown(KeyCode.Mouse0))
				ParentStateMachine.Transition(_transitionToShoot);
		}
	}
}
