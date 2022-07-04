using System;
using RecuIA1P.Core.AI.Finite_State_Machine;
using RecuIA1P.Core.Interfaces;
using RecuIA1P.Core.Player.States;
using RecuIA1P.Core.ScriptableObjects;
using RecuIA1P.Managers;
using UnityEngine;

namespace RecuIA1P.Core.Player.Components
{
	[RequireComponent(typeof(PlayerModel), typeof(PlayerView))]
	public class PlayerController : MonoBehaviour, IReset
	{
		[SerializeField] private EntityData playerData;
		private PlayerModel _playerModel;

		public event Action OnJump, OnShoot, OnReset;

		public event Action<Vector3> OnMove;

		#region • Unity methods (3)

		private void Awake()
		{
			_playerModel = GetComponent<PlayerModel>();
		}

		private void Start()
		{
			_playerModel.SubscribeToEvents(this);

			InitializeStateMachine();
		}

		private void Update()
		{
			if (GameManager.Instance.State != GameManager.GameState.Play) return;

			_stateMachine.UpdateState();
		}

		#endregion


		#region StateMachine

		private StateMachine<PlayerStates> _stateMachine;

		private void InitializeStateMachine()
		{
			#region • States (4)

			var deadState = new PlayerDeadState<PlayerStates>(PlayerStates.Idle, OnMoveCommand, playerData.respawnDelay);
			var idleState = new PlayerIdleState<PlayerStates>(PlayerStates.Dead, PlayerStates.Jump, PlayerStates.Move, PlayerStates.Shoot, IsRunning);
			var jumpState = new PlayerJumpState<PlayerStates>(PlayerStates.Idle, PlayerStates.Move, OnJumpCommand, OnMoveCommand, IsGrounded, IsRunning);
			var shootState =
				new PlayerShootState<PlayerStates>(PlayerStates.Idle, PlayerStates.Move, OnShootCommand, OnMoveCommand, IsGrounded, IsRunning);
			var moveState = new PlayerMoveState<PlayerStates>(PlayerStates.Idle, PlayerStates.Jump, PlayerStates.Shoot, OnMoveCommand, IsRunning);

			#endregion

			#region • Transitions (10)

			deadState.AddTransition(PlayerStates.Idle, idleState);
			
			shootState.AddTransition(PlayerStates.Dead, deadState);
			shootState.AddTransition(PlayerStates.Idle, idleState);
			shootState.AddTransition(PlayerStates.Move, moveState);
			
			idleState.AddTransition(PlayerStates.Dead, deadState);
			idleState.AddTransition(PlayerStates.Jump, jumpState);
			idleState.AddTransition(PlayerStates.Move, moveState);
			idleState.AddTransition(PlayerStates.Shoot, shootState);

			jumpState.AddTransition(PlayerStates.Dead, deadState);
			jumpState.AddTransition(PlayerStates.Idle, idleState);
			jumpState.AddTransition(PlayerStates.Move, moveState);

			moveState.AddTransition(PlayerStates.Dead, deadState);
			moveState.AddTransition(PlayerStates.Idle, idleState);
			moveState.AddTransition(PlayerStates.Jump, jumpState);
			moveState.AddTransition(PlayerStates.Shoot, shootState);

			#endregion

			_stateMachine = new StateMachine<PlayerStates>(idleState);
		}

		#endregion

		#region • Commands (3)

		private void OnMoveCommand(Vector3 moveDirection) => OnMove?.Invoke(moveDirection);

		private void OnJumpCommand() => OnJump?.Invoke();

		private void OnShootCommand() => OnShoot?.Invoke();

		public void OnResetCommand()
		{
			OnReset?.Invoke();
			_stateMachine.Transition(PlayerStates.Idle);
		}

		#endregion

		public void HandleDeath() => _stateMachine.Transition(PlayerStates.Dead);

		private void IsRunning(bool runState) => _playerModel.HandleRun(runState);

		private bool IsGrounded() => _playerModel.IsGrounded();
	}
}
