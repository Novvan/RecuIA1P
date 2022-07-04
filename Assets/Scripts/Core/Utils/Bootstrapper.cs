using System;
using RecuIA1P.Managers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RecuIA1P.Core.Utils
{
	public class Bootstrapper : MonoBehaviour
	{
		private void Awake()
		{
			Execute();
		}

		private static void Execute()
		{
			CreateObject<GameManager>("Managers");
			CreateObject<FlowManager>("Managers");

			Debug.Log("<color=orange><b>Bootstrap:</b> executed</color>");
		}

		private static void CreateObject<T>(string parent) where T : MonoBehaviour
		{
			if (Object.FindObjectOfType<T>() != null) return;

			var par = GameObject.Find(parent);

			if (par == null) par = new GameObject(parent);

			par.transform.SetParent(null);
			par.AddComponent<T>();
		}
	}
}
