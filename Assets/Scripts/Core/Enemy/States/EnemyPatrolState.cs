using System;
using System.Collections.Generic;
using RecuIA1P.Core.AI.Decision_Tree;
using RecuIA1P.Core.AI.Finite_State_Machine;
using RecuIA1P.Core.Enemy.Components;
using RecuIA1P.Core.Utils;
using UnityEngine;

namespace RecuIA1P.Core.Enemy.States
{
	public class EnemyPatrolState<T> : State<T>
	{
		private readonly INode _rootNode;

		private readonly Action<Vector3> _onMoveCommand;
		private readonly Action _onPatrolCommand;
		private readonly Action<bool> _setIdleCommand;
		private readonly Func<bool> _attemptSeePlayer;

		private Transform _target;
		private readonly Transform[] _waypoints;

		private readonly Transform _enemyModel;

		private readonly float _minDistance;

		private Transform _currentWaypoint;
		private readonly HashSet<Transform> _visitedWaypoint = new HashSet<Transform>();

		public EnemyPatrolState(INode rootNode, Action<Vector3> onMoveCommand, Action onPatrolCommand, Action<bool> setIdleCommand,
			Func<bool> attemptSeePlayer, Transform target, Transform[] waypoints, EnemyModel enemyModel, float minDistance)
		{
			_rootNode = rootNode;

			_onMoveCommand = onMoveCommand;
			_onPatrolCommand = onPatrolCommand;
			_setIdleCommand = setIdleCommand;
			_attemptSeePlayer = attemptSeePlayer;

			_target = target;
			_waypoints = waypoints;

			_enemyModel = enemyModel.transform;


			_minDistance = minDistance;
		}

		public override void Awake()
		{
			if (_currentWaypoint == null)
			{
				_currentWaypoint = GetNextWaypoint();
			}

			_setIdleCommand?.Invoke(false);
			_onPatrolCommand?.Invoke();
		}

		public override void Execute()
		{
			Debug.Log("Patrol");
			var direction = _currentWaypoint.position - _enemyModel.position;

			direction.y = 0;

			_onMoveCommand?.Invoke(direction);

			var seePlayer = _attemptSeePlayer.Invoke();

			if (seePlayer)
			{
				_rootNode.Execute();
				return;
			}

			var distanceToWaypoint = Vector3.Distance(_enemyModel.position, _currentWaypoint.position);

			if (distanceToWaypoint > _minDistance) return;

			ResetWaypoint();

			_setIdleCommand?.Invoke(true);

			_rootNode.Execute();
		}

		public override void Sleep() => _setIdleCommand?.Invoke(true);

		private Transform GetNextWaypoint()
		{
			if (_visitedWaypoint.Count == _waypoints.Length) ClearVisitedWaypoint();
			var target = _waypoints.Rand();
			while (_visitedWaypoint.Contains(target))
			{
				target = _waypoints.Rand();
			}

			_visitedWaypoint.Add(target);
			return target;
		}

		private void ClearVisitedWaypoint() => _visitedWaypoint.Clear();

		private void ResetWaypoint()
		{
			_currentWaypoint = GetNextWaypoint();
		}
	}
}
