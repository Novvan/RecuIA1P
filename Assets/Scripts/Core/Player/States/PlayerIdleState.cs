using System;
using RecuIA1P.Core.AI.Finite_State_Machine;
using UnityEngine;

namespace RecuIA1P.Core.Player.States
{
	public class PlayerIdleState<T> : State<T>
	{
		private readonly T _transitionToDead, _transitionToMove, _transitionToJump, _transitionToShoot;

		private readonly Action<bool> _isRunning;

		public PlayerIdleState(T transitionToDead, T transitionToJump, T transitionToMove, T transitionToShoot, Action<bool> isRunning)
		{
			_transitionToDead = transitionToDead;
			_transitionToJump = transitionToJump;
			_transitionToMove = transitionToMove;
			_transitionToShoot = transitionToShoot;

			_isRunning = isRunning;
		}

		public override void Execute()
		{
			_isRunning?.Invoke(Input.GetKey(KeyCode.LeftShift));

			if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
				ParentStateMachine.Transition(_transitionToMove);

			if (Input.GetKeyDown(KeyCode.Space))
				ParentStateMachine.Transition(_transitionToJump);

			if (Input.GetKeyDown(KeyCode.Mouse0))
				ParentStateMachine.Transition(_transitionToShoot);
		}
	}
}
