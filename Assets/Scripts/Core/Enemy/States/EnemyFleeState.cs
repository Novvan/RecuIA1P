using System;
using RecuIA1P.Core.AI.Decision_Tree;
using RecuIA1P.Core.AI.Finite_State_Machine;
using RecuIA1P.Core.AI.Steering_Behaviours;
using UnityEngine;

namespace RecuIA1P.Core.Enemy.States
{
	public class EnemyFleeState<T> : State<T>
	{
		private float _counter;
		private readonly float _timeToFleeing;
		private readonly Action<bool> _setFlee;
		private readonly INode _root;
		private readonly Action _onFinishFlee;
		private readonly Action<Vector3> _onMoveCommand;
		private readonly ISteering _steering;

		public EnemyFleeState(INode root, Action onFinishFlee, Action<bool> setFlee, float timeToFleeing, Action<Vector3> onMoveCommand,
			ISteering steering)
		{
			_root = root;
			_onFinishFlee = onFinishFlee;
			_setFlee = setFlee;
			_timeToFleeing = timeToFleeing;
			_onMoveCommand = onMoveCommand;
			_steering = steering;
		}

		private void ResetCounter()
		{
			_counter = _timeToFleeing;
		}

		public override void Awake()
		{
			_setFlee?.Invoke(true);
		}

		public override void Execute()
		{
			_counter -= Time.deltaTime;
			if (_counter > 0) return;


			_setFlee?.Invoke(false);
			_onFinishFlee?.Invoke();
			ResetCounter();
			_root.Execute();
		}
	}
}
