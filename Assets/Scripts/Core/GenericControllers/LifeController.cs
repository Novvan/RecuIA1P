using System;
using UnityEngine;

namespace RecuIA1P.Core.GenericControllers
{
	public class LifeController
	{
		private float _currentLife;

		public float MaxLife { get; private set; }


		private GameObject _owner;

		public bool IsAlive => _currentLife > 0;

		public bool IsFullHealth => _currentLife == MaxLife;

		public float LifePercentage => _currentLife / MaxLife;

		public bool IsInvincible { get; set; }

		public float InvincibilityTimeStart { get; private set; }

		private float _invincibilityTime;

		public event Action OnDead;

		public event Action<float, float> OnGetDamage; //int 0 = currentLife, int 1 = damage

		public event Action<float, float> OnGetHeal; //int 0 = currentLife, int 1 = healing

		public event Action<float> OnRevive; //Life percentage


		public float CurrentLife
		{
			get => _currentLife;
			set
			{
				_currentLife = value;

				if (!IsAlive)
				{
					OnDead?.Invoke();
				}

				if (_currentLife > MaxLife)
				{
					_currentLife = MaxLife;
				}
				if (_currentLife < 0)
				{
					_currentLife = 0;
				}
			}
		}

		public void Revive()
		{
			_currentLife = MaxLife;
			OnRevive?.Invoke(LifePercentage);
		}

		public LifeController(float maxlife, GameObject owner, float invincibilityTimeStart = 1.5f)
		{
			this.MaxLife = maxlife;
			CurrentLife = maxlife;
			InvincibilityTimeStart = invincibilityTimeStart;
			_invincibilityTime = InvincibilityTimeStart;
			_owner = owner;
		}

		public void GetDamage(float damage, bool ignoresInvincibility = false)
		{
			if (!IsAlive) return;

			if ((!(_invincibilityTime >= InvincibilityTimeStart) || IsInvincible) && !ignoresInvincibility) return;
			CurrentLife -= damage;
			_invincibilityTime = 0;
			OnGetDamage?.Invoke(CurrentLife, damage);
		}


		public void GetHeal(float heal)
		{
			CurrentLife += heal;
			OnGetHeal?.Invoke(CurrentLife, heal);
		}


		public void Update()
		{
			_invincibilityTime += Time.deltaTime;
		}

		public void SetInvincibility(bool isInvincible)
		{
			this.IsInvincible = isInvincible;
		}
	}
}
