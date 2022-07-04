using RecuIA1P.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RecuIA1P.UI
{
	public class ClickyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		[SerializeField] private Image image;
		[SerializeField] private Sprite defaultSprite, pressedSprite;
		[SerializeField] private AudioClip pressedButton, releasedButton;
		[SerializeField] private AudioSource audioSource;


		public void OnPointerDown(PointerEventData eventData)
		{
			image.sprite = pressedSprite;
			audioSource.PlayOneShot(pressedButton);
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			image.sprite = defaultSprite;
			audioSource.PlayOneShot(releasedButton);
		}

		public void QuitGame()
		{
			FindObjectOfType<FlowManager>().QuitGame();
		}
	}
}
