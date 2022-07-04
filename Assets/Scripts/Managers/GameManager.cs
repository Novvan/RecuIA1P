using System;
using UnityEngine;

namespace RecuIA1P.Managers
{
	public class GameManager : MonoBehaviour
	{
		public static GameManager Instance;

		public enum GameState
		{
			Menu,
			Play,
			Finish,
			Lose
		}

		public Action<GameState> OnStateChanged;
		
		public GameState State { get; private set; }

		private void Awake()
		{
			if (Instance != null)
			{
				Destroy(gameObject);
				return;
			}
			else
			{
				Instance = this;
				DontDestroyOnLoad(this);

				Application.targetFrameRate = 144;
			}
		}


		private void Start()
		{
			SetState(GameState.Menu);
		}

		private void SetState(GameState state)
		{
			State = state;
			OnStateChanged?.Invoke(State);
		}

		public void Endgame(bool isVictory) => SetState(isVictory ? GameState.Finish : GameState.Lose);

		public void Startgame() => SetState(GameState.Play);
	}
}
