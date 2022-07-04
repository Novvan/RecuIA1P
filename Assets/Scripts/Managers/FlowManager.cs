using UnityEditor;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace RecuIA1P.Managers
{
	public class FlowManager : MonoBehaviour
	{
		public void QuitGame()
		{
#if UNITY_EDITOR
			EditorApplication.ExitPlaymode();
#else
			Application.Quit();
#endif
		}


		public void ToScene(string scene)
		{
			GameManager.Instance.Startgame();
			SceneManager.LoadScene(scene, LoadSceneMode.Single);
		}
	}
}
