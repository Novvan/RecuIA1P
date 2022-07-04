using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace RecuIA1P.Core.Utils
{
	public static class Tools
	{
		#region Helpers

		// Camera caching

		private static Camera _cam;

		/// <summary>
		/// Returns a cached main camera.
		/// </summary>
		public static Camera Cam
		{
			get
			{
				if (_cam == null)
					_cam = Camera.main;
				return _cam;
			}
		}


		// Is Mouse Over UI

		private static PointerEventData _eventDataCurrentPosition;
		private static List<RaycastResult> _results;


		/// <summary>
		/// Returns true if the mouse is over the UI.
		/// </summary>
		public static bool IsOverUI()
		{
			_eventDataCurrentPosition = new PointerEventData(EventSystem.current)
				{position = Mouse.current.position.ReadValue()};
			_results = new List<RaycastResult>();
			EventSystem.current.RaycastAll(_eventDataCurrentPosition, _results);
			return _results.Count > 0;
		}


		// Cursor Handler

		/// <summary>
		/// Turns the cursor on or off.
		/// </summary>
		public static void ToggleCursor(bool state)
		{
			Cursor.visible = state;
			Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
		}

		#endregion

		#region Extensions

		// Delete All Children
		/// <summary>
		/// Deletes all the children of a transform.
		/// </summary>
		public static void DeleteAllChildren(this Transform t)
		{
			foreach (Transform child in t)
				Object.Destroy(child.gameObject);
		}


		// Random from list
		/// <summary>
		/// Gets a random item from a List.
		/// </summary>
		public static T Rand<T>(this IList<T> list)
		{
			return list[Random.Range(0, list.Count)];
		}

		// Random from dictionary

		/// <summary>
		/// Gets a random item from a dictionary. It's key and value ara available ass out parameters.
		/// </summary>
		public static void Rand<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, out TKey key, out TValue value)
		{
			var keys = new List<TKey>(dictionary.Keys);
			key = keys[Random.Range(0, keys.Count)];
			value = dictionary[key];
		}

		#endregion
	}
}
