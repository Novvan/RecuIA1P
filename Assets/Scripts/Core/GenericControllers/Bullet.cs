using RecuIA1P.Core.Interfaces;
using UnityEngine;

namespace RecuIA1P.Core.GenericControllers
{
	public class Bullet : MonoBehaviour
	{
		[SerializeField] private float speed = 5, damage = 25;

		public GameObject owner;

		private void Update()
		{
			transform.Translate(Vector3.forward * speed * Time.deltaTime);
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject == owner) return;
			if (!other.CompareTag("Enemy") && !other.CompareTag("Player")) return;
			Debug.Log("Hit");
			other.GetComponent<IModel>().TakeDamage(damage);
			Destroy(this.gameObject);
		}
	}
}
