using System;
using UnityEngine;

namespace RecuIA1P.Managers
{
	public class UIManager : MonoBehaviour
	{
		[SerializeField] private GameObject winScreen, gameScreen, loseScreen;

		private void Start()
		{
			GameManager.Instance.Startgame();
		}

		private void OnEnable()
		{
			GameManager.Instance.OnStateChanged += ManageCanvas;
		}

		private void OnDisable()
		{
			GameManager.Instance.OnStateChanged -= ManageCanvas;
		}

		private void ManageCanvas(GameManager.GameState state)
		{
			switch (state)
			{
				case GameManager.GameState.Finish:
					gameScreen.SetActive(false);
					winScreen.SetActive(true);
					break;
				case GameManager.GameState.Lose:
					gameScreen.SetActive(false);
					loseScreen.SetActive(true);
					break;
				default:
					break;
			}
		}
	}
}
