using System;
using System.Collections.Generic;
using RecuIA1P.Core.AI.Line_of_Sight;
using RecuIA1P.Core.AI.RouletteWheel;
using RecuIA1P.Core.GenericControllers;
using RecuIA1P.Core.Interfaces;
using RecuIA1P.Core.ScriptableObjects;
using UnityEngine;

namespace RecuIA1P.Core.Enemy.Components
{
	[RequireComponent(typeof(Rigidbody), typeof(AILineOfSight))]
	public class EnemyModel : MonoBehaviour, IModel
	{
		[Header("Data Bindings")]
		[SerializeField] private EnemyData data;
		[SerializeField] private AILineOfSight aiLineOfSight, atkLineOfSight;

		[Space]
		[SerializeField] private GameObject bullet;
		[SerializeField] private Transform bulletSpawn;


		private Rigidbody _rigidbody;
		private float _currentSpeed, _currentHealth;
		private Vector3 _startingPosition;
		private Dictionary<Action, float> _attackProbs;

		public float CurrentHealth => _currentHealth;

		public float Vel => _rigidbody.velocity.magnitude;

		public AILineOfSight AILineOfSight => aiLineOfSight;

		public AILineOfSight AtkLineOfSight => atkLineOfSight;

		private Action _callToShoot, _callToCatchSomeBreath;


		#region • Unity methods (2)

		private void Awake()
		{
			_rigidbody = GetComponent<Rigidbody>();

			_startingPosition = transform.position;
			_currentHealth = data.maxHealth;
		}

		private void Start()
		{
			_attackProbs = new Dictionary<Action, float>{
				{_callToCatchSomeBreath, 50},
				{_callToShoot, 125}
			};
		}

		#endregion

		#region • Custom methods (1)

		public void SubscribeToEvents(EnemyController controller)
		{
			controller.OnAttack += HandleAttack;
			controller.OnChase += HandleChase;
			controller.OnIdle += HandleIdle;
			controller.OnMove += HandleMove;
			controller.OnPatrol += HandlePatrol;
			controller.OnReset += HandleReset;

			this._callToCatchSomeBreath += CatchSomeBreath;
			this._callToShoot += ShootPlayer;
		}

		#endregion

		#region • Commands (6)

		private void HandleAttack()
		{
			//Call for roulette
			var action = RouletteWheel.Run(_attackProbs);
			
			Debug.Log(action.ToString());
			
			action?.Invoke();
		}

		private void HandleChase() => _currentSpeed = data.chaseSpeed;

		private void HandleIdle()
		{
			_rigidbody.velocity = Vector3.zero;
			HandleMove(Vector3.zero);
		}

		private void HandleMove(Vector3 direction)
		{
			direction = direction.normalized;
			_rigidbody.velocity = direction * _currentSpeed;

			if (direction == Vector3.zero) return;

			transform.forward = Vector3.Lerp(transform.forward, direction, Time.deltaTime * data.rotationSpeed);
		}

		private void HandlePatrol() => _currentSpeed = data.patrolSpeed;

		private void HandleReset()
		{
			transform.position = _startingPosition;
			HandleMove(Vector3.zero);
		}

		public void TakeDamage(float dmg)
		{
			_currentHealth -= dmg;

			if (_currentHealth <= 0)
			{
				Destroy(this.gameObject);
			}
		}

		private void ShootPlayer()
		{
			var newBullet = Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
			newBullet.GetComponent<Bullet>().owner = this.gameObject;

			Destroy(newBullet, 5);
		}

		private void CatchSomeBreath()
		{
			Debug.Log("Catching some breath");
		}

		#endregion
	}
}
