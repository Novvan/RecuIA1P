using UnityEngine;

namespace RecuIA1P.Core.ScriptableObjects
{
	[CreateAssetMenu(fileName = "New Enemy Data", menuName = "ScriptableObjects/Enemies/Data", order = 1)]
	public class EnemyData : ScriptableObject
	{
		[Header("Settings")]
		public float rotationSpeed = 20f;
		public float patrolSpeed = 6f;
		public float chaseSpeed = 8f;
		public float hitBoxSize = 0.5f;
		public LayerMask playerLayer;

		[Header("Controller")]
		public float distanceToAttack = 2;
		public float timeToAttemptAttack = 2;
		public float idleLenght = 4.9f;
		public float minDistance = 0.5f;
		public float timeToOutOfAttack = 1f;

		[Header("Health")]
		public float maxHealth = 100;
	}
}
