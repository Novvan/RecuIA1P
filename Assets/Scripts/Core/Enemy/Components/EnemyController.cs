using System;
using RecuIA1P.Core.AI.Decision_Tree;
using RecuIA1P.Core.AI.Finite_State_Machine;
using RecuIA1P.Core.AI.Steering_Behaviours;
using RecuIA1P.Core.Enemy.States;
using RecuIA1P.Core.Player.Components;
using RecuIA1P.Core.ScriptableObjects;
using RecuIA1P.Managers;
using UnityEngine;

namespace RecuIA1P.Core.Enemy.Components
{
	[AddComponentMenu("AI/Enemy Controller")]
	[RequireComponent(typeof(EnemyModel), typeof(EnemyView))]
	public class EnemyController : MonoBehaviour
	{
		[Header("Data Bindings")]
		[SerializeField] private EnemyData enemyData;

		[Header("Component Bindings")]
		[SerializeField] private PlayerModel target;
		[Space]
		[SerializeField] private Transform[] waypoints;

		private EnemyModel _enemyModel;
		private INode _rootNode;
		private StateMachine<EnemyStates> _stateMachine;

		private bool _previousInSightState;
		private bool _currentInSightState;
		private bool _waitForIdleState;

		#region • Actions (6)

		public event Action OnAttack;

		public event Action OnChase;

		public event Action OnIdle;

		public event Action<Vector3> OnMove;

		public event Action OnPatrol;

		public event Action OnReset;

		#endregion

		#region • Unity methods (3)

		private void Awake()
		{
			_enemyModel = GetComponent<EnemyModel>();
			target = FindObjectOfType<PlayerModel>();
		}

		private void Start()
		{
			_enemyModel.SubscribeToEvents(this);

			InitializeDecisionTree();
			InitializeStateMachine();
		}

		private void Update()
		{
			if (GameManager.Instance.State != GameManager.GameState.Play) return;

			_stateMachine.UpdateState();
		}

		#endregion

		#region • Commands (6)

		private void OnAttackCommand() => OnAttack?.Invoke();

		private void OnChaseCommand() => OnChase?.Invoke();

		private void OnIdleCommand()
		{
			OnIdle?.Invoke();
			SetIdleStateCooldown(true);
		}

		private void OnMoveCommand(Vector3 moveDir) => OnMove?.Invoke(moveDir);

		private void OnPatrolCommand() => OnPatrol?.Invoke();

		public void OnResetCommand()
		{
			OnReset?.Invoke();
			SetIdleStateCooldown(true);

			_rootNode.Execute();
		}

		#endregion

		#region • AI methods (2)

		private void InitializeDecisionTree()
		{
			#region • Actions Nodes (4)

			var transitionToAttack = new ActionNode(() => _stateMachine.Transition(EnemyStates.Attack));
			var transitionToChase = new ActionNode(() => _stateMachine.Transition(EnemyStates.Chase));
			var transitionToIdle = new ActionNode(() => _stateMachine.Transition(EnemyStates.Idle));
			var transitionToPatrol = new ActionNode(() => _stateMachine.Transition(EnemyStates.Patrol));
			

			#endregion

			#region • Question Nodes (6)

			var checkIdleStateCooldown = new QuestionNode(IsIdleStateCooldown, transitionToIdle, transitionToPatrol);
			var didSightChangeToLose = new QuestionNode(SightStateChanged, transitionToIdle, checkIdleStateCooldown);
			var attemptPlayerKill = new QuestionNode(CheckPlayerIsInAtkRange, transitionToAttack, transitionToChase);
			var didSightChangeToAttack = new QuestionNode(SightStateChanged, transitionToChase, attemptPlayerKill);
			var isInSight = new QuestionNode(LastInSightState, didSightChangeToAttack, didSightChangeToLose);
			var isPlayerAlive = new QuestionNode(() => target.LifeController.IsAlive, isInSight, transitionToPatrol);

			#endregion

			_rootNode = isPlayerAlive;
		}

		private void InitializeStateMachine()
		{
			#region • States (4)

			var attackState = new EnemyAttackState<EnemyStates>(_rootNode, OnAttackCommand, SetIdleStateCooldown, enemyData.timeToOutOfAttack);
			var chaseState = new EnemyChaseState<EnemyStates>(_rootNode, OnChaseCommand, SetIdleStateCooldown, OnMoveCommand, target.transform,
				this.transform, enemyData.timeToAttemptAttack);
			var idleState = new EnemyIdleState<EnemyStates>(_rootNode, OnIdleCommand, SetIdleStateCooldown, CheckPlayerInSight, enemyData.idleLenght);
			var patrolState = new EnemyPatrolState<EnemyStates>(_rootNode, OnMoveCommand, OnPatrolCommand, SetIdleStateCooldown, CheckPlayerInSight,
				target.transform, waypoints, _enemyModel, enemyData.minDistance);

			#endregion

			#region • Transitions (8)

			attackState.AddTransition(EnemyStates.Chase, chaseState);
			attackState.AddTransition(EnemyStates.Idle, idleState);

			chaseState.AddTransition(EnemyStates.Idle, idleState);
			chaseState.AddTransition(EnemyStates.Attack, attackState);

			idleState.AddTransition(EnemyStates.Patrol, patrolState);
			idleState.AddTransition(EnemyStates.Chase, chaseState);

			patrolState.AddTransition(EnemyStates.Chase, chaseState);
			patrolState.AddTransition(EnemyStates.Idle, idleState);
			

			#endregion

			_stateMachine = new StateMachine<EnemyStates>(idleState);
		}

		#endregion

		private bool IsIdleStateCooldown() => _waitForIdleState;

		private void SetIdleStateCooldown(bool newState) => _waitForIdleState = newState;
		
		private bool SightStateChanged() => _currentInSightState != _previousInSightState;

		private bool LastInSightState()
		{
			_previousInSightState = _currentInSightState;
			_currentInSightState = _enemyModel.AILineOfSight.SingleTargetInSight(target.transform);

			return _currentInSightState;
		}

		private bool CheckPlayerInSight()
		{
			var playerIsInSight = _enemyModel.AILineOfSight.SingleTargetInSight(target.transform);

			return playerIsInSight;
		}
		
		private bool CheckPlayerIsInAtkRange()
		{
			var playerIsInAttackRange = _enemyModel.AtkLineOfSight.SingleTargetInSight(target.transform);

			return playerIsInAttackRange;
		}
	}
}
