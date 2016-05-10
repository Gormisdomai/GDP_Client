using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Scenes : MonoBehaviour {

	public GameObject leftCollider;
	private bool paused = false;

	public void ReloadScene() {
		SceneManager.LoadScene("Test_Scene");
	}

	public void ToMainMenu() {
		SceneManager.LoadScene("MainMenu");
	}

	private void Update()
	{
		// temporary controls, will add gui

		// back to menu
		if (Input.GetKeyDown("backspace")) {
			// maybe shouldnt register score here in future
			GetComponent<Death>().RegisterScore();
			Time.timeScale = 1.0f;
			ToMainMenu();
		}

		// pause
		if (Input.GetKeyDown("escape")) {
    		if (paused) {
        		Time.timeScale = 1.0f;
				paused = false;
			}
    		else if (Time.timeScale == 1) {
        		Time.timeScale = 0.0f;
				paused = true;
			}
		}
	}

	private void Awake() {
		float screenPos = GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0,0.5f,0)).x;
		Instantiate(leftCollider, new Vector2(screenPos, 0), Quaternion.identity);
		print("asdf asdfas dfasd fasd");
	}
}
