using System;
using RecuIA1P.Core.AI.Decision_Tree;
using RecuIA1P.Core.AI.Finite_State_Machine;
using UnityEngine;

namespace RecuIA1P.Core.Enemy.States
{
	public class EnemyChaseState<T> : State<T>
	{
		private readonly INode _rootNode;

		private readonly Action _onChaseCommand;
		private readonly Action<bool> _onIdleCommand;
		private readonly Action<Vector3> _onMoveCommand;

		private readonly float _timeToAttemptAttack;

		private readonly Transform _self, _target;

		private float _counter;

		public EnemyChaseState(INode rootNode, Action onChaseCommand, Action<bool> onIdleCommand, Action<Vector3> onMoveCommand, Transform target,
			Transform self, float timeToAttemptAttack)
		{
			_rootNode = rootNode;

			_onChaseCommand = onChaseCommand;
			_onIdleCommand = onIdleCommand;
			_onMoveCommand = onMoveCommand;

			_self = self;
			_target = target;

			_timeToAttemptAttack = timeToAttemptAttack;
		}

		public override void Awake()
		{
			_onChaseCommand?.Invoke();
			_onIdleCommand?.Invoke(false);
			ResetCounter();
		}

		public override void Execute()
		{
			var dir = (_target.position - _self.position).normalized;

			_onMoveCommand?.Invoke(dir);

			_counter -= Time.deltaTime;

			if (_counter > 0) return;

			_rootNode.Execute();
			_onIdleCommand?.Invoke(false);

			ResetCounter();
		}

		private void ResetCounter() => _counter = _timeToAttemptAttack;
	}
}
