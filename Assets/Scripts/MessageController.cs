using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class MessageController : MonoBehaviour {
	public Text Message;
	public Button RestartButton;

	public Queue<string> Messages = new Queue<string>();

	public bool PokemonHasFainted = false;

	public void ShowMessage () {
		if (Messages.Count > 0) {
			gameObject.SetActive(true);
			Message.text = Messages.Dequeue();
		} else if (PokemonHasFainted) {
			ShowRestartButton();
		} else {
			HideMessage();
		}
	}

	public void HideMessage () {
		gameObject.SetActive(false);
	}

	public void AddMessage (string message) {
		Messages.Enqueue(message);

		if (!MessageShown()) {
			ShowMessage();
		}
	}

	public void ShowRestartButton () {
		RestartButton.gameObject.SetActive(true);
		Message.gameObject.SetActive(false);
	}

	public void Restart () {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	bool MessageShown () {
		return gameObject.activeSelf;
	}
}
