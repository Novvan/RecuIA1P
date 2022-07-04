using System;
using RecuIA1P.Core.Utils;
using UnityEngine;

namespace RecuIA1P.Core.Player.Components
{
	public class PlayerView : MonoBehaviour
	{
		public static int PosId = Shader.PropertyToID("_PlayerPosition");
		public static int SizeId = Shader.PropertyToID("_Size");

		public LayerMask seeThroughMask;
		public Material wallMaterial;
		private Camera _camera;

		private void Awake()
		{
			_camera = Tools.Cam;
		}

		private void Update()
		{
			var ts = transform.position;

			var dir = _camera.transform.position - ts;
			var ray = new Ray(ts, dir.normalized);

			wallMaterial.SetFloat(SizeId, Physics.Raycast(ray, 3000, seeThroughMask) ? 1 : 0);

			var view = _camera.WorldToViewportPoint(ts);
			wallMaterial.SetVector(PosId, view);
		}
	}
}
