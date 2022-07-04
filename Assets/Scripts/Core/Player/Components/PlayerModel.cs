using System;
using RecuIA1P.Core.GenericControllers;
using RecuIA1P.Core.Interfaces;
using RecuIA1P.Core.ScriptableObjects;
using RecuIA1P.Core.Utils;
using RecuIA1P.Managers;
using UnityEngine;

namespace RecuIA1P.Core.Player.Components
{
	[AddComponentMenu("Player/Player Model")]
	[RequireComponent(typeof(Rigidbody))]
	public class PlayerModel : MonoBehaviour, IMove, IVel, IModel
	{
		[Header("Data Bindings")]
		[SerializeField] private EntityData playerData;

		[Header("Component Bindings")]
		[SerializeField] private Transform groundCheck, bulletSpawn, spawnpoint;
		[SerializeField] private GameObject _bullet;

		private PlayerView _playerView;
		private Rigidbody _rigidbody;
		private Transform _selfTransform;

		public LifeController LifeController { get; private set; }

		public float Vel { get; private set; }

		private float _currentSpeed, _currentLives, _jumpCooldownCounter, _currentHealth;
		private Vector3 _startingPoint;
		private Camera _cam;

		public event Action<float> OnMove;

		#region • Unity methods (3)

		private void Awake()
		{
			_playerView = GetComponent<PlayerView>();
			_rigidbody = GetComponent<Rigidbody>();
			_selfTransform = transform;

			_startingPoint = _selfTransform.position;
			_currentLives = playerData.maxLives;
			_currentHealth = playerData.maxHealth;

			ResetState();
			LifeController.OnDead += OnDeadHandler;

			_cam = Tools.Cam;
		}

		private void Update()
		{
			_jumpCooldownCounter -= Time.deltaTime;
		}

		#endregion

		public void SubscribeToEvents(PlayerController controller)
		{
			LifeController.OnDead += controller.HandleDeath;
			controller.OnJump += HandleJump;
			controller.OnMove += HandleMove;
			controller.OnReset += HandleReset;
			controller.OnShoot += HandleShoot;
		}

		private void HandleJump()
		{
			if (_jumpCooldownCounter > 0 || !IsGrounded()) return;

			_jumpCooldownCounter = playerData.jumpCooldown;
			_rigidbody.AddForce(Vector3.up * playerData.jumpHeight, ForceMode.Impulse);
		}

		private void HandleShoot()
		{
			var newBullet = Instantiate(_bullet, bulletSpawn.position, bulletSpawn.rotation);
			newBullet.GetComponent<Bullet>().owner = this.gameObject;
			Destroy(newBullet, 5);
		}

		public void HandleMove(Vector3 direction)
		{
			var normalizedDirection = direction.normalized;

			CorrectRotation(normalizedDirection);

			_rigidbody.MovePosition(_rigidbody.position + normalizedDirection * (_currentSpeed * Time.deltaTime));

			var dirMagnitude = normalizedDirection.magnitude;
			var moveMagnitude = _currentSpeed * dirMagnitude;

			Vel = moveMagnitude;

			OnMove?.Invoke(moveMagnitude);
		}

		private void HandleReset()
		{
			transform.position = _startingPoint;
			LifeController.Revive();
		}

		public void HandleRun(bool isRunSpeed) => _currentSpeed = isRunSpeed ? playerData.runSpeed : playerData.walkSpeed;

		private static void OnDeadHandler()
		{
			GameManager.Instance.Endgame(false);
		}

		private void ResetState()
		{
			_currentSpeed = playerData.walkSpeed;
			_jumpCooldownCounter = playerData.jumpCooldown;

			LifeController = new LifeController(playerData.maxLives, gameObject);
		}

		public bool IsGrounded() => Physics.CheckSphere(groundCheck.position, playerData.isGroundedRadius, playerData.groundMask);


		private void CorrectRotation(Vector3 moveDir)
		{
			var cameraRay = Tools.Cam.ScreenPointToRay(Input.mousePosition);
			var groundPlane = new Plane(Vector3.up, Vector3.zero);
			if (!groundPlane.Raycast(cameraRay, out var rayLength)) return;
			var pointToLook = cameraRay.GetPoint(rayLength);
			Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);
			transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
		}

		private void OnCollisionEnter(Collision other)
		{
			if (other.gameObject.CompareTag("Enemy"))
			{
				TakeDamage(100);
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.CompareTag("WinTrigger"))
			{
				GameManager.Instance.Endgame(true);
			}
		}

		public void TakeDamage(float dmg)
		{
			_currentHealth -= dmg;
			if (!(_currentHealth <= 0)) return;
			_currentLives--;
			_currentHealth = playerData.maxHealth;
			this.transform.position = spawnpoint.position;
			if (_currentLives <= 0) OnDeadHandler();
		}

#if UNITY_EDITOR
		private void OnDrawGizmosSelected()
		{
			if (playerData == null) return;

			if (groundCheck == null) return;
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(groundCheck.position, playerData.isGroundedRadius);
		}
#endif
	}
}
