using System;
using RecuIA1P.Core.AI.Decision_Tree;
using RecuIA1P.Core.AI.Finite_State_Machine;
using UnityEngine;

namespace RecuIA1P.Core.Enemy.States
{
	public class EnemyAttackState<T> : State<T>
	{
		private readonly INode _rootNode;

		private readonly Action _onAttackCommand;

		private readonly float _timeToOutOfAttack;
		private readonly Action<bool> _setIdleState;

		private float _counter;

		public EnemyAttackState(INode rootNode, Action onAttackCommand, Action<bool> setIdleState, float timeToOutOfAttack)
		{
			_rootNode = rootNode;

			_onAttackCommand = onAttackCommand;

			_timeToOutOfAttack = timeToOutOfAttack;
			_setIdleState = setIdleState;
		}

		public override void Awake()
		{
			_onAttackCommand?.Invoke();
			_setIdleState?.Invoke(false);
		}

		public override void Execute()
		{
			_counter -= Time.deltaTime;

			if (_counter > 0) return;
			_setIdleState?.Invoke(false);
			ResetCounter();
			_rootNode.Execute();
		}

		private void ResetCounter() => _counter = _timeToOutOfAttack;
	}
}
